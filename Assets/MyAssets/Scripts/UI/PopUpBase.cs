using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopUpBase : MonoBehaviour
{
    Tween openTween, closeTween;
    public int popUpType = 0;
    public bool isClose = false;
    [SerializeField] private Transform _transform1;
    [SerializeField] private Transform _transform2;
    public virtual void Open()
    {
        if (GameManager.Instance != null && !GameManager.Instance.listOpenPopUp.Contains(gameObject))
        {
            GameManager.Instance.listOpenPopUp.Add(gameObject);
        }
        gameObject.SetActive(true);
        isClose = false;

        if (popUpType == 0)
        {
            Luna.Unity.LifeCycle.GameEnded();
            openTween?.Kill();
            transform.GetChild(0).localScale = Vector3.zero;
            openTween = transform.GetChild(0).DOScale(1, 0.5f).SetDelay(.5f).SetEase(Ease.OutBack).OnComplete(() => openTween.Kill());
        }
        else if (popUpType == 1)
        {
            Luna.Unity.LifeCycle.GameEnded();
            openTween?.Kill();
            _transform1.localScale = Vector3.one * .95f;
            _transform2.localPosition -= new Vector3(0, 1000, 0);
            openTween = _transform2.DOLocalMoveY(1000, 0.5f).SetRelative(true).SetEase(Ease.OutBack).OnComplete(() => openTween.Kill());
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _transform2.DOScale(new Vector3(1.05f, 0.92f, 0), 0.25f).OnComplete(() =>
                {
                    _transform2.DOScale(.95f, 0.15f);
                });
            });
        }
    }

    public virtual void Close()
    {
        if (isClose) return;
        isClose = true;
        closeTween?.Kill();
        closeTween = transform.GetChild(popUpType).DOScale(0.1f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
        {
            closeTween.Kill();
            gameObject.SetActive(false);
            if (GameManager.Instance != null && GameManager.Instance.listOpenPopUp.Contains(gameObject))
            {
                GameManager.Instance.listOpenPopUp.Remove(gameObject);
            }
        });
    }
}
