
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseToggleButtonUI : MonoBehaviour
{
    protected Button btn;
    protected Image image;
    protected ToggleButtonInfo toggleBtnInfo;
    
    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
    }


    public void ButtonClickAnimation(bool isSelected)
    {
        Vector2 damping = isSelected ? Vector2.one* toggleBtnInfo.btnDamping :  Vector2.one;
        Color targetColor = isSelected ? toggleBtnInfo.selectColor : toggleBtnInfo.unselectedColor;
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(transform.DOScale(damping, toggleBtnInfo.btnMoveDuration).SetEase(Ease.OutQuart));
        sequence.Join(image.DOColor(targetColor, toggleBtnInfo.btnColorChangeDuration).SetEase(Ease.OutQuart));
        
        if (isSelected)
        {
            sequence.AppendCallback(() => btn.interactable = false);
        }
        else
        {
            sequence.AppendCallback(() => btn.interactable = true);
        }
    }
}
