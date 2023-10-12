using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public BlockShootingController BlockShootingController;
    public BlockSpawnController BlockSpawnController;
    public ProgressController ProgressController;
    public SaveLoadController SaveLoadController;

    public static event Action OnGameStart;
    public static event Action OnDataLoad;
    public static event Action OnGameQuit;

    public void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        SaveDataStructure saveData = SaveLoadController.Load();
       
        ProgressController.Init(saveData == null ? 0 : saveData.CurrentScores);
        BlockSpawnController.Init(saveData == null ? new List<BlockSaveData>() : saveData.BlockSaveData);
    }

    public void OnApplicationQuit()
    {
        OnGameQuit?.Invoke();
    }
}
