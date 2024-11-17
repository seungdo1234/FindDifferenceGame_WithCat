using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasePopUpUI : MonoBehaviour
{
    [Header("# Base PopUp Open Data")]
    [SerializeField] protected float initScale;
    [SerializeField] protected float openPopUpDuration;
    
    [Header("# Base PopUp Close Data")]
    [SerializeField] protected float closePopUpDuration;
    [SerializeField] protected float popUpCloseScale;

    protected Vector2 initScaleVec;
    protected Vector2 dampingScaleVec;
    protected Vector2 closeScaleVec;
    protected Transform panel;

    private readonly string panelName = "Panel";
    protected virtual void Awake()
    {
        panel = transform.Find(panelName);
        if (panel == null) {
            Debug.LogError($"{panelName} is exist !");
        }
    }

    public virtual void OpenPopUp()
    {
        gameObject.SetActive(true);
        initScaleVec = Vector2.one * initScale;
        panel.localScale = initScaleVec;
        
        panel.DOScale(Vector2.one, openPopUpDuration).SetEase(Ease.OutBack);
    }
    
    public virtual void ClosePopUp()
    {
        closeScaleVec = Vector2.one * popUpCloseScale;
        panel.DOScale(closeScaleVec, closePopUpDuration).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
    }
}
