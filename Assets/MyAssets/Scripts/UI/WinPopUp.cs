using DG.Tweening;
using Do;
using Do.Scripts.Loading;
using Do.Scripts.Tools;
using System;
using System.Globalization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WinPopUp : PopUpBase
{
    [SerializeField] List<GameObject> featureSprites = new List<GameObject>();
    [SerializeField] Image icon;
    [SerializeField] Image featureFill;
 
    [SerializeField] RectTransform ground;
    [SerializeField] TextMeshProUGUI textFill;
    [SerializeField] TextMeshProUGUI textCoin;
  
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] ParticleSystem particle;
    private List<Vector2> iconPositions = new List<Vector2>()
    {
        new Vector2(-35, 0), new Vector2(-35, -15), Vector2.zero, new Vector2(-10, -40), new Vector2(-35,  -15), Vector2.zero, new Vector2(0, -20), new Vector2(-35,  -15), new Vector2(0, -20)
    };
    [SerializeField] Transform well, done;
    [SerializeField] CoinEffect coinEffect;
    [SerializeField] float timeSuggestX2 = 2f;
   
    [SerializeField] RectTransform posCoinEnd;
    GameObject currentFeature;
    int previousLevelUnlock = 1;
    int coinReward;
    bool isX2 = false;
    bool isCompleted = false;
    private Tween btnNextTween;
    public void Init()
    {

   


        if (GameUtils.Cur_Feature_Index < featureSprites.Count)
        {
            if (currentFeature != null)
                currentFeature.SetActive(false);
            currentFeature = featureSprites[GameUtils.Cur_Feature_Index];
            currentFeature.SetActive(true);
            previousLevelUnlock = GameUtils.Cur_Feature_Index == 0 ? 1 : GameUtils.Feature_Level_Unlock[GameUtils.Cur_Feature_Index - 1];
            float fillAmout = 0f;
            if (GameUtils.Level < 6)
                fillAmout = 0;
            featureFill.fillAmount = fillAmout;
            textFill.text = ((int)(fillAmout * 100)).ToString() + "%";
            //btnNext.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140, 55);
            //btnX2.GetComponent<RectTransform>().anchoredPosition = new Vector2(140, 49);
            //ground.anchoredPosition = new Vector2(0, -50);
        }
        else
        {
            icon.transform.parent.gameObject.SetActive(false);
            //btnNext.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140, 100);
            //btnX2.GetComponent<RectTransform>().anchoredPosition = new Vector2(140, 94);
            //ground.anchoredPosition = new Vector2(0, 50);
        }
        if (GameUtils.Level == 5)
            coinReward = 40;
        else if (GameUtils.Level % 10 == 0)
            coinReward = 30;
        else if (GameUtils.Level % 10 == 5)
            coinReward = 50;
        else
            coinReward = 10;
        textCoin.text = "+" + coinReward.ToString();
        isX2 = false;
        isCompleted = false;
    }
    public void OnClickNext()
    {
      //  btnNext.enabled = false;
        CompleteLevel();
    }
    public void OnClickX2()
    {
        if (isX2) return;
       
        GameUtils.TimeStartAds = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);
     //   btnNext.enabled = false;
        isX2 = true;
        textCoin.transform.DOScale(1.3f, 0.2f).OnComplete(() =>
        {
            coinReward *= 2;
            textCoin.text = "+" + coinReward.ToString();
            textCoin.transform.DOScale(1, 0.2f).OnComplete(() =>
            {
                CompleteLevel();
            });
        });
       

    }
    private void CompleteLevel()
    {
        if (isCompleted) return;
        isCompleted = true;
   

        Close();
      
        DOVirtual.DelayedCall(0.5f, () =>
        {
            GameManager.Instance.disableUI = true;
           // AudioManager.instance.PlaySFX(SoundType.BGM, isBGM:true);
            GameManager.Instance.NextLevel();
        });
    }

    public GameObject gamepbject1;
    public GameObject gamepbject2;
    public override void Open()
    {
        gameObject.SetActive(true);
        Init();
        gamepbject1.transform.localScale = Vector3.one;
        gamepbject2.gameObject.SetActive(false);
       //  AudioManager.instance.CheckBGM();
         AudioManager.instance.PlaySFX(SoundType.Win);
        well.parent.eulerAngles = new Vector3(0, 0, -8.5f);
        well.localPosition = new Vector3(-821, -52, 0);
        done.localPosition = new Vector3(808, 212, 0);
        well.DOLocalMove(new Vector3(-207, 56, 0), 0.4f).SetEase(Ease.OutBack);
        done.DOLocalMove(new Vector3(197, 117, 0), 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            VibrationUtil.Vibrate(150);
            well.parent.transform.DORotate(Vector3.zero, 2.5f);
        });
        DOVirtual.DelayedCall(0.3f, () =>
        {
            //particle.Play();
            particles[0].Play();
            AudioManager.instance.PlaySFX(SoundType.Firework);
        });
        DOVirtual.DelayedCall(0.6f, () =>
        {
            particles[1].Play();
            AudioManager.instance.PlaySFX(SoundType.Firework);
        });
        DOVirtual.DelayedCall(.9f, () =>
        {
            particles[2].Play();
            AudioManager.instance.PlaySFX(SoundType.Firework);
        });
        DOVirtual.DelayedCall(1.1f, () =>
        {
            gamepbject2.SetActive(true);
             gamepbject1.transform.DOScale(0, 2f);
            //btnNext.GetComponent<DOTweenAnimation>().DOPause();
           // btnNext.GetComponent<DOTweenAnimation>().DORewind();


            if (btnNextTween != null)
            {
                btnNextTween.Pause();  // T??ng ???ng DOPause
                btnNextTween.Rewind(); // T??ng ???ng DORewind
            }
      
            base.Open();
            DOVirtual.DelayedCall(0.6f, () =>
            {
           
             
              
             
                float newRatio = 0.2f;
                //AudioManager.instance.PlaySFX(SoundType.Update_Progress);
                if (newRatio >= 1)
                {
                    newRatio = 1;
                    GameUtils.Cur_Feature_Index++;
                    featureFill.DOFillAmount(newRatio, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                        textFill.transform.DOScale(1.4f, 0.5f).OnComplete(() =>
                        {
                            textFill.text = ((int)(newRatio * 100)).ToString() + "%";
                            textFill.transform.DOScale(1f, 0.3f).OnComplete(() =>
                            {
                                AudioManager.instance.PlaySFX(SoundType.Done_Progress); ;
                            });
                        });
                    });
                }
                else
                    featureFill.DOFillAmount(newRatio, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                        textFill.transform.DOScale(1.4f, 0.5f).OnComplete(() =>
                        {
                            textFill.text = ((int)(newRatio * 100)).ToString() + "%";
                            textFill.transform.DOScale(1f, 0.3f);
                        });
                    });


            });
        });
    }

    public override void Close()
    {
        base.Close();
    }
}
