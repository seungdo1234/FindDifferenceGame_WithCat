using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiaryManager : Singleton<DiaryManager>
{
    [Header("# Difference Spot Count Data")]
    [SerializeField] private int[] differenceSpotCount;
    [SerializeField] private int[] rewardCount;

    public DiaryInfo SelectDiaryInfo { get; private set; }
    public E_DifficultyType SelectDifficultyType { get; private set; }


    public int GetDifferenceSpotCount()
    {
        try {
            return differenceSpotCount[(int)SelectDifficultyType];
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return 0;
        }
    }

    public int GetRewardCount()
    {
        try {
            return rewardCount[(int)SelectDifficultyType];
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return 0;
        }
    }

    public void SetDiaryInfo(DiaryInfo info, E_DifficultyType difficultyType)
    {
        SelectDiaryInfo = info;
        SelectDifficultyType = difficultyType;
    }

    public void ClearDiary()
    {
        if (SelectDifficultyType != E_DifficultyType.VeryHard && SelectDiaryInfo.unLockMaxDifficulty == SelectDifficultyType) {
            SelectDiaryInfo.unLockMaxDifficulty = ++SelectDifficultyType;
        }
    }

    public event Action<E_SeasonType> OnUpdateDiaryLockEvent;

    public void CallUpdateDiaryLockEvent(E_SeasonType seasonType)
    {
        OnUpdateDiaryLockEvent?.Invoke(seasonType);
    }
}
