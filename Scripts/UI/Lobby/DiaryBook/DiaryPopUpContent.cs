using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryPopUpContent : BaseButtonUI
{
    [Header("# Diary Lock Info")]
    [SerializeField] private DiaryUnLockBtn diaryUnLockBtn;

    [SerializeField ]private Image image;
    private int scrollNum;
    private Action<int> navigateCallback;

    public DiaryInfo d;
    protected void Awake()
    {
    }

    public void DiaryContentInit(Action<int> navigateCallback)
    {
        base.Awake();
        this.navigateCallback = navigateCallback;
    }
    public void SetDiaryContent(DiaryInfo info, int scrollNum)
    {
        d = info;
        this.scrollNum = scrollNum;
        image.sprite = info.diary.diaryArt;
        diaryUnLockBtn.InitializeButton(info);
        
        RemoveOnClickEvent();
        RegisterOnClickEvent(NavigateDiaryContent);
    }

    private void NavigateDiaryContent()
    {
        ButtonClickAnimation(NavigateScroll);
    }

    private void NavigateScroll()
    {
        navigateCallback?.Invoke(scrollNum);
    }
}
