﻿using UnityEngine;
using System.Collections;

public class TileBaseScript : MonoBehaviour {

	public int tileType = -1;
	public int x = 0;
	public int y = 0;
	public Position position { get { return new Position(x, y); } }

	static bool isDragStarted = false;


	public bool isEmpty { get { return !areThereBelt && !areThereBuilding; } }
	public bool areThereBuilding { get { return myBuilding != null; } }
	public GameObject myBuilding;
	public bool areThereBelt { get { return myBelt != null; } }
	public GameObject myBelt;
	public bool itemPlaceable = false;
	public bool beltPlaceable = false;


	SpriteRenderer rend;
	// Update is called once per frame
	void Update () {
		if (isDragStarted && Input.GetMouseButtonUp (0)) {
			//print ("drag end");
			isDragStarted = false;
		}

		if (MasterDebug.changeColorOnPlacedItemOnTop) {
			if (rend == null)
				rend = GetComponentInChildren<SpriteRenderer>();
			else {
				rend.color = !isEmpty ? Color.green : Color.red;
			}
		}
	}

	void OnMouseDown(){
		//print ("drag start");
		isDragStarted = true;
		Grid.s.ClickTile (gameObject);
	}

	void OnMouseOver(){
		if (isDragStarted) {
			Grid.s.ClickTile (gameObject);
		}
	}

	void OnMouseUp(){
		//print ("drag end");
		isDragStarted = false;
	}

	/*public void SetTileType(){
		//GetComponentInChildren<SpriteRenderer>().sprite = mySet.
	}*/

	public void UpdateLocation(){
		if (Grid.s != null) {
			transform.position = Grid.s.transform.position +
			new Vector3 (x * Grid.s.gridScaleX + Grid.s.gridScaleX / 2,
				y * Grid.s.gridScaleY + Grid.s.gridScaleY / 2, 0);
		} else {

			Grid grid = GameObject.FindObjectOfType<Grid> ();

			transform.position = grid.transform.position +
				new Vector3 (x * grid.gridScaleX + grid.gridScaleX / 2,
					y * grid.gridScaleY + grid.gridScaleY / 2, 0);

		}
	}
}
