using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class DiaryUIEventHandler : MonoBehaviour
{
    [Header("# Diary Info")]
    [SerializeField] private DiaryInfo selectDiaryInfo;
    [SerializeField] private DiaryInfo[] weatherDiaries;
    [SerializeField] private E_SeasonType selectSeasonType;

    [Header("# Diary Difficulty Info")]
    [SerializeField] private E_DifficultyType selectDifficulty;
    
    [Header("# Diary Contents Component")]
    [SerializeField] private SimpleScrollSnap scroll;
    [SerializeField] private Transform diaryContents;
    [SerializeField] private DiaryArt contentPrefab;

    [Header("# Active Diary Content")]
    [SerializeField] private List<DiaryArt> diaryArts;
    
    public event Action<DiaryInfo> OnUpdateDiaryEvent;
    public event Action<int> OnChangeSeasonBtnEvent;

    
    private bool isDiaryPopUpEvent;
    private int sel;
    private void Start()
    {
        DiaryManager.Instance.OnUpdateDiaryLockEvent += UnLockDiary;
    }
    private void UpdateSeasonDiaryScroll()
    {
        weatherDiaries = GameDataManager.Instance.GetWeatherDiarySO(selectSeasonType);
        if (weatherDiaries == null) {
            Debug.LogError($"{selectSeasonType} => WeatherDiaries is Empty");
            return;
        }

        SetDiaryContents();
        ScrollDiary();
        SetDiaryLock();
    }

    private void SetDiaryContents()
    {

        int count = weatherDiaries.Length;
        for (int i = 0; i < diaryArts.Count; i++) {
            diaryArts[i].gameObject.SetActive(count-- > 0);
        }
        for (int i = 0; i < count; i++) {
            DiaryArt art = Instantiate(contentPrefab, diaryContents);
            diaryArts.Add(art);
        }

        scroll.StartSliderInit(scroll.CenteredPanel);   
    }

    private void UnLockDiary(E_SeasonType seasonType)
    {
        if (seasonType == selectSeasonType) {
            SetDiaryLock();
            
            OnUpdateDiaryEvent?.Invoke(selectDiaryInfo);
        }
    }

    public void SelectSeasonTab(E_SeasonType seasonType)
    {
        selectSeasonType = seasonType;

        UpdateSeasonDiaryScroll();
        if (!isDiaryPopUpEvent && scroll.CenteredPanel != 0) {
            scroll.GoToPanel(0);
        }
    }

    private void SetDiaryLock()
    {
        for (int i = 0; i < weatherDiaries.Length; i++) {
            diaryArts[i].DiaryArtInit(weatherDiaries[i]);
        }
    }

    public void ScrollDiary()
    {
        selectDiaryInfo = weatherDiaries[scroll.CenteredPanel];
        OnUpdateDiaryEvent?.Invoke(selectDiaryInfo);
    }

    public void NavigateScroll(E_SeasonType type, int scrollNum)
    {
        isDiaryPopUpEvent = true;
        OnChangeSeasonBtnEvent?.Invoke((int)type);
        ScrollMove(scrollNum, DiaryPopUpEventOver);
    }
    
    public void ScrollMove(int scrollNum, Action callback = null)
    {
        scroll.GoToPanel(scrollNum, callback);
    }

    private void DiaryPopUpEventOver()
    {
        isDiaryPopUpEvent = false;
    }

    public void SelectDifficulty(E_DifficultyType difficultyType)
    {
        selectDifficulty = difficultyType;
    }

    public void StartDiaryGame()
    {
        DiaryManager.Instance.SetDiaryInfo(selectDiaryInfo, selectDifficulty);
    }
}
