using Do;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


using System.Globalization;
using System;

public class KeepPlayingPopUp : PopUpBase
{
    [SerializeField] List<Sprite> iconSprites = new List<Sprite>();
    [SerializeField] List<Sprite> textSprites = new List<Sprite>();
    [SerializeField] Image icon, text;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] GameObject disableKeepPlayingButton;
    bool isClick = false;
    int price;
    private void Init()
    {
        icon.sprite = iconSprites[LevelManager.Instance.keepPlayingTime];
        text.sprite = textSprites[LevelManager.Instance.keepPlayingTime];
        icon.SetNativeSize();
        text.SetNativeSize();
        price = LevelManager.Instance.keepPlayingTime == 0 ? 250 : 450;
        cost.text = price.ToString();
        disableKeepPlayingButton.SetActive(GameUtils.Coin < price);
        isClick = false;
    }
    public void OnClickKeepPlaying()
    {
        if (isClick) return;
        if (GameUtils.Coin < price)
        {
            

            isClick = true;

            UIManager.Instance.UpdateCoin();
       //     AudioManager.Instance.Play(MusicType.Gameplay);
            GameManager.Instance.KeepPlayingLevel();
          
            Close();
        } 
        else
        {
            GameUtils.Coin -= price;
            isClick = true;
           
            UIManager.Instance.UpdateCoin();
         //   AudioManager.Instance.Play(MusicType.Gameplay);
            GameManager.Instance.KeepPlayingLevel();
          //  AdManager.Instance.HideBanner_AllNetWork();
            Close();
        }
    }
    public void OnClickGiveUp()
    {
        if (isClick) return;
        isClick = true;
        Close();
        GameUtils.Times_Give_Up++;
        GameUtils.Win_Streak = 0;
        UIManager.Instance.OpenLoseHeartPopUp();
        UIManager.Instance.SetBackHomeLoseHeartPopUp();
      
      
    }
    public override void Open()
    {
        AudioManager.instance.CheckBGM();
        AudioManager.instance.PlaySFX(SoundType.Lose);
        VibrationUtil.Vibrate(100);
        Init();
      
      
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
