using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FailSpotTouchEvent : MonoBehaviour, IPointerDownHandler
{

    private SpriteRenderer renderer;
    private CatchZoneSystem catchZoneSystem;
    private bool isCatch;
    private Camera mainCamera;

  
    public void TouchStart(CatchZoneSystem catchZoneSystem, Sprite sprite)
    {
        this.catchZoneSystem = catchZoneSystem;
        GameManager.Instance.OnGameStartEvent += GameStart;
        GameManager.Instance.OnGameOverEvent += GameOver;
        
        if (!renderer) {
            renderer = GetComponent<SpriteRenderer>();
            mainCamera = Camera.main;
        }
        renderer.sprite = sprite;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isCatch || GameManager.Instance.IsGameOver) return;

        FailTouchEffect(eventData.position);
    }

    private void FailTouchEffect(Vector2 pos)
    {
        Vector2 world = mainCamera.ScreenToWorldPoint(pos);
        catchZoneSystem.FailSpots(world);
    }

    private void GameStart()
    {
        isCatch = true;
    }
    
    private void GameOver()
    {
        isCatch = false;
        GameManager.Instance.OnGameStartEvent -= GameStart;
        GameManager.Instance.OnGameOverEvent -= GameOver;
    }
}
