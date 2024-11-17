using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundUIHandler : MonoBehaviour
{
    [Header("# BackGround Component data")]
    [SerializeField] private Image blur;
    [SerializeField] private RectTransform deskRect;
    [SerializeField] private RectTransform PostItRect;


    public void SetGameBackGround(bool isGame)
    {
        blur.gameObject.SetActive(isGame);
        deskRect.gameObject.SetActive(!isGame);
        PostItRect.gameObject.SetActive(!isGame);
    }
    
    
}
