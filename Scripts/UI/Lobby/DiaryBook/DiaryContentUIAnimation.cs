using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiaryContentUIAnimation : MonoBehaviour
{
    [Header("# Diary Content Animation Data")]
    [SerializeField] private float duration;

    private readonly Vector3 initVec = new Vector3(1f, 0.3f, 1f);
    public void ChangeSeasonDiaryContents()
    {
        transform.localScale = initVec;

        transform.DOScaleY(1f, duration).SetEase(Ease.OutBack);
    }
    
    
}
