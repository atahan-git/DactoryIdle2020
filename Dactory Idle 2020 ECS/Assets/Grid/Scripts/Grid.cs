﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;



/// <summary>
/// There can only be one grid at a time
/// </summary>
public class Grid : MonoBehaviour {
	public static Grid s;
	string saveName = "TestGrid";

	public bool canEditTiles = false;
	public bool canDrawGizmos = false;


	public float gridScaleX = 1f;
	public float gridScaleY = 1f;

	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public Color color = Color.white;

	public TileSet tileSet;

	[HideInInspector]
	GameObject[,] myTilesGameObjects = new GameObject[10,10];
	[SerializeField]
	TileBaseScript[,] myTiles = new TileBaseScript[10, 10];
	public GameObject emptyTile;
	Tiles myTilesIDs = new Tiles();

	public int myType = 0;

	private void Awake () {
		if (s != null) {
			Debug.LogError(string.Format("More than one singleton copy of {0} is detected! this shouldn't happen.", this.ToString()));
		}
		s = this;

		if (!Load ()) {
			myTilesGameObjects = new GameObject[gridSizeX, gridSizeY];
			myTiles = new TileBaseScript[gridSizeX, gridSizeY];
			myTilesIDs.tiles = new int[gridSizeX, gridSizeY];
		}

		DrawTiles ();
	}

	public TileBaseScript GetTile (Position pos) {
		return myTiles[pos.x, pos.y];
	}

	public Vector2[] rectangleSelect = new Vector2[2];

	void Update(){
		if (!canEditTiles)
			return;

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			myType = 0;
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			myType = 1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			myType = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			myType = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			myType = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			myType = 5;
		}

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f)){
				TileBaseScript tileS;
				try{
					tileS = hit.collider.gameObject.GetComponent<TileBaseScript>();
				}catch{return;}
					
				rectangleSelect [0] = new Vector2 (tileS.x, tileS.y);

			}
		}

		if (Input.GetMouseButtonUp (1)) {
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 10000f)) {
				TileBaseScript tileS;
				try {
					tileS = hit.collider.gameObject.GetComponent<TileBaseScript> ();
				} catch {
					return;
				}


				rectangleSelect [1] = new Vector2 (tileS.x, tileS.y);
				RectangleSelect (rectangleSelect);
				rectangleSelect = new Vector2[2];

			} else {
				rectangleSelect = new Vector2[2];
			}
		}
	}

	void RectangleSelect(Vector2[] selection){
		for (int x = (int)selection [0].x; x <= selection [1].x; x++) {
			for (int y = (int)selection [1].y; y <= selection [0].y; y++) {
				
				ClickTile (myTilesGameObjects [x, y]);

			}
		}
	}

	public void UpdateTileSize(){
		//s = this;
		//DeleteAllTiles ();
		myTilesGameObjects = new GameObject[gridSizeX, gridSizeY];
		myTilesIDs.tiles = new int[gridSizeX, gridSizeY];
		Save ();
		//DrawTiles ();
	}

	public void DrawTiles(){
		DeleteAllTiles ();

		for (int x = 0; x < myTilesGameObjects.GetLength (0); x++) {
			for (int y = 0; y < myTilesGameObjects.GetLength (1); y++) {

				GameObject myTile = (GameObject)Instantiate (tileSet.prefabs[myTilesIDs.tiles [x, y]], transform.position, transform.rotation);
				myTile.gameObject.name = x.ToString() + ", " + y.ToString() + " - "+ myTile.gameObject.name;
				myTilesGameObjects [x, y] = myTile;

				TileBaseScript myTileScript = myTile.GetComponent<TileBaseScript> ();
				myTiles[x, y] = myTileScript;

				myTileScript.x = x;
				myTileScript.y = y;
				//myTileScript.mySet = tileSet;
				//myTileScript.tileType = myTilesIDs.tiles [x, y];
				myTileScript.UpdateLocation ();

				myTileScript.transform.parent = transform;
			}
		}
	}

	void DeleteAllTiles(){
		foreach(GameObject gam in myTilesGameObjects){
			Destroy (gam);
		}

		GameObject[] myChildren = new GameObject[transform.childCount];
		int n = 0;
		foreach (Transform child in transform) {
			myChildren [n] = child.gameObject;
			n++;
		}

		foreach(GameObject gam in myChildren){
			DestroyImmediate (gam);
		}
	}

	void OnDrawGizmos(){
		if (!canDrawGizmos)
			return;
		//Vector3 pos = Camera.current.transform.position;
		Gizmos.color = this.color;

		for (int i = 0; i <= gridSizeX; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (i * gridScaleX, 0, 0), 
				transform.position + new Vector3 (i * gridScaleX, 0, 0) + new Vector3 (0, gridSizeY * gridScaleY, 0));
		}
		for (int i = 0; i <= gridSizeY; i++) {
			Gizmos.DrawLine (transform.position + new Vector3 (0, i * gridScaleY, 0), 
				transform.position + new Vector3 (0, i * gridScaleY, 0) + new Vector3 (gridSizeX * gridScaleX, 0, 0));
		}

		//origin
		Gizmos.color = Color.red;
				Gizmos.DrawSphere (transform.position, gridScaleX / 5);
	}

	public void ClickTile(GameObject tile){
		if (!canEditTiles)
			return;
		
		s = this;
		//print (tile);

		TileBaseScript myTileScript = tile.GetComponent<TileBaseScript> ();

		int x = myTileScript.x;
		int y = myTileScript.y;

		Destroy (myTilesGameObjects [x, y].gameObject);

		GameObject tileToSpawn;

		tileToSpawn = tileSet.prefabs [myType];
		myTilesIDs.tiles [x, y] = myType;

		GameObject myTile = (GameObject)Instantiate (tileToSpawn, transform.position, transform.rotation);
		myTilesGameObjects [x, y] = myTile;

		myTileScript = myTile.GetComponent<TileBaseScript> ();
		myTiles[x, y] = myTileScript;

		myTileScript.x = x;
		myTileScript.y = y;
		myTileScript.UpdateLocation ();

		myTileScript.transform.parent = transform;

		//Save ();
	}

	void OnApplicationQuit(){
		Save ();
	}

	public void Save (){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + saveName + ".banana");

		Tiles data = myTilesIDs;

		bf.Serialize (file, data);
		file.Close ();
		print ("Grid Data Saved");
	}

	public bool Load(){
		try {
			if (File.Exists (Application.persistentDataPath + "/" + saveName + ".banana")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + saveName + ".banana", FileMode.Open);
				Tiles data = (Tiles)bf.Deserialize (file);
				file.Close ();

				myTilesIDs = data;
				gridSizeX = myTilesIDs.tiles.GetLength (0);
				gridSizeY = myTilesIDs.tiles.GetLength (1);
				myTilesGameObjects = new GameObject[gridSizeX, gridSizeY];
				myTiles = new TileBaseScript[gridSizeX, gridSizeY];
				print ("Data Loaded");
				return true;
			} else {
				print ("No Data Found");
				return false;
			}
		} catch {
			File.Delete (Application.persistentDataPath + "/" + saveName + ".banana");
			print ("Corrupt Data Deleted");
			return false;
		}
	}
}


[Serializable]
public class Tiles {
	public int[,] tiles;
}