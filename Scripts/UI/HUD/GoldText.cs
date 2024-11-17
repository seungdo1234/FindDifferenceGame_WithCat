using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldText : BaseUpdateText
{
    private void Start()
    {
        UIManager.Instance.OnGoldUpdateEvent += TextUpdateEvent;

        TextInit(PlayerDataManager.Instance.GetGold);
    }

    protected override void TextUpdateEvent(int amount)
    {
        base.TextUpdateEvent(amount);

    }
}
