using DG.Tweening;
using Do;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardLevelPopUp : MonoBehaviour
{
    //[SerializeField] Transform middleSkull;
    //[SerializeField] Transform leftSkull;
    //[SerializeField] Transform rightSkull;
    //[SerializeField] Image backLight;
    //[SerializeField] Image hardLevelText;
    //[SerializeField] List<Sprite> lightSprites = new List<Sprite>();
    //[SerializeField] List<Sprite> textSprites = new List<Sprite>();
    //[SerializeField] List<ParticleSystem> skullParticles = new List<ParticleSystem>();
    //Tween backLightTween;

    //public void PlayAnimation(bool isVeryHard)
    //{
    //    gameObject.SetActive(true);
    //    if (isVeryHard)
    //    {
    //        backLight.sprite = lightSprites[1];
    //        hardLevelText.sprite = textSprites[1];
    //    }
    //    else
    //    {
    //        backLight.sprite = lightSprites[0];
    //        hardLevelText.sprite = textSprites[0];
    //    }
    //    AnimationStart();
    //    DOVirtual.DelayedCall(2.5f, () => AnimationEnd());

    //}
    //private void AnimationStart()
    //{
    //    transform.GetComponent<Image>().DOFade(0, 0);
    //    backLight.DOFade(0, 0);
    //    middleSkull.GetComponent<Image>().DOFade(0, 0);
    //    middleSkull.localPosition = new Vector3(0, 450, 0);
    //    leftSkull.localPosition = new Vector3(-750, -115, 0);
    //    rightSkull.localPosition = new Vector3(750, -115, 0);
    //    hardLevelText.transform.localScale = Vector3.zero;
    //    foreach (var particle in skullParticles)
    //    {
    //        //particle.GetComponent<ParticleSystemRenderer>().material.DOFade(0, 0);
    //        particle.GetComponent<ParticleSystemRenderer>().material.DOFade(50/255f, 0.5f);
    //    }

    //    middleSkull.GetComponent<Image>().DOFade(1, 0.5f);
    //    middleSkull.DOLocalMoveY(145, 0.5f).SetEase(Ease.OutQuad);
    //    leftSkull.DOLocalMove(new Vector3(-280, 25, 0), 0.8f).SetEase(Ease.OutQuad);
    //    rightSkull.DOLocalMove(new Vector3(280, 25, 0), 0.8f).SetEase(Ease.OutQuad);
    //    hardLevelText.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    //    transform.GetComponent<Image>().DOFade(250 / 255f, 0.5f);
    //    backLightTween?.Kill();
    //    backLightTween = backLight.DOFade(1f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    //    AudioManager.Instance.Play(SoundType.Hard_Level);
    //}
    //private void AnimationEnd()
    //{
    //    backLightTween?.Kill();
    //    transform.GetComponent<Image>().DOFade(253/255f, 0.5f).OnComplete(() =>
    //    {
    //        transform.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() =>
    //        {
    //            gameObject.SetActive(false);
    //            if (GameManager.Instance.listOpenPopUp.Contains(gameObject))
    //                GameManager.Instance.listOpenPopUp.Remove(gameObject);
    //        });
    //        foreach (var particle in skullParticles)
    //        {
    //            particle.GetComponent<ParticleSystemRenderer>().material.DOFade(0, 0.3f);
    //        }
    //    });
    //    middleSkull.GetComponent<Image>().DOFade(0, 0.5f);

    //    middleSkull.DOLocalMoveY(450, 0.5f).SetEase(Ease.OutQuad);
    //    leftSkull.DOLocalMove(new Vector3(-750, -115, 0), 0.8f).SetEase(Ease.OutQuad);
    //    rightSkull.DOLocalMove(new Vector3(750, -115, 0), 0.8f).SetEase(Ease.OutQuad);
    //    hardLevelText.transform.DOScale(0, 0.6f).SetEase(Ease.InBack);
    //    backLight.DOFade(0, 0.6f);

    //}
}
