using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseToggleController : MonoBehaviour
{
    [field:Header("# Base Toggle Button Data Info")]
    [field:SerializeField] public ToggleButtonInfo ToggleBtnInfo { get; private set; }

    protected BaseToggleButtonUI selectToggle;
    
    protected void ChangeToggleEvent(BaseToggleButtonUI changeToggle, Action toggleCallback =null)
    {
        selectToggle?.ButtonClickAnimation(false);

        selectToggle = changeToggle;
        toggleCallback?.Invoke();
        
        selectToggle?.ButtonClickAnimation(true);
    }
}
