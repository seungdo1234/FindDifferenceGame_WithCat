using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DiaryPopUpSeasonButton : BaseToggleButtonUI
{
    [field:Header("# Season Button Type")]
    [field:SerializeField] public E_SeasonType SeasonType { get; private set; }
    
    private DiaryPopUpSeasonTab seasonTab;
    
    protected override void Awake()
    {
        base.Awake();
        seasonTab = GetComponentInParent<DiaryPopUpSeasonTab>();
        toggleBtnInfo = seasonTab.ToggleBtnInfo;
        btn.onClick.AddListener(SelectSeasonClickEvent);
    }

    private void SelectSeasonClickEvent()
    {
        seasonTab.ChangeSeasonToggle(this);
    }

    
}
