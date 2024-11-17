using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class LobbyAnimationEvent : MonoBehaviour
{
    [System.Serializable]
    private class UIElement
    {
        public RectTransform rect;
        public Vector2 startPos;
        public Vector2 endPos;
        public Vector2 direction;
    }

    [Header("# Lobby Animation Data")]
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] [Range(0f, 1f)] private float overShootRatio = 0.3f;
    [SerializeField] private Ease startMoveEase = Ease.OutQuad;
    [SerializeField] private Ease endMoveEase = Ease.InQuad;

    [Header("# Lobby Component Data")]
    [SerializeField] private UIElement diary;
    [SerializeField] private UIElement diaryContent;
    [SerializeField] private UIElement diaryBook;
    [SerializeField] private UIElement startBtn;
    [SerializeField] private DiaryUIEventHandler diaryUIEventHandler;
    [SerializeField] private GameObject touchBlock;

    private const float MOVE_DISTANCE = 1000f;
    private const float MOVE_OFFSET = 50f;

    private void Start()
    {
        SetInitPositions();
        OpenLobby();
        GameManager.Instance.gameData.GameDataInit().Forget();
    }

    public void OpenLobby()
    {
        gameObject.SetActive(true);
        RunMoveLobbyAnimation(true, false);
    }

    private void SetInitPositions()
    {
        SetElementPositions(diary, Vector2.left);
        SetElementPositions(diaryContent, Vector2.right);
        SetElementPositions(diaryBook, Vector2.right);
        SetElementPositions(startBtn, Vector2.down);
    }

    private void SetElementPositions(UIElement element, Vector2 direction)
    {
        element.endPos = element.rect.anchoredPosition;
        element.startPos = element.endPos + direction * MOVE_DISTANCE;
        element.direction = -direction;  // Reverse direction for animation
    }
    
    private void RunMoveLobbyAnimation(bool isOpenLobby , bool isGameStart)
    {
        touchBlock.SetActive(true);
        
        AnimateElement(diary, isOpenLobby);
        AnimateElement(diaryContent, isOpenLobby);
        AnimateElement(diaryBook, isOpenLobby);
        AnimateElement(startBtn, isOpenLobby);
        
        if(isOpenLobby) {
                GameManager.Instance.gameData.DiaryContentInit();
        }
        
        CompleteMoveEvent(isGameStart).Forget();
    }

    private async UniTaskVoid CompleteMoveEvent(bool isGameStart)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(moveDuration));

        touchBlock.SetActive(false);

        if (isGameStart) {
            GameObject countDown = UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.GameStartCanvas);
            countDown.GetComponent<CountDownAnimationEventHandler>().StartCountDown();
        }
    }

    public void GameStartEvent()
    {
        diaryUIEventHandler.StartDiaryGame();
        RunMoveLobbyAnimation(false, true);
    }
    
    private void AnimateElement(UIElement element, bool isStart)
    {
        Vector2 from = isStart ? element.startPos : element.endPos;
        Vector2 to = isStart ? element.endPos : element.startPos;
        Ease ease = isStart ? startMoveEase : endMoveEase;

        Vector2 overshootPos = (isStart ? to : from) + element.direction * MOVE_OFFSET;

        Sequence sequence = DOTween.Sequence();

        float overShootDuration = moveDuration * (isStart ? overShootRatio : 1 - overShootRatio);
        float backDuration = moveDuration - overShootDuration;

        element.rect.anchoredPosition = from;

        sequence.Append(element.rect.DOAnchorPos(overshootPos, overShootDuration).SetEase(ease));
        sequence.Append(element.rect.DOAnchorPos(to, backDuration).SetEase(ease));

        if (element == diaryContent && !isStart) {
            sequence.InsertCallback(moveDuration * 0.2f,()=> diaryUIEventHandler.ScrollMove(0));
        }
        sequence.Play();
    }
    
}