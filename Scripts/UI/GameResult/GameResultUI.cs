using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [Header("# Game Result Damping Animation Data")]
    [SerializeField] private float imageInitScale;
    [SerializeField] private float imageDampingScale;
    [Space(10)]
    [SerializeField] private float buttonInitScale;
    [SerializeField] private float buttonDampingScale;
    [Space(10)]
    [SerializeField] private float dampingDuration;
    [SerializeField] private Ease dampingEase;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failSprite;

    [Header("# Game Result Text Animation Data")]
    [SerializeField] private float textAnimationDuration;
    [SerializeField] private Ease textEase;
    
    
    [Header("# Game Result Component")]
    [SerializeField] private Image resultImage;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject rewardFrame;
    [SerializeField] private GameResultOkayBtn okayBtn;

    [SerializeField] private Transform pencilText;
    private int rewardCount;
    public void OnGameResultUI(bool isClear)
    {
        gameObject.SetActive(true);
        ResultInit(isClear);
        StartResultImageAnimation();
    }

    private void ResultInit(bool isClear)
    {
        rewardFrame.SetActive(isClear);
        okayBtn.BtnInit(this);
        
        if (isClear) {
            resultImage.sprite = successSprite;
            // 보상 텍스트 설정
            DiaryManager.Instance.ClearDiary();
            rewardCount = DiaryManager.Instance.GetRewardCount();
            AnimateNumber(0, rewardCount);
        }
        else {
            resultImage.sprite = failSprite;
        }
    }

    private void StartResultImageAnimation()
    {
        resultImage.transform.localScale = imageInitScale * Vector3.one;
        okayBtn.transform.localScale = buttonInitScale * Vector3.one;
        
        resultImage.transform.DOScale(imageDampingScale, dampingDuration).SetEase(dampingEase);
        okayBtn.transform.DOScale(buttonDampingScale, dampingDuration).SetEase(dampingEase);
    }
    
    private void AnimateNumber(int startValue, int endValue)
    {
        DOTween.To(() => startValue, x => 
        {
            startValue = x;
            rewardText.text = startValue.ToString(); 
        }, endValue, textAnimationDuration).SetEase(textEase);
    }

    public void OnClickEvent()
    {
        SpawnCurrencyEffect().Forget();
    }
    
    private async UniTaskVoid SpawnCurrencyEffect()
    {
        for (int i = 0; i < rewardCount; i++) {
            CurrencyEffect currencyEffect = PoolManager.Instance.SpawnFromPool<CurrencyEffect>(E_PoolObjectType.CurrencyEffect);
            currencyEffect.CurrencyEffectInit(rewardText.transform.position, pencilText.position, E_CurrencyType.Pencil);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        GoToLobby();
    }

    private void GoToLobby()
    {
        gameObject.SetActive(false);
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.BackGroundCanvas).GetComponent<BackGroundUIHandler>().SetGameBackGround(false);
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.InGameCanvas).SetActive(false);
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.GameStartCanvas).GetComponent<CountDownAnimationEventHandler>().ResetData();
        UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.LobbyCanvas).GetComponent<LobbyAnimationEvent>().OpenLobby();

    }
}
