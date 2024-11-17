using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public CanvasLocator CanvasLocator {  get; private set; }
    public UIAnimationEvent UIAnimation {  get; private set; }

    public event Action<int> OnPencilUpdateEvent;
    public event Action<int> OnGoldUpdateEvent;
    public event Action<int> OnGemUpdateEvent;
    public Camera MainCamera { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        UIAnimation = new UIAnimationEvent();
        
        MainCamera = Camera.main;
    }

    public void CanvasLocatorInit(CanvasLocator locator)
    {
        this.CanvasLocator = locator;
    }

    public void CallPencilUpdateEvent( int amount)
    {
        OnPencilUpdateEvent?.Invoke(amount);
    }
    public void CallGoldUpdateEvent( int amount)
    {
        OnGoldUpdateEvent?.Invoke(amount);
    }
    public void CallGemUpdateEvent( int amount)
    {
        OnGemUpdateEvent?.Invoke(amount);
    }
}
