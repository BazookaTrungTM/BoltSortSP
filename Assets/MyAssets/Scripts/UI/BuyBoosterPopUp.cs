using DG.Tweening;

using System;
using System.Globalization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyBoosterPopUp : PopUpBase
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Image iconBooster;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] List<Sprite> iconSprites = new List<Sprite>();
    [SerializeField] List<string> titles = new List<string>()
    {
        "UNDO",
        "SPECIAL NUT",
        "EXTRA HOLDER"
    };
    [SerializeField] List<string> contents = new List<string>()
    {
        "Revert the last move!",
        "Complete a holder bolt!",
        "Add an extra holder!"
    };
    BoosterType currentType;

    public void Init(BoosterType type)
    {
        currentType = type;
        titleText.text = titles[(int)type];
        iconBooster.sprite = iconSprites[(int)type];
        content.text = contents[(int)type];
        iconBooster.SetNativeSize();
    }
    public void OnClickGetFreeByAd()
    {
        GameUtils.TimeStartAds = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);

        if (currentType == BoosterType.Undo)
            GameUtils.Undo_Booster++;
        else if (currentType == BoosterType.Fill)
            GameUtils.Fill_Hol_Booster++;
        else if (currentType == BoosterType.Add_Holder)
            GameUtils.Add_Hol_Booster++;
        BoosterController.Instance.SetStatusBooster(currentType);

        Close();
    }
    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
