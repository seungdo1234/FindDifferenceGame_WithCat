using UnityEngine;
[System.Serializable]
public class ToggleButtonInfo
{
    [Header("# Button Animation Data Info")]
    public float btnDamping;
    public float btnMoveDuration;
    public float btnColorChangeDuration;
    
    [Header("# Button Animation Color Type")]
    public Color selectColor;
    public Color unselectedColor;
}
