using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class SuccessResult : PoolObject
{
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float dampingScale = 1.1f;
    [SerializeField] private Ease setEase = Ease.OutBack;

    private SpriteRenderer spriteRenderer;

    public void SuccessInit(Vector3 spotPos)
    {
        transform.position = spotPos;
        transform.localScale = Vector3.zero;

        if (!spriteRenderer) {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f); // 빨간색, 완전 불투명
        SuccessAnimation();
    }

    private void SuccessAnimation()
    {
        Vector3 damping = Vector3.one * dampingScale;
        transform.DOScale(damping, animationDuration).SetEase(setEase);

        DOVirtual.DelayedCall(animationDuration * 0.3f, () => {
            PoolManager.Instance.SpawnFromPool<SuccessEffect>(E_PoolObjectType.SuccessEffect).EffectInit(E_PoolObjectType.SuccessEffect,transform.position);
        });
    }
    
}
