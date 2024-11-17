using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PencilText : BaseUpdateText
{
    [SerializeField] private RectTransform icon;

    private Sequence iconSequence;
    private void Start()
    {
        UIManager.Instance.OnPencilUpdateEvent += TextUpdateEvent;

        TextInit(PlayerDataManager.Instance.GetPencil);
    }

    protected override void TextUpdateEvent(int amount)
    {
        base.TextUpdateEvent(amount);

        scaleAnimation();
    }

    private void scaleAnimation()
    {
        if(iconSequence != null)
            iconSequence.Kill();

        iconSequence = DOTween.Sequence();

        iconSequence.Append(icon.DOScale(1.1f, 0.15f).SetEase(Ease.OutQuad));
        iconSequence.Append(icon.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));
    }
}
