
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum E_CurrencyType{Gold, Gem, Pencil}
public class CurrencyEffect : PoolObject
{
    [Header("# Spawn Circle Data")]
    [SerializeField] private float radius;
    
    [Header("# Fade Animation Data")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private float moveFadeDuration;
    [SerializeField] private Ease fadeEase;
    
    [Header("# Scale Animation Data")]
    [SerializeField] private float initScale;
    [SerializeField] private float dampingScale;
    [SerializeField] private float scaleDuration;
    [SerializeField] private Ease scaleEase;
    
    [Header("# Move Animation Data")]
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    
    private SpriteRenderer spriteRenderer;
    
    private readonly Color alphaColor = new Color(1, 1, 1, 0);
    
    private const float MOVE_OFFSET = 0.15f;
    public void CurrencyEffectInit(Vector3 centeredPos, Vector3 targetPos ,E_CurrencyType type)
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = GameDataManager.Instance.CurrencySpriteDatas[(int)type];

        SetEffectPosition(centeredPos);
        StartAnimation(targetPos + Vector3.right * MOVE_OFFSET);
    }

    private void SetEffectPosition(Vector3 centeredPos)
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(0f, radius);
        
        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius;
        float y = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius;
        
        Vector3 randomPosition = centeredPos + new Vector3(x, y, 0);

        transform.position = randomPosition;
    }

    private void StartAnimation(Vector3 targetPos)
    {
        transform.localScale = initScale * Vector3.one;
        spriteRenderer.color = alphaColor;
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(spriteRenderer.DOFade(1, fadeDuration).SetEase(fadeEase));
        sequence.Join(transform.DOScale(dampingScale, scaleDuration).SetEase(scaleEase));
        
        sequence.Append(transform.DOMove(targetPos, moveDuration).SetEase(moveEase));
        
        float moveFadeInsertTime = Mathf.Max(moveDuration - moveFadeDuration, 0.01f);
        sequence.Insert(moveFadeInsertTime, spriteRenderer.DOFade(0, moveFadeDuration).SetEase(fadeEase))
            .OnComplete(() => {
               PlayerDataManager.Instance.ChangeCurrency(1);
                PoolManager.Instance.ReturnToPool(E_PoolObjectType.CurrencyEffect, this);
        });
        

        sequence.Play();
    }
}
