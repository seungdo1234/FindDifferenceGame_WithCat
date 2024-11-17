using System;
using System.Collections.Generic;
using UnityEngine;

public class CatchZoneSystem : MonoBehaviour
{
    [Header("Art Info")]
    [SerializeField] private FailSpotTouchEvent mainArt;
    [SerializeField] private FailSpotTouchEvent differenceArt;
    
    private DiaryInfo selectedDiary;
    private int catchCount;
    private int currentCount;
    private CatchCountUIHandler catchCountUIHandler;

    private readonly List<SuccessResult> successResults = new List<SuccessResult>();
    private readonly List<SuccessSpotTouchEvent> spotList = new List<SuccessSpotTouchEvent>();

    public void SetArtSpot(CatchCountUIHandler uiHandler)
    {
        catchCountUIHandler = uiHandler;
        InitializeDiaryInfo();
        SetupArtTouchEvents();
        CreateDifferenceSpots();
    }

    private void InitializeDiaryInfo()
    {
        selectedDiary = DiaryManager.Instance.SelectDiaryInfo;
        catchCount = DiaryManager.Instance.GetDifferenceSpotCount();
        currentCount = 0;
    }

    private void SetupArtTouchEvents()
    {
        mainArt.TouchStart(this, selectedDiary.diary.diaryArt);
        differenceArt.TouchStart(this, selectedDiary.diary.diaryArt);
    }

    private void CreateDifferenceSpots()
    {
        ClearExistingSpots();
        HashSet<GameObject> uniqueSpots = CollectionUtils.GetUniqueCollections(selectedDiary.diary.diaryDifferenceSpots, catchCount);
        foreach (GameObject spot in uniqueSpots)
        {
            CreateSpotPair(spot);
        }
    }

    private void ClearExistingSpots()
    {
        for (int i = differenceArt.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(differenceArt.transform.GetChild(i).gameObject);
        }
    }

    private void CreateSpotPair(GameObject spotPrefab)
    {
        Transform instanceDifferSpot = InstantiateSpot(spotPrefab, differenceArt.transform);
        Transform instanceMainSpot = InstantiateSpot(spotPrefab, mainArt.transform);

        SuccessSpotTouchEvent differSpotEvent = instanceDifferSpot.GetComponent<SuccessSpotTouchEvent>();
        SuccessSpotTouchEvent mainSpotEvent = instanceMainSpot.GetComponent<SuccessSpotTouchEvent>();

        InitializeSpotEvents(differSpotEvent, mainSpotEvent);
    }

    private Transform InstantiateSpot(GameObject prefab, Transform parent)
    {
        Transform instance = Instantiate(prefab, parent).transform;
        instance.localPosition = Vector3.zero;
        instance.localScale = Vector3.one;
        return instance;
    }

    private void InitializeSpotEvents(SuccessSpotTouchEvent differSpotEvent, SuccessSpotTouchEvent mainSpotEvent)
    {
        differSpotEvent.SpotInit(this, true, mainSpotEvent);
        mainSpotEvent.SpotInit(this, false, differSpotEvent);
        
        spotList.Add(differSpotEvent);
        spotList.Add(mainSpotEvent);
    }

    public void CheckSpots(Vector3 checkSpot, Vector3 otherSpot)
    {
        CreateSuccessResults(checkSpot, otherSpot);
        IncrementCatchCount();
        CreateCatchStarEffect(checkSpot);
    }

    private void CreateSuccessResults(Vector3 spot1, Vector3 spot2)
    {
        CreateSuccessResult(spot1);
        CreateSuccessResult(spot2);
    }

    private void CreateSuccessResult(Vector3 spot)
    {
        SuccessResult successResult = PoolManager.Instance.SpawnFromPool<SuccessResult>(E_PoolObjectType.Success);
        successResult.SuccessInit(spot);
        successResults.Add(successResult);
    }

    private void IncrementCatchCount()
    {
        if (++currentCount == catchCount)
        {
            GameManager.Instance.GameOver(true);
        }
    }

    private void CreateCatchStarEffect(Vector3 checkSpot)
    {
        CatchStarEffect catchStarEffect = PoolManager.Instance.SpawnFromPool<CatchStarEffect>(E_PoolObjectType.CatchStartEffect);
        CatchUI catchUI = catchCountUIHandler.GetCatchUI(currentCount);
        catchStarEffect.CatchStartEffectInit(checkSpot, catchUI);
    }

    public void FailSpots(Vector3 touchSpot)
    {
        FailResult failResult = PoolManager.Instance.SpawnFromPool<FailResult>(E_PoolObjectType.Fail);
        failResult.FailInit(touchSpot);
        TouchManager.Instance.CallFailTouchEvent();
    }
    
    public void ResetCatchZone()
    {
        gameObject.SetActive(false);
        ReturnSuccessResultsToPool();
        DestroySpots();
        ClearLists();
    }

    private void ReturnSuccessResultsToPool()
    {
        foreach (var result in successResults)
        {
            PoolManager.Instance.ReturnToPool(E_PoolObjectType.Success, result);
        }
    }

    private void DestroySpots()
    {
        foreach (var spot in spotList)
        {
            Destroy(spot.gameObject);
        }
    }

    private void ClearLists()
    {
        successResults.Clear();
        spotList.Clear();
    }
}