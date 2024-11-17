using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum E_ButtonState
{
    Unlocked,
    Locked
}

public class DiaryUnLockBtn : BaseButtonUI
{
    [Header("# UnLock Button Info")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI openText;
    [SerializeField] private TextMeshProUGUI currencyText;

    private DiaryInfo currentDiaryInfo;

    protected override void Awake()
    {
        base.Awake();
        RegisterOnClickEvent(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ButtonClickAnimation(UnlockDiary);
    }

    private void UnlockDiary()
    {
        currentDiaryInfo.UnLockDiary();
        currentDiaryInfo = null;
        HideUnlockButton();
    }

    private void HideUnlockButton()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void InitializeButton(DiaryInfo diaryInfo)
    {
        if (!diaryInfo.isLock)
        {
            HideUnlockButton();
            return;
        }
        
        ShowUnlockButton();
        currentDiaryInfo = diaryInfo;
        
        E_ButtonState state = DetermineButtonState(diaryInfo);
        UpdateButtonAppearance(state);
        UpdateCurrencyText(diaryInfo.requiredCurrency);
    }

    private void ShowUnlockButton()
    {
        transform.parent.gameObject.SetActive(true);
    }

    private E_ButtonState DetermineButtonState(DiaryInfo diaryInfo)
    {
        return PlayerDataManager.Instance.HasCurrency(diaryInfo.requiredCurrency) 
            ? E_ButtonState.Unlocked : E_ButtonState.Locked;
    }

    private void UpdateButtonAppearance(E_ButtonState state)
    {
        if (!btn) {
            BtnInit();
            RegisterOnClickEvent(UnlockDiary);
        }
        
        int stateIndex = (int)state;
        buttonImage.sprite = GameDataManager.Instance.ButtonStateInfo[stateIndex].interactSprite;
        openText.color = GameDataManager.Instance.ButtonStateInfo[stateIndex].openTextColor;
        currencyText.fontMaterial = GameDataManager.Instance.ButtonStateInfo[stateIndex].outLine;
        btn.interactable = (state == E_ButtonState.Unlocked);
    }

    private void UpdateCurrencyText(int requiredCurrency)
    {
        currencyText.text = requiredCurrency.ToString();
    }
}