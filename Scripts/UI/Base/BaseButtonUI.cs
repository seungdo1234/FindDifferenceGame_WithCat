using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class BaseButtonUI : MonoBehaviour
{
    [Header("# Base Button Data")]
    [SerializeField] private float damping;
    [SerializeField] private float BtnAnimationDuration;
    [SerializeField] private Ease setEase;
    
    protected Button btn;
    private Sequence clickSequence;
    protected virtual void Awake()
    {
        BtnInit();
    }

    protected void RegisterOnClickEvent(UnityAction registerEvent)
    {
        btn.onClick.AddListener(registerEvent);
    }

    protected void BtnInit()
    {
        if (!btn) {
            btn = GetComponent<Button>();
        }
    }
    protected void RemoveOnClickEvent( UnityAction removeEvent = null)
    {
        if (!btn) {
            btn = GetComponent<Button>();
        }
        
        if(removeEvent == null)
            btn.onClick.RemoveAllListeners();
        else
            btn.onClick.RemoveListener(removeEvent);
    }

    protected void ButtonClickAnimation(Action callback)
    {
        clickSequence?.Kill();

        Vector3 dampingVec = Vector3.one* damping;
        Vector3 initPos = transform.localScale;
        float duration = BtnAnimationDuration / 2;
        
        btn.interactable = false;
        
        clickSequence = DOTween.Sequence();

        clickSequence.Append(transform.DOScale(dampingVec, duration).SetEase(setEase));
        clickSequence.AppendCallback( ()=>  callback?.Invoke());
        clickSequence.Append(transform.DOScale(initPos, duration).SetEase(setEase));
        clickSequence.OnComplete(() => {
            btn.interactable = true;
        });
    }
}
