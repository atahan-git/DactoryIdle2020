﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class BeltMaster : MonoBehaviour {
	public static BeltMaster s;

	[SerializeField] protected List<BeltObject> allBelts = new List<BeltObject>();
	[SerializeField] protected List<BeltPreProcessor.BeltGroup> beltGroups = new List<BeltPreProcessor.BeltGroup>();

	[SerializeField] protected List<BeltItem> allBeltItems = new List<BeltItem>();

	protected BeltPreProcessor beltPreProc;
	protected BeltItemSlotUpdateProcessor beltItemSlotProc;

	public bool autoStart = true;
	public bool debugDraw = false;

	public const float beltUpdatePerSecond = 4;
	public const float itemWorldPositionZOffset = -1f;

	public ObjectPoolSimple<BeltItem> itemPool;
	[HideInInspector]
	 public ObjectPool entityPool; //refactor this asap pls, belt item slot should not access this


	protected List<MagicItemCreator> allCreators = new List<MagicItemCreator>();
	protected List<MagicItemDestroyer> allDestroyers = new List<MagicItemDestroyer>();

	public int maxItemCount = 4000;

	EntityManager entityManager;
	// Start is called before the first frame update
	void Start() {
		s = this;
		entityManager = World.Active.EntityManager;

		if (autoStart)
			StartBeltSystem();
	}

	public void StartBeltSystem () {
		print("Starting Belt System");
		itemPool = new ObjectPoolSimple<BeltItem>(maxItemCount, maxItemCount);
		itemPool.SetUp();

		entityPool = GetComponent<ObjectPool>();

		SetupBeltSystem();

		CreateGfxs();
		StartBeltSystemLoops();


		allCreators = new List<MagicItemCreator>(FindObjectsOfType<MagicItemCreator>());
		allDestroyers = new List<MagicItemDestroyer>(FindObjectsOfType<MagicItemDestroyer>());
	}

	void CreateGfxs () {
		
		for (int i = 0; i < allBelts.Count; i++) {
			allBelts[i].UpdateGraphics();
		}
	}

	public void SetupBeltSystem () {
		allBelts = new List<BeltObject>(FindObjectsOfType<BeltObject>());

		for (int i = 0; i <  allBelts.Count; i++) {
			BeltObject belt = allBelts[i];
			belt.SetPosBasedOnWorlPos();
		}

		beltPreProc = new BeltPreProcessor(beltGroups, allBeltItems, GetBeltAtLocation);

		beltPreProc.PrepassBelts(allBelts);

		beltItemSlotProc = new BeltItemSlotUpdateProcessor(itemPool, beltGroups);
	}

	public void StartBeltSystemLoops () {
		StartCoroutine(BeltItemSlotUpdateLoop());
	}

	public void AddOneBeltConnectedToOne (BeltObject newBelt, BeltObject updatedBelt) {
		allBelts.Add(newBelt);
		beltPreProc.UpdateOneBeltBeltSlots(newBelt);
		beltPreProc.UpdateOneBeltBeltSlots(updatedBelt);
		ProcessBeltGroupingChange(newBelt);
	}

	public void AddOneBelt (BeltObject newBelt) {
		allBelts.Add(newBelt);
		beltPreProc.UpdateOneBeltBeltSlots(newBelt);
		ProcessBeltGroupingChange(newBelt);
	}

	public void ChangeOneBelt (BeltObject updatedBelt) {
		beltPreProc.UpdateOneBeltBeltSlots(updatedBelt);
		ProcessBeltGroupingChange(updatedBelt);
	}

	private void ProcessBeltGroupingChange (BeltObject updatedBelt) {
		beltPreProc.ProcessBeltGroupingChange(updatedBelt);
	}

	IEnumerator BeltItemSlotUpdateLoop () {
		while (true) {
			beltItemSlotProc.UpdateBeltItemSlots();

			ApplyPositionsToEntities();

			CreateItems();

			DestroyItems();

			//BeltItemGfxUpdateProcessor.UpdateBeltItemPositions();

			yield return new WaitForSeconds(1f / beltUpdatePerSecond);
		}
	}

	void ApplyPositionsToEntities () {
		for (int i = 0; i < itemPool.objectPool.Length; i++) {
			if (itemPool.objectPool[i].isMovedThisLoop) {
				float3 pos = new float3(
				BeltMaster.s.itemPool.objectPool[i].myRandomOffset.x + BeltMaster.s.itemPool.objectPool[i].mySlot.position.x,
				BeltMaster.s.itemPool.objectPool[i].myRandomOffset.y + BeltMaster.s.itemPool.objectPool[i].mySlot.position.y,
				BeltMaster.itemWorldPositionZOffset);

				entityManager.SetComponentData(BeltMaster.s.entityPool.GetEntity(i), new ItemMovement { targetWithOffset = pos });
			}
		}
	}

	void CreateItems () {
		for (int i = 0; i < allCreators.Count; i++) {
			allCreators[i].CreateItemsBasedOnTick();
		}
	}

	void DestroyItems () {
		for (int i = 0; i < allDestroyers.Count; i++) {
			allDestroyers[i].DestroyItemsOnSlots();
		}
	}


	float timer = 500f;
	float maxTime = 500f;
	// Update is called once per frame
	void Update () {
		if (debugDraw) {
			if (timer > maxTime) {
				foreach (BeltPreProcessor.BeltGroup beltGroup in beltGroups)
					foreach (List<BeltItemSlot> beltItemSlotGroup in beltGroup.beltItemSlotGroups)
						foreach (BeltItemSlot beltItemSlot in beltItemSlotGroup)
							beltItemSlot.DebugDraw();
				timer = 0;


			}


			/*for (int i = 0; i < itemPool.objectpool.Length; i++)
				itemPool.objectpool[i].DebugDraw();*/
		}


		timer += Time.deltaTime;
		//print("####");
		//print(beltGroups.Count);
		//print(beltGroups[0].beltItemSlotGroups.Count);
		//print(beltGroups[0].beltItemSlotGroups[0].Count);
	}

	protected BeltObject GetBeltAtLocation (Position pos) {
		BeltObject belt = null;
		try {
			belt = Grid.s.GetTile(pos).myItem.GetComponent<BeltObject>();
		} catch { }
		return belt;
	}


	public int activeItemCount = 0;
	public bool CreateItemAtBeltSlot (BeltItemSlot slot /*, int itemTyep*/) {
		if (slot != null) {
			if (slot.myItem == null) {
				slot.myItem = itemPool.Spawn();
				slot.myItem.myEntityId = entityPool.Spawn(slot.position, slot.position);
				activeItemCount++;
				return true;
			} 
		}

		return false;
	}

	public void DestroyItemAtSlot (BeltItemSlot slot) {
		if (slot != null) {
			if (slot.myItem != null) {
				entityPool.DestroyPooledObject(slot.myItem.myEntityId);
				itemPool.DestroyPooledObject(slot.myItem);
				slot.myItem = null;
				activeItemCount--;
			}	
		}
	}
}
