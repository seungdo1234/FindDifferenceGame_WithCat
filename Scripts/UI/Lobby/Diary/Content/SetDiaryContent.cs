
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetDiaryContent : MonoBehaviour
{
    [Header("# Diary Title Component Info")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI weatherText;

    [Header("# Diary Component Info")]
    [SerializeField] private TextMeshProUGUI[] diaryContentTexts;
    [SerializeField] private DiaryUIEventHandler diaryEvent;
    private void Awake()
    {
        diaryEvent.OnUpdateDiaryEvent += UpdateDiaryContent;
    }

    private void UpdateDiaryContent(DiaryInfo info)
    {
        try {
            titleText.text = info.isLock ? "" : info.diary.diaryName;
            weatherText.text = info.isLock ? "" : info.diary.weatherDesc;

            ResetContentText();
            for (int i = 0; i < info.diary.diaryDesc.Length; i++) {
                diaryContentTexts[i].text = info.isLock ? "" : info.diary.diaryDesc[i];
            }
        }
        catch (Exception e) {
            Debug.LogError($"UpdateDiaryContent => {e} ");
            throw;
        }
    }

    private void ResetContentText()
    {
        foreach (TextMeshProUGUI t in diaryContentTexts) {
            t.text = "";
        }
    }

}
