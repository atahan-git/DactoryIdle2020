﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds all the building data. This will be passed around the RecipeSets, building world objects, etc.
/// </summary>
[CreateAssetMenu(fileName = "New Building Data", menuName = "Building Data")]
public class BuildingData : ScriptableObject {
	public string uniqueName = "Base_newbuilding"; // The universal identifier. 

	public ArrayLayout shape;
	public static Vector2 center = new Vector2(3, 3);

	// Make sure to also add new crafting type to RecipeSet/CraftingNode if you are adding new crafting type building!
	public enum ItemType { Belt, Miner, Furnace, ProcessorSingle, ProcessorDouble, Press, Coiler, Cutter, Lab, Building, Base, Decal, Storage, Connector, Spaceship, ShipPart }

	public ItemType myType;

	[Tooltip("Grade 1 factory will only be able to process grade 1 items etc. Also determines miner range")]
	public int buildingGrade = 0;

	public float energyUse = 0;

	public enum BuildingGfxType {
		SpriteBased, AnimationBased, PrefabBased
	}

	[Header("In the world, only one of these will be used")]
	public BuildingGfxType gfxType = BuildingGfxType.SpriteBased;
	[Header("but always set the sprite for UI uses")]
	public Sprite gfxSprite;
	[Space]
	public Sprite gfxShadowSprite;
	public bool isAnimatedShadow = false;
	public SpriteAnimationHolder gfxSpriteAnimation;
	public GameObject gfxPrefab;
	public Vector2 spriteOffset = new Vector2(0.5f, 0.5f);

	public bool playerBuildBarApplicable = true;
}
