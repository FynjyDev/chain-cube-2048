using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public BlocksDataManager BlocksDataManager;
    public BlockShootingController BlockShootingController;
    public BlockSpawnController BlockSpawnController;
    public ProgressController ProgressController;
    public SaveLoadController SaveLoadController;
    public UIController UIController;
    public DeadLine DeadLine;

    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action OnGameQuit;

    public void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        SaveDataStructure saveData = SaveLoadController.Load();

        DeadLine.Init(this);
        BlockShootingController.Init(BlockSpawnController, DeadLine);
        ProgressController.Init(saveData == null ? 0 : saveData.CurrentScores, UIController);
        BlockSpawnController.Init(saveData == null ? new List<BlockSaveData>() : saveData.BlockSaveData, BlockShootingController, ProgressController, BlocksDataManager);
    }

    public void GameLose()
    {
        OnGameEnd?.Invoke();
    }

    public void OnApplicationQuit()
    {
        OnGameQuit?.Invoke();
    }

    public void ReloadGame()
    {
        OnGameQuit?.Invoke();
        SceneManager.LoadScene(0);
    }
}
