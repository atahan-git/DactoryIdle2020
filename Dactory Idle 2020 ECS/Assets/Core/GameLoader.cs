﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Deals with starting up the game. Runs all the starting functionality, in the wanted order.
/// </summary>
public class GameLoader : MonoBehaviour {

    public static bool isGameLoadingDone = false;
    public static bool isGameLoadingSuccessfull = false;

    public delegate void LoadYourself();

    static event LoadYourself loadCompleteEventEarly;
    static event LoadYourself loadCompleteEvent;

    public void LoadGame() {
        if (DataSaver.s.Load()) {
            isGameLoadingSuccessfull = true;
        } else {
            isGameLoadingSuccessfull = false;
        }

        loadCompleteEventEarly?.Invoke();
        loadCompleteEvent?.Invoke();
            
        isGameLoadingDone = true;
    }
    
    /// <summary>
    /// This must be called from "Awake"
    /// Remember to add the "OnDestroy" pair > RemoveFromCall
    /// </summary>
    public static void CallWhenLoaded(LoadYourself callback, int order) {
        loadCompleteEventEarly += callback;
    }

    /// <summary>
    /// This must be called from "Awake"
    /// Remember to add the "OnDestroy" pair > RemoveFromCall
    /// </summary>
    public static void CallWhenLoaded(LoadYourself callback) {
        loadCompleteEvent += callback;
    }

    /// <summary>
    /// This should always be called "OnDestroy" to make things work if you ever delete an object and/or reload a scene
    /// </summary>
    /// <param name="callback"></param>
    public static void RemoveFromCall(LoadYourself callback) {
        loadCompleteEventEarly -= callback;
        loadCompleteEvent -= callback;
    }
}
