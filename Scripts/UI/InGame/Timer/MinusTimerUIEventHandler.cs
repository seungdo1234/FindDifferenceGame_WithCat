using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MinusTimerUIEventHandler : MonoBehaviour
{
    [Header("# Minus Animation Move Data")]
    [SerializeField] private float yOffset;
    [SerializeField] private float moveY;
    [SerializeField] private float moveAnimationDuration;
    [SerializeField] private Ease moveEase;
    
    [Header("# Minus Animation Damping Data")]
    [SerializeField] private float initScale;
    [SerializeField] private Ease dampingEase;
    
    [Header("# Minus Animation Fade Data")]
    [SerializeField] [Range(0f, 1f)]private float fadeRatio;
    [SerializeField] private Ease fadeEase;

    private RectTransform rect;
    private Image image;
    private Sequence minusSequence;
    
    public void SetMinusUI(float posX)
    {
        if (!rect) {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
        rect.anchoredPosition = new Vector2(posX, yOffset);

        StartMinusAnimation();
    }

    private void StartMinusAnimation()
    {
        if(minusSequence != null)
            minusSequence.Kill();
        
        minusSequence = DOTween.Sequence();

        rect.localScale = initScale * Vector3.one;
        float fadeInsertTime = moveAnimationDuration * fadeRatio;
        float fadeDuration = moveAnimationDuration * (1 - fadeRatio);    
        
        minusSequence.Append(rect.DOAnchorPosY(moveY, moveAnimationDuration).SetEase(moveEase));
        minusSequence.Join(rect.DOScale(Vector3.one, moveAnimationDuration / 2).SetEase(dampingEase));
        minusSequence.Join(image.DOFade(1, fadeInsertTime / 2).SetEase(fadeEase));
        minusSequence.Insert(fadeInsertTime, image.DOFade(0, fadeDuration).SetEase(fadeEase));

        minusSequence.Play();
    }
}
