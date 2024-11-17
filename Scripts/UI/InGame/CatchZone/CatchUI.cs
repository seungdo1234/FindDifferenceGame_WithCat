using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CatchUI : MonoBehaviour
{
    [Header("# Catch UI Color Data")]
    [SerializeField] private Color changeColor;
    [SerializeField] private Color initColor;
    [SerializeField] private float colorChangeDuration;
    [SerializeField] private Ease colorChangeEase;

    [Header("# Catch UI Damping Data")]
    [SerializeField] private float dampingScale;
    [SerializeField] private float initScale;
    [SerializeField] private float dampingDuration;
    [SerializeField] private Ease dampingEase;

    [Header("# Catch UI Init Animation Data")]
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    private float targetMoveX;

    [Header("# Catch UI Fade Animation Data")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private Ease fadeEase;

    public RectTransform Rect { get; private set; }
    private Image image;
    private CatchCountUIHandler catchCountUIHandler;
    private int catchNum;
    public void CatchUIInit(CatchCountUIHandler catchCountUIHandler, int num)
    {
        this.catchCountUIHandler = catchCountUIHandler;
        catchNum = num;

        image = GetComponent<Image>();
        Rect = GetComponent<RectTransform>();

        GameManager.Instance.OnGameStartEvent += TargetMoveAnimation;
    }

    public void SetMovePosition(float moveX)
    {
        targetMoveX = moveX;
    }

    private void TargetMoveAnimation()
    {
        image.color = initColor;
        transform.localScale = Vector3.one * initScale;
        gameObject.SetActive(true);

        image.DOFade(1f, fadeDuration).SetEase(fadeEase);
        Rect.DOAnchorPosX(targetMoveX, moveDuration).SetEase(moveEase);
    }

    public void SetCatchUI()
    {
        StartCatchAnimation();
    }

    private void StartCatchAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        float duration = dampingDuration / 2;
        sequence.Append(transform.DOScale(dampingScale, duration).SetEase(dampingEase));
        sequence.Append(transform.DOScale(initScale, duration).SetEase(dampingEase));
        sequence.Join(image.DOColor(changeColor, colorChangeDuration).SetEase(colorChangeEase));
        sequence.OnComplete(() => catchCountUIHandler.CatchAnimationEvent(catchNum));

        sequence.Play();
    }

}
