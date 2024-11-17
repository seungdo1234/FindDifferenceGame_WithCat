using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CatchZoneUIAnimationHandler : MonoBehaviour
{
    [Header("# Catch Zone Game Start Animation")]
    [SerializeField] private float moveY_Offset;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease setEase;
    
    [Header("# Catch Zone Component")]
    [SerializeField] private Transform mainArt;
    [SerializeField] private Transform differenceArt;

    public void CatchZoneAnimationInit()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
        GameManager.Instance.OnStartAnimationEvent += StartAnimationAnimation;
    }
    public void StartAnimationAnimation()
    {
        gameObject.SetActive(true);
        
        mainArt.localPosition = Vector2.zero;
        differenceArt.localPosition = Vector2.zero;
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainArt.DOLocalMoveY(-moveY_Offset, moveDuration).SetEase(setEase));
        sequence.Join(differenceArt.DOLocalMoveY(moveY_Offset, moveDuration).SetEase(setEase));
        sequence.OnComplete(() => {
            GameManager.Instance.GameStart();
        });
        
        sequence.Restart();
    }
}
