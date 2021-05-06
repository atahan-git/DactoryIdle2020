﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

//namespace Atahan.NodeItemTree {
    public class NodeItemTreeMakerMaster : MonoBehaviour {
        /*public static NodeItemTreeMakerMaster s;

        public RecipeSet myRecipeSet;

        public RectTransform ItemsParent;
        public GameObject ItemListItemPrefab;

        public RectTransform NodeParent;
        public GameObject ItemNodePrefab;
        public GameObject CraftingNodePrefab;

        [HideInInspector]
        public Camera mycam;
        public Camera linesCam;

        public List<NodeGfx> allNodeGfxs = new List<NodeGfx>();

        public RectTransform canvas;

        public RectTransform NodeAreaRect;
        public RectTransform NodeAreaInnerRect;

        private int lastIdGiven = 0;
        private void Awake () {
            s = this;
            mycam = Camera.main;
            NodeGfx.snapMultUI = NodeGfx.snapMult *canvas.localScale.x;
        }

        private void Start () {
            // Draw menu
            foreach (ItemSet myItemSet in myRecipeSet.myItemSets) {
                foreach (Item myItem in myItemSet.items) {
                    GameObject itemListItem = Instantiate(ItemListItemPrefab, ItemsParent);
                    itemListItem.GetComponent<DragMe>().myItem = myItem;
                    itemListItem.transform.GetChild(0).GetComponent<Image>().sprite = myItem.mySprite;
                    itemListItem.transform.GetChild(1).GetComponent<Text>().text = myItem.name;
                }
            }
            
            //Draw Nodes
            foreach (var itemNode in myRecipeSet.myItemNodes) {
                //Vector3 pos = itemNode.pos;
                var node = Instantiate(ItemNodePrefab, NodeParent);
                allNodeGfxs.Add(node.GetComponent<NodeGfx>());
                node.GetComponent<ItemNodeGfx>().SetUp(this, itemNode);
                (node.transform as RectTransform).anchoredPosition = itemNode.pos;
                //itemNode.pos = pos;

                lastIdGiven = Mathf.Max(lastIdGiven, itemNode.id);
            }
            
            foreach (var craftingNode in myRecipeSet.myCraftingNodes) {
                //Vector3 pos = craftingNode.pos;
                var node = Instantiate(CraftingNodePrefab, NodeParent);
                allNodeGfxs.Add(node.GetComponent<NodeGfx>());
                node.GetComponent<CraftingNodeGfx>().SetUp(this, craftingNode);
                (node.transform as RectTransform).anchoredPosition = craftingNode.pos;
                //craftingNode.pos = pos;
                
                lastIdGiven = Mathf.Max(lastIdGiven, craftingNode.id);
            }
            
            RescaleNodeArea();
        }


        public void CreateItemNodeAtPosition (PointerEventData data) {
            var originalObj = data.pointerDrag;
            if (originalObj == null)
                return;

            var dragMe = originalObj.GetComponent<DragMe>();
            if (dragMe == null)
                return;

            var myItem = dragMe.myItem;
            if (myItem == null)
                return;
            GameObject node;
            if (myItem.uniqueName == "CraftingProcess") {
                node = Instantiate(CraftingNodePrefab, NodeParent);
                var craftingNode = new CraftingNode(lastIdGiven++);
                myRecipeSet.myCraftingNodes.Add(craftingNode);
                node.GetComponent<CraftingNodeGfx>().SetUp(this, craftingNode);
                node.GetComponent<CraftingNodeGfx>().PositionUpdated();
                
            } else {
                node = Instantiate(ItemNodePrefab, NodeParent);
                var itemNode = new ItemNode(lastIdGiven++, myItem);
                myRecipeSet.myItemNodes.Add(itemNode);
                node.GetComponent<ItemNodeGfx>().SetUp(this, itemNode);
                node.GetComponent<ItemNodeGfx>().PositionUpdated();
            }
            
            allNodeGfxs.Add(node.GetComponent<NodeGfx>());
            //node.transform.position = mycam.ScreenToWorldPoint(data.position);
            node.transform.position = data.position;
            node.transform.position = new Vector3(node.transform.position.x,node.transform.position.y,0);
            node.transform.localScale = Vector3.one;
        }

        private NodePortGfx prevPort;
        private NodeGfx prevNode;
        public void BeginClickConnect(NodeGfx node, NodePortGfx port) {
            
            if (prevNode == null) {
                prevPort = port;
                prevNode = node;
            } else {
                bool canConnect = false;

                switch (port.myType) {
                    case NodePortGfx.PortType.craftInput:
                        if (prevPort.myType == NodePortGfx.PortType.itemOutput)
                            canConnect = true;
                        break;
                    case NodePortGfx.PortType.craftOutput:
                        if (prevPort.myType == NodePortGfx.PortType.itemInput)
                            canConnect = true;
                        break;
                    case NodePortGfx.PortType.itemInput:
                        if (prevPort.myType == NodePortGfx.PortType.craftOutput)
                            canConnect = true;
                        break;
                    case NodePortGfx.PortType.itemOutput:
                        if (prevPort.myType == NodePortGfx.PortType.craftInput)
                            canConnect = true;
                        break;
                }

                if (canConnect) {
                    ConnectPorts(prevPort, port);
                    CraftingNode craft;
                    ItemNode item;
                    bool isInput;
                    if (prevNode is CraftingNodeGfx) {
                        craft = prevNode.myNode as CraftingNode;
                        item = node.myNode as ItemNode;
                        if (prevPort.myType == NodePortGfx.PortType.craftInput)
                            isInput = true;
                        else
                            isInput = false;

                    } else {
                        craft = node.myNode as CraftingNode;
                        item = prevNode.myNode as ItemNode;
                        if (port.myType == NodePortGfx.PortType.craftInput)
                            isInput = true;
                        else
                            isInput = false;
                    }

                    craft.AddConnection(item,1, isInput);
                    item.AddConnection(craft, isInput);

                    prevNode = null;
                    prevPort = null;
                } else {
                    port.ClickConnectDone();
                    if (prevPort) {
                        prevPort.ClickConnectDone();
                    }
                    prevPort = port;
                    prevNode = node;
                }
            }
        }

        public NodeGfx GetNodeGfxFromNode(Node node) {
            foreach (var nodeGfx in allNodeGfxs) {
                if (nodeGfx.myNode.id == node.id && nodeGfx.myNode is ItemNode) {
                    return nodeGfx;
                }
            }

            print("not node!");
            return null;
        }

        public void ConnectPorts(NodePortGfx first, NodePortGfx second) {
            first.AddConnection(second);
            second.AddConnection(first);
        }


        public void DeleteNode(NodeGfx node) {
            allNodeGfxs.Remove(node);
            if (node.myNode is ItemNode) {
                myRecipeSet.myItemNodes.Remove(node.myNode as ItemNode);
            } else {
                myRecipeSet.myCraftingNodes.Remove(node.myNode as CraftingNode);
            }
        }

        const float changeIncrements = 500;
        public void RescaleNodeArea() {
            float xmin = float.MaxValue, xmax = float.MinValue, ymin = float.MaxValue, ymax = float.MinValue;
            foreach (var nodeGfx in allNodeGfxs) {
                xmin = Mathf.Min(nodeGfx.GetComponent<RectTransform>().anchoredPosition.x, xmin);
                xmax = Mathf.Max(nodeGfx.GetComponent<RectTransform>().anchoredPosition.x, xmax);
                ymin = Mathf.Min(nodeGfx.GetComponent<RectTransform>().anchoredPosition.y, ymin);
                ymax = Mathf.Max(nodeGfx.GetComponent<RectTransform>().anchoredPosition.y, ymax);
            }
            
            /*print("min and max values");
            print(xmin);
            print(xmax);
            print(ymin);
            print(ymax);/

            var rect = NodeAreaInnerRect.rect;
            //var scale = NodeAreaRect.localScale.x;
            var scale = 1;
            
            float leftSide =  -NodeParent.anchoredPosition.x- rect.width / 2;
            float rightSide =  -NodeParent.anchoredPosition.x+ rect.width / 2;
            float topSide = -NodeParent.anchoredPosition.y+ rect.height / 2;
            float bottomSide = -NodeParent.anchoredPosition.y- rect.height / 2;

            /*print("box edges");
            print(leftSide);
            print(rightSide);
            print(topSide);
            print(bottomSide)/
            
            //NodeParent.SetParent(canvas);

            bool madeShift = false;
            Vector2 totalShift = Vector2.zero;
            if (xmin < leftSide) {
                NodeAreaRect.sizeDelta = new Vector2(((NodeAreaRect.sizeDelta.x/ changeIncrements)+1)*changeIncrements, NodeAreaRect.sizeDelta.y);
                NodeAreaInnerRect.sizeDelta = new Vector2(((NodeAreaInnerRect.sizeDelta.x/changeIncrements)+1)*changeIncrements, NodeAreaInnerRect.sizeDelta.y);
                NodeAreaRect.anchoredPosition += new Vector2(-changeIncrements/2,0)*scale;
                totalShift+= new Vector2(-changeIncrements/2,0)*scale;
                madeShift = true;
                print("enlarging to the leftSide");

            } else if(xmin >leftSide + changeIncrements){
                if (NodeAreaRect.sizeDelta.x > 2500) {
                    NodeAreaRect.sizeDelta = new Vector2(((NodeAreaRect.sizeDelta.x / changeIncrements) - 1) * changeIncrements, NodeAreaRect.sizeDelta.y);
                    NodeAreaInnerRect.sizeDelta = new Vector2(((NodeAreaInnerRect.sizeDelta.x / changeIncrements) - 1) * changeIncrements, NodeAreaInnerRect.sizeDelta.y);
                    NodeAreaRect.anchoredPosition -= new Vector2(-changeIncrements / 2, 0)*scale;
                    totalShift-= new Vector2(-changeIncrements / 2, 0)*scale;
                    madeShift = true;
                    print("reducing to the leftSide");
                }
            }
            
             if (xmax > rightSide) {
                NodeAreaRect.sizeDelta = new Vector2(((NodeAreaRect.sizeDelta.x/changeIncrements)+1)*changeIncrements, NodeAreaRect.sizeDelta.y);
                NodeAreaInnerRect.sizeDelta = new Vector2(((NodeAreaInnerRect.sizeDelta.x/changeIncrements)+1)*changeIncrements, NodeAreaInnerRect.sizeDelta.y);
                NodeAreaRect.anchoredPosition += new Vector2(+changeIncrements/2,0)*scale;
                totalShift+= new Vector2(+changeIncrements/2,0)*scale;
                madeShift = true;
                print("enlarging to the rightSide");
                    
            }else if(xmax <rightSide - changeIncrements){
                if (NodeAreaRect.sizeDelta.x > 2500) {
                    NodeAreaRect.sizeDelta = new Vector2(((NodeAreaRect.sizeDelta.x / changeIncrements) - 1) * changeIncrements, NodeAreaRect.sizeDelta.y);
                    NodeAreaInnerRect.sizeDelta = new Vector2(((NodeAreaInnerRect.sizeDelta.x / changeIncrements) - 1) * changeIncrements, NodeAreaInnerRect.sizeDelta.y);
                    NodeAreaRect.anchoredPosition -= new Vector2(+changeIncrements / 2, 0)*scale;
                    totalShift-= new Vector2(+changeIncrements / 2, 0)*scale;
                    madeShift = true;
                    print("reducing to the rightSide");
                }
            }
            
             if (ymin < bottomSide) {
                NodeAreaRect.sizeDelta = new Vector2(NodeAreaRect.sizeDelta.x,((NodeAreaRect.sizeDelta.y/ changeIncrements)+1)*changeIncrements);
                NodeAreaInnerRect.sizeDelta = new Vector2(NodeAreaInnerRect.sizeDelta.x,((NodeAreaInnerRect.sizeDelta.y/ changeIncrements)+1)*changeIncrements);
                NodeAreaRect.anchoredPosition += new Vector2(0,-changeIncrements/2)*scale;
                totalShift+= new Vector2(0,-changeIncrements/2)*scale;
                madeShift = true;
                print("enlarging to the bottomSide");
                
            } else if(ymin >bottomSide + changeIncrements){
                if (NodeAreaRect.sizeDelta.y > 2500) {
                    NodeAreaRect.sizeDelta = new Vector2(NodeAreaRect.sizeDelta.x, ((NodeAreaRect.sizeDelta.y / changeIncrements) - 1) * changeIncrements);
                    NodeAreaInnerRect.sizeDelta = new Vector2(NodeAreaInnerRect.sizeDelta.x, ((NodeAreaInnerRect.sizeDelta.y / changeIncrements) - 1) * changeIncrements);
                    NodeAreaRect.anchoredPosition -= new Vector2(0, -changeIncrements / 2)*scale;
                    totalShift-= new Vector2(0, -changeIncrements / 2)*scale;
                    madeShift = true;
                    print("reducing to the bottomSide");
                }
            }
            
             if (ymax > topSide) {
                NodeAreaRect.sizeDelta = new Vector2(NodeAreaRect.sizeDelta.x,((NodeAreaRect.sizeDelta.y/ changeIncrements)+1)*changeIncrements);
                NodeAreaInnerRect.sizeDelta = new Vector2(NodeAreaInnerRect.sizeDelta.x,((NodeAreaInnerRect.sizeDelta.y/ changeIncrements)+1)*changeIncrements);
                NodeAreaRect.anchoredPosition += new Vector2(0,+changeIncrements/2)*scale;
                totalShift+= new Vector2(0,+changeIncrements/2)*scale;
                madeShift = true;
                print("enlarging to the topSide");
                
            } else if(ymax < topSide - changeIncrements){
                if (NodeAreaRect.sizeDelta.y > 2500) {
                    NodeAreaRect.sizeDelta = new Vector2(NodeAreaRect.sizeDelta.x, ((NodeAreaRect.sizeDelta.y / changeIncrements) - 1) * changeIncrements);
                    NodeAreaInnerRect.sizeDelta = new Vector2(NodeAreaInnerRect.sizeDelta.x, ((NodeAreaInnerRect.sizeDelta.y / changeIncrements) - 1) * changeIncrements);
                    NodeAreaRect.anchoredPosition -= new Vector2(0, +changeIncrements / 2)*scale;
                    totalShift-= new Vector2(0, +changeIncrements / 2)*scale;
                    madeShift = true;
                    print("reducing to the topSide");
                }
            }

            NodeParent.anchoredPosition -= totalShift;
            //NodeParent.SetParent(NodeAreaRect);
            if (madeShift) {
                RescaleNodeArea();
            }

            //NodeParent.anchoredPosition = Vector3.zero;
        }*/
        
    }

//}