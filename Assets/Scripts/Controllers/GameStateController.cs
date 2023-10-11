using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static event Action OnGameStart;

    public void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }
}
