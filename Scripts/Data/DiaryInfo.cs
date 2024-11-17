using System;
using UnityEngine;
[System.Serializable]
public class DiaryInfo : DiarySaveInfo
{
    public DiarySO diary;
    public int requiredCurrency = 20;
    
    public void UnLockDiary()
    {
        isLock = false;
        PlayerDataManager.Instance.ChangeCurrency(-requiredCurrency);
        DiaryManager.Instance.CallUpdateDiaryLockEvent(diary.seasonType);
    }
}

public class DiarySaveInfo
{
    public bool isLock;
    public E_DifficultyType unLockMaxDifficulty;
}