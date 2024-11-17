using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CatchStarEffect : PoolObject
{
    [Header("# Catch Star Particle Component")]
    [SerializeField] private ParticleSystem parentParticle;
    [SerializeField] private ParticleSystem[] childParticles;
    
    [Header("# Catch Star Animation Move Data")]
    [SerializeField] private float moveAnimationDuration;
    [SerializeField] private Ease moveEase;
    
    [Header("# Catch Star Animation Fade Data")]
    [SerializeField] [Range(0f, 1f)]private float fadeRatio;
    [SerializeField] private Ease fadeEase;
    
    private CatchUI target;
    
    public void CatchStartEffectInit(Vector2 curPos, CatchUI targetCatchUI)
    {

        transform.position = curPos;
        target = targetCatchUI;

        StartAnimation();
    }

    private void StartAnimation()
    {
        
        Sequence sequence = DOTween.Sequence(); ;    
        
        sequence.Append(transform.DOMove(target.transform.position, moveAnimationDuration).SetEase(moveEase));
        sequence.OnComplete(() => {
            CatchMatchedEffect matchEffect = PoolManager.Instance.SpawnFromPool<CatchMatchedEffect>(E_PoolObjectType.CatchMatchedEffect);
            matchEffect.EffectInit(E_PoolObjectType.CatchMatchedEffect ,target.transform.position);
            target.SetCatchUI();
            PoolManager.Instance.ReturnToPool(E_PoolObjectType.CatchStartEffect, this);
        });
    }
    

}
