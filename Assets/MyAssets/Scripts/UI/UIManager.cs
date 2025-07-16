using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Do;
using DG.Tweening;



public class UIManager : Singleton<UIManager>
{
    #region Monobehaviour Func
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion


    [SerializeField] WinPopUp winPopUp;


    [SerializeField] LoseHeartPopUp loseHeartPopUp;






    public void Init(int goalNum, bool isReplay = false)
    {
        //transform.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;

        if (GameUtils.Feature_Level_Unlock.Contains(GameUtils.Level) && !isReplay)
        {
            GameManager.Instance.IsAction = true;
            GameManager.Instance.disableUI = true;
            //DOVirtual.DelayedCall(0.5f, () => OpenFeatureUnlockPopUp());
            OpenFeatureUnlockPopUp();
        }
        else if (GameUtils.Level % 10 == 4 && GameUtils.Level > 10)
        {

            OpenPopUpHardLevel(true);
        }
        else if (GameUtils.Level % 10 == 9)
        {

            OpenPopUpHardLevel(false);
        }
    }
    public void OpenWinPopUp()
    {
        // Debug.LogError("open win");
        winPopUp.Open();
    }

    public void OpenLosePopUp(bool isQuit = false)
    {
        //if (LevelManager.Instance.keepPlayingTime == 0 || (LevelManager.Instance.keepPlayingTime == 1 && LevelManager.Instance.currentGoal.Count < 4))
        //{

        //    // Debug.LogError("lOSE POPUP");
        //}   
        //else
        //{



        //}


        AudioManager.instance.CheckBGM();
        AudioManager.instance.PlaySFX(SoundType.Lose);
        loseHeartPopUp.Open();
        loseHeartPopUp.Init(isQuit);
        loseHeartPopUp.SetTextLose(true);
        loseHeartPopUp.isOpenByKeeplaying = true;
        GameUtils.Win_Streak = 0;
    }
    public void OpenSettingPopUp()
    {
        if (BoosterController.Instance.isFillingHolder || GameManager.Instance.disableUI)
            return;

        LevelManager.Instance.AddLevelSequence("S");

    }
    public void OnClickRetry()
    {
        if (BoosterController.Instance.isFillingHolder || GameManager.Instance.disableUI)
            return;

        LevelManager.Instance.AddLevelSequence("R");
        loseHeartPopUp.Open();
        loseHeartPopUp.Init();
    }
    public void OpenLoseHeartPopUp(bool isQuit = false)
    {
        loseHeartPopUp.Open();
        loseHeartPopUp.Init(isQuit);

    }
    public void SetBackHomeLoseHeartPopUp()
    {
        loseHeartPopUp.isOpenByKeeplaying = true;
    }
    public void OpenRefillHeartPopUp()
    {
        // refillHeartPopUp.Open();
    }
    public void UpdateHeartRefill()
    {
        //  refillHeartPopUp.Init();
    }
    public void RegisterRefillCloseEvent()
    {

    }
    public void OnClickButtonRetry()
    {
        if (BoosterController.Instance.isFillingHolder)
            return;
        GameManager.Instance.ReplayLevel();
    }
    public void UpdateLevelProcess()
    {
        //topUI.UpdateProcess();
    }
    public void UpdateCoin()
    {
        //  topUI.UpdateCoin();
        BoosterController.Instance.SetStatusBooster(BoosterType.Undo);
        BoosterController.Instance.SetStatusBooster(BoosterType.Fill);
        BoosterController.Instance.SetStatusBooster(BoosterType.Add_Holder);
    }
    public void OpenBoosterUnlockPopUp(BoosterType boosterType)
    {

    }
    public void OpenFeatureUnlockPopUp()
    {
        //  unlockFeaturePopUp.Open();
    }
    public void OpenPopUpHardLevel(bool isVeryHard)
    {
        // if (!GameManager.Instance.listOpenPopUp.Contains(hardLevelPopUp.gameObject))
        //     GameManager.Instance.listOpenPopUp.Add(hardLevelPopUp.gameObject);
        //  DOVirtual.DelayedCall(0.5f, () => hardLevelPopUp.PlayAnimation(isVeryHard));
    }
    public void OpenPopUpBuyBooster(BoosterType boosterType)
    {

    }
}
