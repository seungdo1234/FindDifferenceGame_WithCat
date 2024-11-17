using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CatchCountUIHandler : MonoBehaviour
{
    [Header("# Catch UI Prefab")]
    [SerializeField] private CatchUI catchUIPrefab;

    [Header("# Set Width Info")]
    [SerializeField] private float widthOffset;

    [FormerlySerializedAs("allMovePosY")]
    [Header("# All Catch Animation Data")]
    [SerializeField] private float allMoveUpPosY;
    [SerializeField] private float allMoveDownPosY;
    [SerializeField] private float allCatchDuration;
    [SerializeField] [Range(0f, 1f)]private float allCatchDownUpMoveRatio;
    [SerializeField] private float allCatchOffsetTime;
    [SerializeField] private Ease allCatchEase;

    private int catchCount;
    private List<CatchUI> catchList = new List<CatchUI>();

    private RectTransform rect;
    private RectTransform catchUIRect;

    private bool isClear;
    public void CatchCountInit()
    {
        catchCount = DiaryManager.Instance.GetDifferenceSpotCount();
        InitializeRectTransforms();
        AddCatchUI();
        SetWidth();
        SetCatchPosition();
    }

    private void InitializeRectTransforms()
    {
        if (!rect) {
            rect = GetComponent<RectTransform>();
            catchUIRect = catchUIPrefab.GetComponent<RectTransform>();
        }
    }

    private void AddCatchUI()
    {
        int newUICount = catchCount - catchList.Count;
        for (int i = 0; i < newUICount; i++) {
            CatchUI catchUI = Instantiate(catchUIPrefab, transform);
            catchUI.CatchUIInit(this, catchList.Count);
            catchUI.gameObject.SetActive(false);
            catchList.Add(catchUI);
        }
    }

    private void SetCatchPosition()
    {
        int half = catchCount / 2;
        bool isEven = catchCount % 2 == 0;
        float offset = catchUIRect.sizeDelta.x + widthOffset;
        float plusValue = isEven ? 0.5f : 0f;

        if (!isEven)
            catchList[half].SetMovePosition(0);

        for (int i = 0; i < catchCount; i++) {
            float move = (i - half + plusValue) * offset;
            catchList[i].SetMovePosition(move);
        }
    }

    private void SetWidth()
    {
        float catchTotalWidth = (catchUIRect.sizeDelta.x * catchCount) + widthOffset;
        rect.sizeDelta = new Vector2(catchTotalWidth, rect.sizeDelta.y);
    }

    public CatchUI GetCatchUI(int catchNum)
    {
        return catchList[catchNum - 1];
    }

    public void CatchAnimationEvent(int catchNum)
    {
        AllCatchAnimationEvent(catchNum).Forget();
    }

    private int count = 0;
    private async UniTaskVoid AllCatchAnimationEvent(int catchNum)
    {
        count = 0;
        isClear = catchNum + 1 == catchCount;

        await AnimateCatchSequence(catchNum, catchNum + 1, true);
        await UniTask.WhenAll(AnimateCatchSequence(catchNum + 1, catchCount, true), AnimateCatchSequence(catchNum - 1, -1, false));
        
        if (isClear) {
            GameManager.Instance.IsCatch = true;
        }
    }

    private async UniTask AnimateCatchSequence(int start, int end, bool isPlus)
    {
        int step = isPlus ? 1 : -1;
        for (int i = start; i != end; i += step) {
            AnimateSingleCatch(i);
            await UniTask.Delay(TimeSpan.FromSeconds(allCatchOffsetTime));
            
        }
    }

    private void AnimateSingleCatch(int idx)
    {
        var sequence = DOTween.Sequence();
        float moveDuration = (allCatchDuration * (1 - allCatchDownUpMoveRatio)) / 2;
        float downUpDuration = allCatchDuration * allCatchDownUpMoveRatio;

        sequence.Append(catchList[idx].Rect.DOAnchorPosY(allMoveUpPosY, moveDuration).SetEase(allCatchEase));
        if (isClear ) 
            sequence.AppendCallback(() => CelebrateAllClearEffect(catchList[idx].transform.position));
        
        sequence.Append(catchList[idx].Rect.DOAnchorPosY(allMoveDownPosY, moveDuration).SetEase(allCatchEase));
        sequence.Append(catchList[idx].Rect.DOAnchorPosY(0, downUpDuration).SetEase(allCatchEase));

        sequence.Play();
    }
    
    private void CelebrateAllClearEffect(Vector3 pos)
    {
        PoolManager.Instance.SpawnFromPool<SuccessEffect>(E_PoolObjectType.SuccessEffect).EffectInit(E_PoolObjectType.SuccessEffect, pos);
    }
    
    public void ResetCatchCountUIs()
    {
        foreach (CatchUI catchUI in catchList) {
            catchUI.Rect.anchoredPosition = Vector3.zero;
            catchUI.gameObject.SetActive(false);
        }
    }
}
