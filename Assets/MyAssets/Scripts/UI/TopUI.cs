using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image levelProcessFill;
    [SerializeField] TextMeshProUGUI remainGoal;
    [SerializeField] Transform remainGoalIcon;
    [SerializeField] ParticleSystem remainGoalParticle;
    [SerializeField] TextMeshProUGUI coinText;
    int goalNum;
    int goalCount;
    Tween processTween, t1, t2;

    public void Init(int goalNum, int hardLevelKey)
    {
        KillAllTween();
        goalCount = 0;
        levelText.text = "LEVEL " + GameUtils.Level.ToString();
        if (hardLevelKey == 0)
            levelText.color = Color.white;
        else if (hardLevelKey == 1)
            levelText.color = Color.red;
        else
            levelText.color = new Color (1, 0, 1, 1);
        levelProcessFill.fillAmount = 0;
        this.goalNum = goalNum;
        remainGoal.text = goalNum.ToString();
        UpdateCoin();
    }

    public void UpdateProcess()
    {
        goalCount++;
        KillAllTween();
        processTween = levelProcessFill.DOFillAmount(goalCount / (float) goalNum, 0.2f).SetEase(Ease.OutQuad);
        t1 = remainGoalIcon.DOScale(1.3f, 0.2f).OnComplete(() =>
        {
            t2 = remainGoalIcon.DOScale(1f, 0.2f);
        });
        remainGoal.text = (goalNum - goalCount).ToString();
        remainGoalParticle.Play();
    }

    public void UpdateCoin()
    {
        int coin = GameUtils.Coin;
        coinText.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
        {
            if (coin > 1000000)
            {
                int headNum = coin / 1000000;
                coinText.text = headNum.ToString() + "M";
            }
            else
                coinText.text = coin.ToString();
            coinText.transform.DOScale(1f, 0.2f);
        });
    }
    public void KillAllTween()
    {
        processTween?.Kill();
        t1?.Kill();
        t2?.Kill();
    }
}
