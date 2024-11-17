using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiaryPopUpSeasonTab : BaseToggleController
{
    private DiaryPopUpUI diaryPopUpUI;
    public void DiaryPopUpSeasonTabInit()
    {
        try {
            diaryPopUpUI = UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.DiaryBookCanvas).GetComponent<DiaryPopUpUI>();
            
            diaryPopUpUI.OpenPopUpEvent += () => ChangeToggleEvent(transform.GetChild(0).GetComponent<DiaryPopUpSeasonButton>(),UpdateSeason);
        }
        catch (Exception e) {
            Debug.LogError(e);
            throw;
        }
    }

    public void ChangeSeasonToggle(BaseToggleButtonUI changeToggle)
    {
        ChangeToggleEvent(changeToggle, UpdateSeason);
    }
    private void UpdateSeason()
    {
        DiaryPopUpSeasonButton seasonBtn = selectToggle as DiaryPopUpSeasonButton;
        if (seasonBtn != null) diaryPopUpUI.SelectSeasonTab(seasonBtn.SeasonType);
    }
}
