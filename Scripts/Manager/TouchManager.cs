using System;
using Unity.VisualScripting;
using UnityEngine;

public class TouchManager : Singleton<TouchManager>
{
    public event Action<Touch> OnTouchBeganEvent;

    public event Action OnFailTouchEvent; 
    public event Action OnSuccessTouchEvent; 
    
    // public event Action<Touch> OnTouchMovedEvent;
    // public event Action<Touch> OnTouchEndedEvent;

    // Editor
    private Touch simulatedTouch;
    private bool isSimulatingTouch = false;

    private void Update()
    {
#if UNITY_EDITOR
        ProcessEditorInput();
#else
        ProcessTouches();
#endif
    }

    private void ProcessTouches()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            ProcessTouch(touch);
        }
    }

    private void ProcessEditorInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            simulatedTouch = CreateSimulatedTouch(TouchPhase.Began);
            isSimulatingTouch = true;
            ProcessTouch(simulatedTouch);
        }
    }

    private Touch CreateSimulatedTouch(TouchPhase touchPhase)
    {
        return new Touch
        {
            phase = touchPhase,
            position = Input.mousePosition,
            fingerId = 0
        };
    }

    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnTouchBeganEvent?.Invoke(touch);
                break;
        }
    }

    public void CallFailTouchEvent()
    {
        OnFailTouchEvent?.Invoke();
    }
    
    public void CallSuccessTouchEvent()
    {
        OnSuccessTouchEvent?.Invoke();
    }
}