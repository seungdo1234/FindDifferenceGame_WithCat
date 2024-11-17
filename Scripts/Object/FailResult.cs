using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization; // DOTween 사용을 위해 추가

public class FailResult : PoolObject
{
    [Header("# Damping Data")]
    [SerializeField] private float dampingScale;
    [SerializeField] private float dampingUpScale;
    [SerializeField] [Range(0f,1f)] private float dampingUpRatio;
    [SerializeField] private float dampingDuration;
    [SerializeField] private Ease setDampingEase;
    [SerializeField] private float initScale;
    
    [Header("# Fade Data")]
    [SerializeField] [Range(0f,1f)] private float fadeRatio;
    [SerializeField] private Ease setFadeEase;
    
    private SpriteRenderer spriteRenderer;

    public void FailInit(Vector3 spotPos)
    {
        if(!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        transform.position = spotPos;
        PlayFailAnimation();
    }

    private void PlayFailAnimation()
    {
        transform.localScale = Vector3.one * initScale;

        Sequence failSequence = DOTween.Sequence();
        
        float dampingDownTime = dampingDuration * dampingUpRatio;
        float dampingUpTime = dampingDuration * (1- dampingUpRatio); 
        float fadeInsertTime = dampingDuration * fadeRatio;
        float fadeDuration = dampingDuration * (1 - fadeRatio);
        
        failSequence.Append(transform.DOScale(dampingScale, dampingDownTime).SetEase(setDampingEase));
        failSequence.Join(spriteRenderer.DOFade(1f, fadeInsertTime / 2).SetEase(setFadeEase));
        failSequence.Append(transform.DOScale(initScale, dampingUpTime).SetEase(setDampingEase));
        failSequence.Insert(fadeInsertTime, spriteRenderer.DOFade(0f, fadeDuration).SetEase(setFadeEase));
        failSequence.OnComplete(() => PoolManager.Instance.ReturnToPool(E_PoolObjectType.Fail, this));
    }
}
