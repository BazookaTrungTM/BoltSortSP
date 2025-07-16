using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public bool isActive;
    public bool isLock;
    [SerializeField] LayoutElement layout;
    [SerializeField] Image activeImg;
    [SerializeField] Text activeText;
    [SerializeField] RectTransform icon;
    [SerializeField] RectTransform lockIcon;
    [SerializeField] float duration;
    public void ActiveToInactive()
    {
        layout.minWidth = 0;
        activeImg.DOFade(0, duration);
        activeText.DOFade(0, duration);
        icon.DOAnchorPosY(-175, duration);
        icon.DOSizeDelta(new Vector2(200, 200), duration);
    }
    public void InactiveToActive()
    {
        layout.minWidth = 100;
        activeImg.DOFade(1, duration);
        activeText.DOFade(1, duration);
        icon.DOAnchorPosY(-115, duration);
        icon.DOSizeDelta(new Vector2(270, 270), duration);
    }
    public void ShakeLock()
    {
        lockIcon.DORotate(new Vector3(0, 0, 20), 0.04f).OnComplete(() =>
        {
            lockIcon.DORotate(new Vector3(0, 0, -20), 0.08f).OnComplete(() =>
            {
                lockIcon.DORotate(Vector3.zero, 0.04f);
            });
        });
    }
    public void Init(int key)
    {
        if (key == 0) // inactive
        {
            lockIcon.gameObject.SetActive(false);
            layout.minWidth = 0;
            activeImg.DOFade(0, 0);
            activeText.DOFade(0, 0);
            icon.localScale = Vector3.one;
            icon.sizeDelta = new Vector2(200, 200);
            icon.anchoredPosition = new Vector2(0, -175);
        }
        else if (key == 1) //active
        {
            lockIcon.gameObject.SetActive(false);
            layout.minWidth = 100;
            activeImg.DOFade(1, 0);
            activeText.DOFade(1, 0);
            icon.localScale = Vector3.one;
            icon.sizeDelta = new Vector2(270, 270);
            icon.anchoredPosition = new Vector2(0, -115);
        }
        else //lock
        {
            lockIcon.gameObject.SetActive(true);
            layout.minWidth = 0;
            activeImg.DOFade(0, 0);
            activeText.DOFade(0, 0);
            icon.localScale = Vector3.zero;
        }
    }
}
