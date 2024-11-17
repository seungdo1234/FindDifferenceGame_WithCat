using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimerEventHandler : MonoBehaviour
{
    [Header("# Timer Data")]
    [SerializeField] private float maxGameTime;
    [SerializeField] private float decreaseFailSecond;

    [Header("# Timer Warning Event Info")]
    [SerializeField] private Image timerGaugeImage;
    [SerializeField] [Range(0f, 1f)] private float warningRatio;
    [Space(10)]
    [SerializeField] private Color warningTargetColor;
    [SerializeField] private float warningColorDuration;
    [Space(10)]
    [SerializeField] private float sliderDamping;
    [SerializeField] private float warningScaleDuration;
    private bool isWarning;

    private float curTimer;

    [Header("# Timer Text Info")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("# Clock Info")]
    [SerializeField] private Image clock;
    [SerializeField] private float rotationDuration;
    [SerializeField] private Ease setEase;

    [Header("# Smooth Animation Info")]
    [SerializeField] private float smoothLerpTime;

    [Header("# Minus Animation Info")]
    [SerializeField] private MinusTimerUIEventHandler minusTimerEvent;
    private RectTransform gaugeRect;
    private CancellationTokenSource smoothAnimationCTS;
    private bool isSmooth;

    [Header("# Timer Move Animation Info")]
    [SerializeField] private float moveDuration;
    private RectTransform timerRect;
    private Vector2 endPos;
    private Vector2 startPos;
    
    private const float Y_OFFSET = 500f;
    
    private bool IsGameOver => GameManager.Instance.IsGameOver;

    public void RegisterTimerEvent()
    {
        gaugeRect = timerGaugeImage.GetComponent<RectTransform>();
        timerRect = GetComponent<RectTransform>();
        
        GameManager.Instance.OnGameStartEvent += StartAnimationTimerEvent;
        GameManager.Instance.OnStartAnimationEvent += StartTimerMoveAnimation;
        TouchManager.Instance.OnFailTouchEvent += DecreaseTimer;

        endPos = timerRect.anchoredPosition;
        startPos = timerRect.anchoredPosition + Vector2.up * Y_OFFSET;
        
    }

    private void TimerInit()
    {
        timerGaugeImage.fillAmount = 1f;
        timerText.text = $"{maxGameTime:F0}/{maxGameTime:F0}";
        StopAllAnimation();
    }
    
    private void StartAnimationTimerEvent()
    {
        smoothAnimationCTS = new CancellationTokenSource();

        TimerEventUniTask().Forget();
        ClockAnimation();
    }

    private void StartTimerMoveAnimation()
    {
        TimerInit();
        timerRect.DOAnchorPos(endPos, moveDuration).SetEase(Ease.OutBack);
    }

    private async UniTaskVoid TimerEventUniTask()
    {
        float maxTimer = curTimer = maxGameTime;
        float warningTime = maxTimer * warningRatio;

        while (!IsGameOver && curTimer > 0) {
            if (!isSmooth) {
                curTimer -= Time.deltaTime;
                RunTimer(warningTime, maxTimer);
            }
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        if (!IsGameOver) {
            timerGaugeImage.fillAmount = 0f;
            timerText.text = $"0/0";
            StopAllAnimation();
            GameManager.Instance.GameOver(false);
        }
        
    }
    
    private void RunTimer(float warningTime, float maxTimer)
    {
        if (!isWarning && curTimer <= warningTime) {
            isWarning = true;
            UIManager.Instance.UIAnimation.ColorAnimation(timerGaugeImage, warningTargetColor, warningColorDuration);
            UIManager.Instance.UIAnimation.ScaleAnimation(timerGaugeImage.transform, sliderDamping, warningScaleDuration);
        }

        if (isWarning && curTimer > warningTime) {
            isWarning = false;
            UIManager.Instance.UIAnimation.KillColorAnimation();
            timerGaugeImage.color = Color.white;

        }

        timerGaugeImage.fillAmount = curTimer / maxTimer;
        timerText.text = $"{curTimer:F0}/{maxGameTime} ";
    }


    private void DecreaseTimer()
    {
        curTimer = Mathf.Max(curTimer - decreaseFailSecond, 0);
        float cur = timerGaugeImage.fillAmount;
        float goal = curTimer / maxGameTime;

        if (isSmooth) {
            smoothAnimationCTS?.Cancel();
            smoothAnimationCTS?.Dispose();

            smoothAnimationCTS = new CancellationTokenSource();
        }

        SmoothGaugeAnimation(cur, goal, smoothAnimationCTS.Token).Forget();

        minusTimerEvent.SetMinusUI(goal * gaugeRect.sizeDelta.x);
    }
    private async UniTaskVoid SmoothGaugeAnimation(float cur, float goal, CancellationToken cancellationToken)
    {
        float currentTime = 0f;
        isSmooth = true;
        while (currentTime < smoothLerpTime) {
            if (cancellationToken.IsCancellationRequested)
                break;

            currentTime += Time.deltaTime;
            float t = currentTime / smoothLerpTime;
            t = Mathf.Min(t, 1);
            float smoothValue = Mathf.Lerp(cur, goal, t);
            timerGaugeImage.fillAmount = smoothValue;

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        isSmooth = false;
    }

    private void StopAllAnimation()
    {
        clockRotationSequence.Kill();
        UIManager.Instance.UIAnimation.KillColorAnimation();
    }
    
    public void ReadyTimer()
    {
        timerRect.anchoredPosition = startPos;
    }

    #region Clock Animation

    private Sequence clockRotationSequence;
    private readonly Vector3 clockRightRot = new Vector3(0f, 0f, 12.5f);
    private readonly Vector3 clockLeftRot = new Vector3(0f, 0f, -10f);
    private void ClockAnimation()
    {
        clock.transform.DORotate(clockRightRot, rotationDuration / 2).SetEase(setEase).OnComplete(() => {
            if (clockRotationSequence != null)
                clockRotationSequence.Kill();

            clock.transform.localRotation = Quaternion.Euler(clockRightRot);
            clockRotationSequence = DOTween.Sequence();

            clockRotationSequence.Append(clock.transform.DORotate(clockLeftRot, rotationDuration).SetEase(setEase));
            clockRotationSequence.Append(clock.transform.DORotate(clockRightRot, rotationDuration).SetEase(setEase));

            clockRotationSequence.SetLoops(-1, LoopType.Yoyo);
            clockRotationSequence.Play();
        });
    }

  #endregion
}
