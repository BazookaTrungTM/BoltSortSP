using DG.Tweening;
using Do;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockFeaturePopUp : PopUpBase
{
    [SerializeField] List<GameObject> iconObjs = new List<GameObject>();
    GameObject currentFeature;
    public override void Open()
    {
        if (currentFeature != null) 
            currentFeature.SetActive(false);
        currentFeature = iconObjs[GameUtils.Feature_Level_Unlock.IndexOf(GameUtils.Level)];
        currentFeature.SetActive(true);
        base.Open();
        //DOVirtual.DelayedCall(0.7f, () =>
        //{
            AudioManager.instance.PlaySFX(SoundType.Feature_Anounce);
        //});
    }

    public override void Close()
    {
        base.Close();
        DOVirtual.DelayedCall(0.4f, () => 
        {
          //  TutorialController.Instance.OpenTutorial(); 
        });
    }
}
