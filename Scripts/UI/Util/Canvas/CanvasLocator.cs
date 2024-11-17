using System;
using System.Collections.Generic;
using UnityEngine;

public enum E_CanvasName { MainHUDCanvas, InGameCanvas, DiaryBookCanvas, LobbyCanvas, GameResultCanvas ,GameStartCanvas,BackGroundCanvas}
public class CanvasLocator : MonoBehaviour
{
    [SerializeField] private E_CanvasName[] activateCompileCanvasArr;

    private Dictionary<int , GameObject> canvasDict = new Dictionary<int , GameObject>();

    private void Awake()
    {
        CanvasLocatorInit();
    }

    private void Start()
    {
        StartGameRequiredActiveCanvas();
        UIManager.Instance.CanvasLocatorInit(this);
    }

    private void StartGameRequiredActiveCanvas()
    {
        foreach (E_CanvasName canvasName in Enum.GetValues(typeof(E_CanvasName))) {
            GameObject canvas = canvasDict[(int)canvasName];
            canvas.SetActive(Array.Exists(activateCompileCanvasArr, element => canvasName == element));
        }
    }

    private void CanvasLocatorInit()
    {
        foreach (E_CanvasName canvasName in Enum.GetValues(typeof(E_CanvasName))) {
            Transform findCanvas = transform.Find(canvasName.ToString());

            if (findCanvas != null)
            {
                findCanvas.gameObject.SetActive(true);

                canvasDict[(int)canvasName] = findCanvas.gameObject;
            }
            else
            {
                Debug.LogError($"{canvasName} is Empty");
            }
        }
    }
    

    public GameObject GetCanvas(E_CanvasName canvasName)
    {
        return canvasDict.GetValueOrDefault((int)canvasName);
    }
}
