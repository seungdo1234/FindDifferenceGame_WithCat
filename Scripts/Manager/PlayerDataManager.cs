using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerSaveData
{
    public PlayerData playerData;
    public List<DiarySaveInfo> DiarySaveInfos;
}

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [SerializeField] private PlayerData data;

    public int GetPencil => data.pencil;
    public int GetGold => data.gold;
    public int GetGem => data.gem;

    private SaveLoadSystem saveLoadSystem;

    protected override void Awake()
    {
        base.Awake();
        saveLoadSystem = new SaveLoadSystem();
    }
    public bool HasCurrency(int amount)
    {
        return data.pencil >= amount;
    }

    public void ChangeCurrency(int amount)
    {
        data.pencil += amount;
        UIManager.Instance.CallPencilUpdateEvent( data.pencil);
    }
    
    public bool HasGold(int amount)
    {
        return data.gold >= amount;
    }

    public void ChangeGold(int amount)
    {
        data.gold += amount;
        UIManager.Instance.CallGoldUpdateEvent(data.gold);
    }
    public bool HasGem(int amount)
    {
        return data.gem >= amount;
    }

    public void ChangeGem(int amount)
    {
        data.gem += amount;
        UIManager.Instance.CallGemUpdateEvent(data.gem);
    }
}

