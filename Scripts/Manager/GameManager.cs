using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGameOverEvent;
    public event Action OnGameStartEvent;
    public event Action OnStartAnimationEvent;
    public bool IsGameOver { get; private set; }
    public bool IsCatch { get;  set; }

    public DataInitSystem gameData { get; set; }
    public void GameOver(bool isClear)
    {
        IsGameOver = true;
        
        GameOverTask(isClear).Forget();
    }

    private async UniTaskVoid GameOverTask(bool isClear)
    {
        OnGameOverEvent?.Invoke();

        if (isClear) {
            await UniTask.WaitUntil(() => IsCatch);
        }
        GameResultUI resultUI = UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.GameResultCanvas).GetComponent<GameResultUI>();
        resultUI.OnGameResultUI(isClear);
    }

    public void GameReady()
    {
        OnStartAnimationEvent?.Invoke();
    }

    public void GameStart()
    {
        IsGameOver = false;
        IsCatch = false;
        OnGameStartEvent?.Invoke();
    }
}
