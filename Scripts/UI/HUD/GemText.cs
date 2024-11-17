using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemText : BaseUpdateText
{
    private void Start()
    {
        UIManager.Instance.OnGemUpdateEvent += TextUpdateEvent;

        TextInit(PlayerDataManager.Instance.GetGem);
    }

    protected override void TextUpdateEvent(int amount)
    {
        base.TextUpdateEvent(amount);

    }
}