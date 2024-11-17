using TMPro;
using UnityEngine;
using DG.Tweening;

public class BaseUpdateText : MonoBehaviour
{
    [Header("# Base Update Text Info")]
    [SerializeField] private float updateDuration;
    
    protected TextMeshProUGUI updateText;
    protected int curValue;

    private void Awake()
    {
        updateText = GetComponent<TextMeshProUGUI>();
    }

    protected void TextInit(int value)
    {
        curValue = value;
        updateText.text = curValue.ToString();
    }
    protected virtual void TextUpdateEvent(int amount)
    {
        TextUpdateAnimation(curValue, amount);
    }
    
    private void TextUpdateAnimation(int startValue, int endValue)
    {
        curValue = endValue;
        DOTween.To(() => startValue, x => 
        {
            startValue = x;
            updateText.text = startValue.ToString(); 
        }, endValue, updateDuration).SetEase(Ease.OutQuart);
    }
}
