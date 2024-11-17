using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : BaseToggleButtonUI
{
    [field:Header("# Difficulty Button Data")]
    [field:SerializeField] public E_DifficultyType DifficultyType { get; private set; }

    private GameObject maskObj;
    private DiaryDifficultyTab difficultyTab;

    private const string DE_ACTIVE_MASK = "DeActiveMask";
    protected override void Awake()
    {
        base.Awake();
        difficultyTab = GetComponentInParent<DiaryDifficultyTab>();
        btn.onClick.AddListener(SelectSeasonClickEvent);
        toggleBtnInfo = difficultyTab.ToggleBtnInfo;

        maskObj = transform.Find(DE_ACTIVE_MASK).gameObject;
    }

    public void DifficultyBtnInit(E_DifficultyType difficultyType, bool isUnLock, Sprite btnImage)
    {
        DifficultyType = difficultyType;
        btn.interactable = isUnLock;
        maskObj.SetActive(!isUnLock);
        image.sprite = btnImage;
    }

    private void SelectSeasonClickEvent()
    {
        difficultyTab.DifficultyToggle(this);
    }

}
