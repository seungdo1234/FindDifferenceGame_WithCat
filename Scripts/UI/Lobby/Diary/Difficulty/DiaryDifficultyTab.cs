using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DifficultyType { Default = -1, Easy, Normal, Hard, VeryHard }
public enum E_DifficultyButtonState { Default = -1, Active, DeActive, Lock }
public class DiaryDifficultyTab : BaseToggleController
{
    [Header("# Diary Difficulty Tab Info")]
    [SerializeField] private Sprite btnActiveStateSprite;
    [SerializeField] private Sprite btnLockStateSprite;

    [Header("# Difficulty Button Info")]
    [SerializeField] private DifficultyButton[] difficultyButtons;
    [SerializeField] private bool isBool;

    [Header("# Diary UI Info")]
    [SerializeField] private DiaryUIEventHandler diaryUI;

    private DiaryInfo selDiaryInfo;
    private void Awake()
    {
        DifficultyToggle(difficultyButtons[(int)E_DifficultyType.Easy]);
    }

    private void Start()
    {
        diaryUI.OnUpdateDiaryEvent += UpdateButtonState;
    }
    
    private void UpdateButtonState(DiaryInfo diaryInfo)
    {
        selDiaryInfo = diaryInfo;
        bool isArtUnLock = !diaryInfo.isLock;
        for (int i = 0; i < difficultyButtons.Length; i++) {
            bool isUnLock = isArtUnLock && i <= (int)diaryInfo.unLockMaxDifficulty;
            Sprite btnImage = isArtUnLock && isUnLock ? btnActiveStateSprite : btnLockStateSprite;
            difficultyButtons[i].DifficultyBtnInit((E_DifficultyType)i, isUnLock, btnImage);
        }

        DifficultyToggle(isArtUnLock ? difficultyButtons[(int)diaryInfo.unLockMaxDifficulty] : null);
    }

    public void DifficultyToggle(BaseToggleButtonUI changeToggle)
    {
        ChangeToggleEvent(changeToggle, ChangeDifficulty);
    }

    private void ChangeDifficulty()
    {
        DifficultyButton difficultyButton = selectToggle as DifficultyButton;
        if (difficultyButton != null) diaryUI.SelectDifficulty(difficultyButton.DifficultyType);
    }
}
