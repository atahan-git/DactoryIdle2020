﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A helper class for the individual building listing in the inventoryItemSlots panel
/// </summary>
public class MiniGUI_BuildingListing : MonoBehaviour {

    public Text nameText;
    public Image img;
    public BuildingData myDat;
    GUI_BuildingBarController myCont;

    public void SetUp (BuildingData _myDat, GUI_BuildingBarController _myCont) {
        myDat = _myDat;
        nameText.text = myDat.name;
        img.sprite = myDat.gfxSprite;
        myCont = _myCont;
    }

    public void ChangeBuildableState (bool state) {
        if (state) {
            img.color = new Color(1, 1, 1);
        } else {
            float darkness = 0.5f;
            img.color = new Color(darkness, darkness, darkness);
        }
    }


    public void BeginDrag() {
        Debug.Log("Begin Building Listing Drag" + myDat);
        if (myDat != null)
            myCont.BeginDragInventoryBuilding(myDat);
    }
}
    