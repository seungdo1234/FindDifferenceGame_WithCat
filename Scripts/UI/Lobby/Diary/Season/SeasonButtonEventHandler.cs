using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SeasonButtonEventHandler : MonoBehaviour
{

    [Header("# Season Button Components")]
    [SerializeField] private SeasonButton[] seasonButtons;
    [SerializeField] private Image pageImage;

    [Header("# Season Button Animation Data")]
    [SerializeField] private float dampingScale;
    [SerializeField] private float initScale;
    [SerializeField] private float dampingDuration;
    [SerializeField] private Ease dampingEase;
    private int selectedSeason = -1;
    private DiaryUIEventHandler diaryUIEvent;

    private void Awake()
    {
        diaryUIEvent = GetComponentInParent<DiaryUIEventHandler>();
        diaryUIEvent.OnChangeSeasonBtnEvent += SelectSeason;
    }

    public void SeasonDiaryInit()
    {
        SelectSeason(0);
    }
    
    
    public void SelectSeason(int season)
    {
        selectedSeason = season;
        pageImage.sprite = seasonButtons[selectedSeason].page; 
        
        diaryUIEvent.SelectSeasonTab((E_SeasonType)selectedSeason);
        SelectTextAnimation();
    }

    private void SelectTextAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        RectTransform textRect = seasonButtons[selectedSeason].text;
        float duration = dampingDuration / 2;
        sequence.Append(textRect.DOScale(dampingScale, duration).SetEase(dampingEase));
        sequence.Append(textRect.DOScale(initScale, duration).SetEase(dampingEase));
        sequence.Play();
    }
    
}