﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;




/// <summary>
/// Controls the crafting UI panel
/// OBSOLETE - PLAYER CANNOT CRAFT FROM INVENTORY ANYMORE
/// </summary>
public class GUI_CraftingController : MonoBehaviour {
    private List<CraftingNode> allCraftingProcesses = new List<CraftingNode>();

    public Transform CraftingDisplayParent;
    public GameObject CraftingDisplayPrefab;

    public Transform CraftingQueueParent;
    public GameObject CraftingQueuePrefab;
    Queue<MiniGUI_CraftingQueueDisplay> CraftingQueue = new Queue<MiniGUI_CraftingQueueDisplay>();

    /*void Start() {
        var dividedCraftingProcesses = DataHolder.s.GetAllCraftingProcessNodesDivided();
        for (int i = 1; i < dividedCraftingProcesses.Length; i++) {
            if (i != DataHolder.s.CraftingNodeTypeToIndex(CraftingNode.cTypes.Building)) // We do not want to have building crafting
                allCraftingProcesses.AddRange(dividedCraftingProcesses[i]);
        }

        for (int i = 0; i < allCraftingProcesses.Count; i++) {
            ;
            var disp = Instantiate(CraftingDisplayPrefab, CraftingDisplayParent);
            disp.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DataHolder.s.GetItem(DataHolder.s.GetConnections(allCraftingProcesses[i], true)[0].itemUniqueName).GetSprite();
            disp.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = DataHolder.s.GetConnections(allCraftingProcesses[i], true)[0].count.ToString();

            if (DataHolder.s.GetConnections(allCraftingProcesses[i], true).Count > 1) {
                disp.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = DataHolder.s.GetItem(DataHolder.s.GetConnections(allCraftingProcesses[i], true)[1].itemUniqueName).GetSprite();
                disp.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = DataHolder.s.GetConnections(allCraftingProcesses[i], true)[1].count.ToString();
            } else {
                disp.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
                disp.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }

            disp.transform.GetChild(3).GetComponent<Text>().text = allCraftingProcesses[i].timeCost.ToString() + " s";

            try {
                disp.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = DataHolder.s.GetItem(DataHolder.s.GetConnections(allCraftingProcesses[i], false)[0].itemUniqueName).GetSprite();
                disp.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = DataHolder.s.GetConnections(allCraftingProcesses[i], false)[0].count.ToString();
            } catch {
                Debug.LogError("Crafting process doesn't have output!");
            }


            int x = new int();
            x = i;
            disp.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(delegate { CraftItem(x); });
        }

    }
    */

    public void CraftItem(int craftingProcessIndex) {

        /*//if (Player_InventoryController.s.CanCraftItem(allCraftingProcesses[craftingProcessIndex])) {
        if (true) {
            print("Crafting item " + craftingProcessIndex.ToString());
            var newCraftingProcess = Instantiate(CraftingQueuePrefab, CraftingQueueParent).GetComponent<MiniGUI_CraftingQueueDisplay>();
            newCraftingProcess.transform.SetAsFirstSibling();

            newCraftingProcess.SetUp(allCraftingProcesses[craftingProcessIndex], this);
            CraftingQueue.Enqueue(newCraftingProcess);
        } else {
            print("Cannot craft item " + craftingProcessIndex.ToString() + " - not enough resources");
        }*/
    }

    public void CancelCraftItem(MiniGUI_CraftingQueueDisplay target) {
        /*if (target == curActiveProcess) {
            curActiveProcess.DestroySelf();
            curActiveProcess = null;
        } else {
            CraftingQueue = new Queue<MiniGUI_CraftingQueueDisplay>(CraftingQueue.Where(x => x != target));
            target.DestroySelf();
        }*/
    }

    /*private MiniGUI_CraftingQueueDisplay curActiveProcess;
    private void Update() {
        if (curActiveProcess == null) {
            if (CraftingQueue.Count > 0) {
                curActiveProcess = CraftingQueue.Dequeue();
            }
        } else {
            curActiveProcess.progress += (1f / curActiveProcess.timeReq) * Time.deltaTime;
            curActiveProcess.UpdateDisplay();
            if (curActiveProcess.progress >= 1) {
                /*if (Player_InventoryController.s.CanCraftItem(curActiveProcess.myCraftingNode)) {
                    if (Player_InventoryController.s.TryAndAddItem(curActiveProcess.myItem)) {
                        Player_InventoryController.s.UseCraftingResources(curActiveProcess.myCraftingNode, 1);
                        print("Item Craftin success: " + curActiveProcess.myItem.uniqueName);
                    }
                }#1#
                print("Item Crafting failed: " + curActiveProcess.myItem.uniqueName);

                curActiveProcess.DestroySelf();
                curActiveProcess = null;
            }
        }
    }*/

}
