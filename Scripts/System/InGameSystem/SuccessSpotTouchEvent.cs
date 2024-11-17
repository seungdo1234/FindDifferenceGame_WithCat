using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SuccessSpotTouchEvent : MonoBehaviour, IPointerDownHandler
{
    private SpriteRenderer renderer;
    private bool isCheck;

    private SuccessSpotTouchEvent otherCheckSpot;
    private CatchZoneSystem catchZoneSystem;
    
    private const int MAIN_ORDER_IN_LAYER = 11;
    private const int DIFFER_ORDER_IN_LAYER = 1;

    private BoxCollider2D col;

    private Vector3 center => col.bounds.center;
    public void SpotInit(CatchZoneSystem catchZoneSystem , bool isDifferenceArt , SuccessSpotTouchEvent otherSuccessSpot)
    {
        GameManager.Instance.OnGameStartEvent += GameStart;
        GameManager.Instance.OnGameOverEvent += GameOver;

        if (!renderer) {
            renderer = GetComponent<SpriteRenderer>();
            col = GetComponent<BoxCollider2D>();
        }

        this.otherCheckSpot = otherSuccessSpot;
        this.catchZoneSystem = catchZoneSystem;
        
        renderer.color = isDifferenceArt ? Color.white : Color.clear;
        renderer.sortingOrder = isDifferenceArt ?DIFFER_ORDER_IN_LAYER  : MAIN_ORDER_IN_LAYER;
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isCheck || GameManager.Instance.IsGameOver) return;

        CheckSpot();
        otherCheckSpot.CheckSpot();

        RunCheckAnimation();
    }

    private void CheckSpot()
    {
       isCheck = true;
    }


    private void RunCheckAnimation()
    {
        catchZoneSystem.CheckSpots(center, otherCheckSpot.center);
    }
    
    private void GameStart()
    {
        isCheck = false;
    }
    
    private void GameOver()
    {
        isCheck = false;
        GameManager.Instance.OnGameStartEvent -= GameStart;
        GameManager.Instance.OnGameOverEvent -= GameOver;
    }
}

