using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class DiaryPopUpUI : BasePopUpUI
{
    [Header("# Diary Book Info")]
    [SerializeField] private DiaryInfo selectDiarySO;
    [SerializeField] private DiaryInfo[] weatherDiaries;
    [SerializeField] private E_SeasonType selectSeasonType;
    [SerializeField] private DiaryUIEventHandler diary;
    
    [Header("# Diary Book Contents")]
    [SerializeField] private DiaryContentUIAnimation diaryContents;
    [SerializeField] private List<DiaryPopUpContent> diaryContentList;
    [SerializeField] private DiaryPopUpContent contentPrefab;

    public event Action OpenPopUpEvent;
    protected override void Awake()
    {
        base.Awake();
    }
    
    public override void OpenPopUp()
    {
        OpenPopUpEvent?.Invoke();
        base.OpenPopUp();
    }
    
    public void SelectSeasonTab(E_SeasonType seasonType)
    {
        selectSeasonType = seasonType;
        diaryContents.ChangeSeasonDiaryContents();
        UpdateDiaryBook();
    }
    
    private void UpdateDiaryBook()
    {
        weatherDiaries = GameDataManager.Instance.GetWeatherDiarySO(selectSeasonType);
        if (weatherDiaries == null) {
            Debug.LogError($"{selectSeasonType} => WeatherDiaries is Empty");
            return;
        }

        SetDiaryContents();
    }
    
    private void SetDiaryContents()
    {
        try 
        {
            int count = weatherDiaries.Length;
            int existingCount = diaryContentList.Count;

            for (int i = existingCount; i < count; i++)
            {
                DiaryPopUpContent content = Instantiate(contentPrefab, diaryContents.transform);
                content.DiaryContentInit(NavigateDiary);
                diaryContentList.Add(content);
            }

            for (int i = 0; i < diaryContentList.Count; i++) {
                if (i < count)
                {
                    diaryContentList[i].gameObject.SetActive(true);
                    diaryContentList[i].SetDiaryContent(weatherDiaries[i], i);
                }
                else
                {
                    diaryContentList[i].gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SetDiaryContents에서 오류 발생: {e.Message}");
            throw;
        }
    }

    private void NavigateDiary(int scrollNum)
    {
        ClosePopUp();
        diary.NavigateScroll(selectSeasonType, scrollNum);
    }
}
