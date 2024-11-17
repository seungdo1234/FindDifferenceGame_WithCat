using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeUIEvent 
{
    private Dictionary<Image, float> fadeImageDict = new Dictionary<Image, float>();
    private Dictionary<TextMeshProUGUI, float> fadeTextDict = new Dictionary<TextMeshProUGUI, float>();
    
    public void TryImageFadeEvent(Image fadeText, float fadeTime)
    {
        fadeImageDict.TryAdd(fadeText, 0);

        if (fadeImageDict[fadeText] > 0)
        {
            fadeImageDict[fadeText] = fadeTime;
        }
        else
        {
            ImageFadeEffect(fadeText, fadeTime);
        }
    }

    private async UniTaskVoid ImageFadeEffect(Image fadeImage, float fadeTime)
    {
        fadeImageDict[fadeImage] = fadeTime;

        while (fadeImageDict[fadeImage] > 0 )
        {
            if (GameManager.Instance.IsGameOver) {
                fadeImage.gameObject.SetActive(false);
                break;
            }
            fadeImageDict[fadeImage] -= Time.deltaTime;

            float a = Mathf.Lerp(0, 1, fadeImageDict[fadeImage] / fadeTime);
            fadeImage.color = new Color(1, 1, 1, a);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
    
    public void TryTextFadeEvent(TextMeshProUGUI fadeText, float fadeTime)
    {
        fadeTextDict.TryAdd(fadeText, 0);

        if (fadeTextDict[fadeText] > 0)
        {
            fadeTextDict[fadeText] = fadeTime;
        }
        else
        {
            TextFadeEffect(fadeText, fadeTime);
        }
    }
    private async UniTaskVoid TextFadeEffect(TextMeshProUGUI fadeText, float fadeTime)
    {
        fadeTextDict[fadeText] = fadeTime;

        while (fadeTextDict[fadeText] > 0 )
        {
            if (GameManager.Instance.IsGameOver) {
                break;
            }
            fadeTextDict[fadeText] -= Time.deltaTime;

            float a = Mathf.Lerp(0, 1, fadeTextDict[fadeText] / fadeTime);
            fadeText.color = new Color(1, 1, 1, a);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

}
