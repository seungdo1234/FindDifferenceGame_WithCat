using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CountDownAnimationEventHandler : MonoBehaviour
{
    [System.Serializable]
    private class CountDownSprites
    {
        public Sprite One;
        public Sprite Two;
        public Sprite Three;
    }

    [Header("Count Down Animation Component")]
    [SerializeField] private CountDownSprites numberSprites;
    [SerializeField] private Image numberImage;
    [SerializeField] private Image backgroundImage;

    [Header("Count Down Animation Data")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float offset = 0.1f;
    [SerializeField] private Ease setEase = Ease.OutQuad;

    [Header("Fade Animation Data")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private Ease fadeEase = Ease.OutQuad;

    [Header("Component Data")]
    [SerializeField] private CatchZoneSystem catchZoneSystem;
    [SerializeField] private CatchCountUIHandler catchCount;
    [SerializeField] private TimerEventHandler timerEvent;
    [SerializeField] private BackGroundUIHandler backGroundUIHandler;
    
    private Vector3 initScale;
    private Vector3 targetScale;
    private Color numberInitColor;
    private Color backgroundInitColor;

    private void Awake()
    {
        InitializeScales();
        InitializeColors();
    }
    private void InitializeScales()
    {
        initScale = numberImage.transform.localScale;
        targetScale = initScale - (Vector3.one * offset);
        targetScale = new Vector3(Mathf.Max(targetScale.x, 0), Mathf.Max(targetScale.y, 0), Mathf.Max(targetScale.z, 0));
    }

    private void InitializeColors()
    {
        numberInitColor = numberImage.color;
        backgroundInitColor = backgroundImage.color;
    }

    public void StartCountDown()
    {
        PrepareForCountDown();
        StartAnimation();
    }
    
    #region Count Down Init
    
    private void PrepareForCountDown()
    {
        SetInitialAlpha();
        SetCanvasVisibility();
        PrepareGameComponents();
    }

    private void SetInitialAlpha()
    {
        numberImage.color = new Color(numberInitColor.r, numberInitColor.g, numberInitColor.b, 0f);
        backgroundImage.color = new Color(backgroundInitColor.r, backgroundInitColor.g, backgroundInitColor.b, 0f);
        gameObject.SetActive(true);
    }

    private void SetCanvasVisibility()
    {
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.InGameCanvas).SetActive(true);
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.LobbyCanvas).SetActive(false);
    }

    private void PrepareGameComponents()
    {
        catchZoneSystem.SetArtSpot(catchCount);
        catchCount.CatchCountInit();
        timerEvent.ReadyTimer();
        backGroundUIHandler.SetGameBackGround(true);
    }

  #endregion

    private void StartAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        AddFadeInAnimation(sequence);
        AddCountdownAnimation(sequence);
        AddFadeOutAnimation(sequence);

        sequence.OnComplete(OnAnimationComplete);
        sequence.Restart();
    }

    private void AddFadeInAnimation(Sequence sequence)
    {
        sequence.Join(numberImage.DOFade(numberInitColor.a, fadeDuration).SetEase(fadeEase));
        sequence.Join(backgroundImage.DOFade(backgroundInitColor.a, fadeDuration).SetEase(fadeEase));
    }

    private void AddCountdownAnimation(Sequence sequence)
    {
        AddScaleAnimation(sequence, numberSprites.Three);
        AddScaleAnimation(sequence, numberSprites.Two);
        AddScaleAnimation(sequence, numberSprites.One);
    }

    private void AddScaleAnimation(Sequence sequence, Sprite sprite)
    {
        sequence.AppendCallback(() => {
            numberImage.sprite = sprite;
            numberImage.transform.localScale = initScale;
        });
        sequence.Append(numberImage.transform.DOScale(targetScale, duration).SetEase(setEase));
    }

    private void AddFadeOutAnimation(Sequence sequence)
    {
        sequence.Append(numberImage.DOFade(0f, fadeDuration).SetEase(fadeEase));
        sequence.Join(backgroundImage.DOFade(0f, fadeDuration).SetEase(fadeEase));
    }

    private void OnAnimationComplete()
    {
        GameManager.Instance.GameReady();
        gameObject.SetActive(false);
        numberImage.sprite = numberSprites.Three;
    }
    
    public void ResetData()
    {
        catchZoneSystem.ResetCatchZone();
        catchCount.ResetCatchCountUIs();
    }
}