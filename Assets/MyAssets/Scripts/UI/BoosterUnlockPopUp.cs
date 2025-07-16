using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUnlockPopUp : PopUpBase
{
    [SerializeField] Image titleText;
    [SerializeField] Image iconBooster;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] List<Sprite> titleSprites = new List<Sprite>();
    [SerializeField] List<Sprite> iconSprites = new List<Sprite>();
    [SerializeField] List<string> contents = new List<string>()
    {
        "Revert the last move!",
        "Complete a holder bolt!",
        "Add an extra holder"
    };

    public void OpenByType(BoosterType type)
    {
        Open();
        GameManager.Instance.IsAction = false;
        titleText.sprite = titleSprites[ (int) type];
        iconBooster.sprite = iconSprites[ (int) type];
        content.text = contents[ (int) type];
        titleText.SetNativeSize();
        iconBooster.SetNativeSize();
    }
    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
   
        GameManager.Instance.disableUI = false;
        LevelManager.Instance.OpenHandTutLevel6();

    }
}
