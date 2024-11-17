using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

public class DataInitSystem : MonoBehaviour
{
    [Header("# Lobby Init")]
    [SerializeField] private SeasonButtonEventHandler seasonButtonEvent;
    [SerializeField] private DiaryPopUpButton diaryPopUpButton;
    [SerializeField] private DiaryPopUpSeasonTab diaryPopUpSeasonTab;

    [Header("# InGame Init")]
    [SerializeField] private TimerEventHandler timerEventHandler;
    [SerializeField] private CatchZoneUIAnimationHandler catchZoneUIAnimation;

    private void Start()
    {
        GameManager.Instance.gameData = this;

        //LobbyDataInit().Forget();
        // InGameDataInit().Forget();
    }
    
    public async UniTaskVoid GameDataInit()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);
        diaryPopUpButton.DiaryButtonInit();
        diaryPopUpSeasonTab.DiaryPopUpSeasonTabInit();
        catchZoneUIAnimation.CatchZoneAnimationInit();
        timerEventHandler.RegisterTimerEvent();
    }

    public void DiaryContentInit()
    {
        seasonButtonEvent.SeasonDiaryInit();
    }
    
    // public async UniTaskVoid LobbyDataInit()
    // {
    //     await UniTask.Yield(PlayerLoopTiming.Update);
    //     diaryPopUpButton.DiaryButtonInit();
    //     diaryPopUpSeasonTab.DiaryPopUpSeasonTabInit();
    // }
    // public async UniTaskVoid InGameDataInit()
    // {
    //     await UniTask.Yield(PlayerLoopTiming.Update);
    //     catchZoneUIAnimation.CatchZoneAnimationInit();
    //     timerEventHandler.TimerInit();
    // }
}
