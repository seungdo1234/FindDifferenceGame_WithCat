using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAnimationEvent
{
    public FadeUIEvent FadeUI;

    public UIAnimationEvent()
    {
        FadeUI = new FadeUIEvent();

       // GameManager.Instance.OnGameOverEvent += AnimationKillAll;
    }

    public void PunchAnimation(Transform ui, float damping)
    {
        ui.localScale = Vector3.one;
        float scale = ui.localScale.x;

        Sequence uiSequence = DOTween.Sequence();
        uiSequence.Append(ui.DOScale(scale - damping, 0.1f).SetEase(Ease.OutQuad));
        uiSequence.Append(ui.DOScale(scale + damping, 0.1f).SetEase(Ease.OutQuad));
        uiSequence.OnComplete(() => { ui.localScale = Vector3.one; });
    }

    public void MoveYAnimation(Transform ui, float duration, float offset, Action callback = null)
    {
        DOTween.Kill(ui);
        float move = ui.position.y + offset;
        ui.DOMoveY(move, duration).SetEase(Ease.OutQuad).OnComplete(() => {
            ui.gameObject.SetActive(false);
            callback?.Invoke();
        });
    }

    public void ColorAnimation(Image ui, Color targetColor, float duration)
    {
        ui.DOColor(targetColor, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("Color");

    }

    public void ScaleAnimation(Transform ui, float damping, float duration)
    {
        ui.DOScale(damping, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("Color");
    }

    public void ChangeUIColor(Image uiElement, Color targetColor, float duration)
    {
        if (uiElement != null) {
            uiElement.DOColor(targetColor, duration);
        }
    }

    public void KillColorAnimation()
    {
        DOTween.Kill("Color");
    }

    public void AnimationKillAll()
    {
        DOTween.KillAll();
    }
}