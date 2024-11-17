using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    [field:Header("# Diary Info")]
    [field:SerializeField] public DiaryInfo[] SpringDiaries { get; private set; }
    [field:SerializeField] public DiaryInfo[] SummerDiaries { get; private set; }
    [field:SerializeField] public DiaryInfo[] FallDiaries { get; private set; }
    [field:SerializeField] public DiaryInfo[] WinterDiaries { get; private set; }

    [field:Header("# Button State Info")]
    [field:SerializeField] public DiaryUnLockInfo[] ButtonStateInfo { get; private set; }

    [field:Header("# Currency Sprite Info")]
    [field:SerializeField] public Sprite[] CurrencySpriteDatas { get; private set; }
    public DiaryInfo[] GetWeatherDiarySO(E_SeasonType type)
    {
        return type switch
        {
            E_SeasonType.Spring => SpringDiaries,
            E_SeasonType.Summer => SummerDiaries,
            E_SeasonType.Fall => FallDiaries,
            E_SeasonType.Winter => WinterDiaries,
            _ => null
        };
    }

    protected override void Awake()
    {
        base.Awake();
        DiaryDataInit();
    }

    private void DiaryDataInit()
    {
        SpringDiaries = LoadDiariesForSeason("Spring");
        SummerDiaries = LoadDiariesForSeason("Summer");
        FallDiaries = LoadDiariesForSeason("Fall");
        WinterDiaries = LoadDiariesForSeason("Winter");
    }

    private DiaryInfo[] LoadDiariesForSeason(string season)
    {
        DiarySO[] diaries = Resources.LoadAll<DiarySO>($"Diary/{season}");
        DiaryInfo[] diaryInfos = new DiaryInfo[diaries.Length];

        for (int i = 0; i < diaries.Length; i++)
        {
            diaryInfos[i] = new DiaryInfo
            {
                diary = diaries[i],
                isLock = i != 0,
                requiredCurrency = 20 
            };
        }

        return diaryInfos;
    }
}
