﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_InventoryController : MonoBehaviour {

    public static bool isExtraInfoVisible = false;

    public Transform BuildingsParent;
    public GameObject BuildingListingPrefab;
    GUI_BuildingBarController bbar;

    public Transform InventoryParent;
    public GameObject InventoryListingPrefab;
    Player_InventoryController pcont;

    // Start is called before the first frame update
    void Start () {
        if (Player_InventoryController.isInventoryLoadingDone)
            DrawInventory();
        else
            Player_InventoryController.drawInventoryEvent += DrawInventory;

        if (PlayerPrefs.GetInt("extrainfo", 0) == 1) {
            ToggleExtraInfo();
        }
    }

    void DrawInventory () {
        print("Drawing Inventory");
        bbar = GetComponent<GUI_BuildingBarController>();
        foreach (BuildingData dat in DataHolder.s.AllBuildings()) {
            if(dat.playerBuildable)
                Instantiate(BuildingListingPrefab, BuildingsParent).GetComponent<MiniGUI_BuildingListing>().SetUp(dat, bbar);
        }
        print(DataHolder.s.AllBuildings().Length.ToString() + " Buildings are put into building list");

        pcont = transform.parent.GetComponentInChildren<Player_InventoryController>();
        foreach (InventoryItemSlot it in pcont.mySlots) {
            Instantiate(InventoryListingPrefab, InventoryParent).GetComponent<MiniGUI_InventoryListing>().SetUp(it, this);
        }
        print(pcont.mySlots.Count.ToString() + " ItemSlots are drawn");
    }

    void OnDestroy() {
        Player_InventoryController.drawInventoryEvent -= DrawInventory;
    }

    public void ToggleExtraInfo () {
		isExtraInfoVisible = !isExtraInfoVisible;
        BuildingInfoDisplay.isExtraInfoVisible = isExtraInfoVisible;
        PlayerPrefs.SetInt("extrainfo", isExtraInfoVisible ? 1 : 0);
    }
}

