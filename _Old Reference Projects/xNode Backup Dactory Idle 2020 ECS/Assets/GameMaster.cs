﻿using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster s;
	public static bool loadingDone = false;

	private void Awake () {
		if (s != null) {
			Debug.LogError(string.Format("More than one singleton copy of {0} is detected! this shouldn't happen.", this.ToString()));
		}
		s = this;
		GameLoader.isGameLoadingDone = false;
	}

	// Use this for initialization
	void Start () {
		GetComponent<GameLoader>().LoadGame();
		loadingDone = true;
		BeltMaster.s.StartBeltSystem();
		BuildingMaster.s.StartBuildingSystem();

		/*if (GameLoader.isGameLoadingSuccessfull == false) {
			
		}*/
	}


	public static void StartSavingGameProcess() {
		if (loadingDone)
			DataSaver.s.SaveGame();
	}

	private void OnApplicationPause () {
		StartSavingGameProcess();
	}


	private void OnApplicationQuit () {
		StartSavingGameProcess();
	}

}
