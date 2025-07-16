using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Do;
using Spine;

public class GoalScrew : MonoBehaviour
{
    [SerializeField] Transform arrowParent;
    [SerializeField] Transform root;
    [SerializeField] MeshRenderer colorRenderer;
    [SerializeField] Animator animator;
    public Transform itemHolder;
    public NutType type;
    public Transform gate;
    public int size;
    public float delta;
    public int index;
    public ColorType color;
    public ColorType initColor;
    public ColorType gradientColor;
    public int amountSortPre;
    public List<Nut> myNutsList = new List<Nut>();
    public int ropeCount = 0;

    Tween arrowTween;

    private int _closeTrigger = Animator.StringToHash("Close");
    private int _openTrigger = Animator.StringToHash("Open");
    private int _baseColor = Shader.PropertyToID("_BaseColor");
    // private int _metalic = Shader.PropertyToID("_Metallic");
    // private int _smoothness = Shader.PropertyToID("_Smoothness");

    public void Init(bool isRecycle)
    {
        myNutsList.Clear();
        amountSortPre = 0;
        ropeCount = 0;
        SetIdle();
        if (isRecycle)
        {
            animator.SetTrigger(_openTrigger);
            DOVirtual.DelayedCall(0.5f, () => animator.enabled = false);
        }
        else
            animator.enabled = false;

        //AnimOpen();
    }
    private void SetIdle()
    {
        root.localPosition = Vector3.zero;
        root.localRotation = Quaternion.identity;
        root.localScale = Vector3.one;
        root.GetChild(3).localPosition = Vector3.zero;
        root.GetChild(3).localRotation = Quaternion.identity;
        root.GetChild(3).localScale = Vector3.one;
    }
    public void AnimOpen()
    {
        root.localPosition = Vector3.zero;
        root.localScale = Vector3.one * 0.1f;
        root.GetChild(3).localPosition = new Vector3(0, -5, 0);
        root.DOScale(1, 0.1f);
        root.GetChild(3).DOLocalMoveY(-3.25f, 0.16f).OnComplete(() =>
        {
            root.GetChild(3).DOLocalMoveY(0, 0.24f);
        });
    }
    private void AnimClose()
    {
        DOVirtual.DelayedCall(0.15f, () =>
        {
            root.GetChild(3).DOLocalMoveY(-3.25f, 0.12f).OnComplete(() =>
            {
                root.DOScale(0.1f, 0.08f);
                root.GetChild(3).DOLocalMoveY(-5, 0.08f).OnComplete(() => PlayCloseEffect());
            });
        });
    }
    public void ChangeColorMaterial(bool isInit = false)
    {
        colorRenderer.materials[0].DOColor(Factory.Instance.GetColorNut(color), _baseColor, isInit ? 0 : 0.3f).SetEase(Ease.InQuad);
        if (color == ColorType.None)
        {
            // colorRenderer.materials[0].DOFloat(0.924f, _metalic, isInit ? 0 : 0.3f).SetEase(Ease.InQuad);
            // colorRenderer.materials[0].DOFloat(0.576f, _smoothness, isInit ? 0 : 0.3f).SetEase(Ease.InQuad);
        }
        else
        {
            // colorRenderer.materials[0].DOFloat(0.8f, _metalic, isInit ? 0 : 0.3f).SetEase(Ease.InQuad);
            // colorRenderer.materials[0].DOFloat(0.4f, _smoothness, isInit ? 0 : 0.3f).SetEase(Ease.InQuad);
        }
        //colorRenderer.material = mat;
    }
    public void AddNut(Nut nut)
    {
        nut.transform.localScale = Vector3.one;
        myNutsList.Add(nut);
        if (gradientColor == ColorType.None)
        {
            gradientColor = nut.data.color;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                ChangeColorMaterial();
                LevelManager.Instance.ChangeGoalGradientColor(index, nut.data.color);
            });
        }
    }
    public void RemoveNut(Nut nut)
    {
        if (myNutsList.Contains(nut))
            myNutsList.Remove(nut);
    }
    public void SortNutPos(Nut nut, int index, bool isBooster = false)
    {
        Vector3 nutPos = new Vector3(0, index * delta, 0);
        nut.SortPos(nutPos, () =>
        {
            if (!isBooster)
                nut.PlayCompleteParticle();
            else
                EffectManager.Instance.PlayNutSpecialEffect(nut.transform.position);


            // Debug.LogError("SortNutPos");
            CheckDoneGoal(index);
        });
    }
    public void CheckDoneGoal(int indexInList)
    {
        if (indexInList + 1 == size)
        {
            // Debug.LogError("CheckDoneGoal");
            LevelManager.Instance.CheckOpenTowel(color);
            // Debug.LogError("CheckDoneGoal1");
            LevelManager.Instance.CheckWinLevel();
            // Debug.LogError("CheckDoneGoal2");
            Vector3 pos = transform.localPosition;
            // Debug.LogError("CheckDoneGoal3");
            BoosterController.Instance.RemoveUndoData(this);
            // Debug.LogError("CheckDoneGoal4");
            Dispose();
            // Debug.LogError("CheckDoneGoal5");
            DOVirtual.DelayedCall(0.32f, () =>
            {
                // Debug.LogError("CheckDoneGoal8");
                LevelManager.Instance.ReInitGoal(index, pos);
            });
            // Debug.LogError("CheckDoneGoal6");
            // EffectManager.Instance.PlayFillEffect(index, colorRenderer.material.GetColor(_baseColor));
            // Debug.LogError("CheckDoneGoal7");
            //  VibrationUtil.Vibrate(50);
        }
    }

    public void PlayCloseEffect()
    {
        UIManager.Instance.UpdateLevelProcess();
        for (int i = myNutsList.Count - 1; i >= 0; i--)
        {
            myNutsList[i].GetComponent<Nut>().ReturnToPool();
        }
        if (LevelManager.Instance.currentGoal.Contains(this))
            LevelManager.Instance.currentGoal.Remove(this);
        colorRenderer.materials[0].DOColor(Factory.Instance.GetColorNut(color), _baseColor, 0).SetEase(Ease.InQuad);
        // colorRenderer.materials[0].DOFloat(0.95f, _metalic, 0).SetEase(Ease.InQuad);
        // colorRenderer.materials[0].DOFloat(0.2f, _smoothness, 0).SetEase(Ease.InQuad);
        Factory.Instance.ReturnGoalScrewToPool(type, gameObject);
    }
    public void OpenFillBoosterMode()
    {
        arrowParent.gameObject.SetActive(true);
        float ratio = GameConfig.Instance.RatioScaleScreen > 1 ? GameConfig.Instance.RatioScaleScreen * 2 - 1 : 1;
        arrowParent.GetChild(1).localPosition = new Vector3(0, -4 * ratio, 0);
        arrowParent.localPosition = Vector3.zero;
        arrowTween = arrowParent.DOLocalMoveY(0.5f * ratio, 0.5f)
                        .SetEase(Ease.OutSine)
                        .SetLoops(-1, LoopType.Yoyo);
    }
    public void CloseFillBoosterMode()
    {
        arrowParent.gameObject.SetActive(false);
        arrowTween?.Kill();
    }
    public void Dispose()
    {
        AudioManager.instance.PlaySFX(SoundType.Holder_Close);
        animator.enabled = true;
        animator.SetTrigger(_closeTrigger);

        //AnimClose();
    }

    public void KillAllTween()
    {
        DOTween.Kill(gameObject);
    }
}
