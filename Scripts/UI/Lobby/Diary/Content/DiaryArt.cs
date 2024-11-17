using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryArt : MonoBehaviour
{
    [Header("# Diary Art Info")]
    [SerializeField] private Image art;

    [Header("# Diary Lock Info")]
    [SerializeField] private DiaryUnLockBtn diaryUnLockBtn;

    public void DiaryArtInit(DiaryInfo diary )
    {
        DiaryInfo info = diary;
        diaryUnLockBtn.InitializeButton(info);
        art.sprite = info.diary.diaryArt;
    }
}
