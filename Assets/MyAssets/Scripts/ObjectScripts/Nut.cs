using DG.Tweening;

using UnityEngine;

using System;

using TMPro;
//using GogoGaga.OptimizedRopesAndCables;



public class Nut : MonoBehaviour
{
    public NutData data;
    public Screw screwParent;
    public Animation spinAnim;
    public bool isMoving = false;
    public float speedRotate;
    public float speed = 35;
    public int iceStep = 0;
    public int bombTurn = 0;
    public Transform visual;
    public TrailRenderer trailRenderer;
    [SerializeField] MeshRenderer colorRenderer;
    [SerializeField] GameObject shadow2;
    [SerializeField] ParticleSystem completeParticle;
    [SerializeField] ParticleSystem iceBreakParticle;
    [SerializeField] ParticleSystem revealParticle;

    [SerializeField] GameObject bombComponent;
    [SerializeField] TextMeshPro bombCount;
    // [SerializeField] Rope firtAttachment;
    [SerializeField] Material colorMaterial;
    [SerializeField] Material hiddenMaterial;
    public Nut connectedNut;
    public Transform secondAttachment;
    Tween noSpaceTween;
    Tween rotateTween;
    Tween moveTween;
    private Vector3 pos, noSpacePos, initAngle;
    public bool isExplore = false;
    private int _baseColor = Shader.PropertyToID("_BaseColor");
    private int _smoothness = Shader.PropertyToID("_Smoothness");
    private int _metalic = Shader.PropertyToID("_Metallic");

    public void Init(NutData nutData)
    {
        data = new NutData()
        {
            nutType = nutData.nutType,
            color = nutData.color,
            bombCount = nutData.bombCount,
            isIce = nutData.isIce,
            isHidden = nutData.isHidden
        };
        noSpaceTween?.Kill();
        rotateTween?.Kill();
        moveTween?.Kill();
        isMoving = false;
        connectedNut = null;
        //    firtAttachment.gameObject.SetActive(false);
        secondAttachment.gameObject.SetActive(false);
        //  firtAttachment.GetComponent<LineRenderer>().positionCount = 0;
        isExplore = false;
        visual.GetComponent<BoxCollider>().enabled = true;
        SetSpecialComponent();
        if (data.isHidden)
            GetComponentInChildren<MeshRenderer>().material = hiddenMaterial;
        else
        {
            //GetComponentInChildren<MeshRenderer>().material = colorMaterial;
            //InitColor(data.color);
            GetComponentInChildren<MeshRenderer>().material = Factory.Instance.GetNutMaterial(data.color);
        }
        Color trailColor = Factory.Instance.GetGradientColorByType(data.color);
        if (data.color != ColorType.Blue && data.color != ColorType.Blue_2)

            trailColor.a = 1;
        trailRenderer.startColor = trailColor;
        if (data.nutType == NutType.Normal)
            initAngle = new Vector3(0, -180, 0);
        else if (data.nutType == NutType.Square)
            initAngle = Vector3.zero;
        else
            initAngle = new Vector3(0, 75, 0);
    }
    public void AssignPos(Vector3 pos)
    {
        this.pos = pos;
        noSpacePos = new Vector3(pos.x, pos.y + 1f, pos.z);
    }
    private void InitColor(ColorType color)
    {
        //colorRenderer.materials[0] = Factory.Instance.GetNutMaterial(color);
        colorRenderer.materials[0].DOFloat(Factory.Instance.GetMetalicNut(color), _metalic, 0f);
        colorRenderer.materials[0].DOFloat(Factory.Instance.GetSmoothnessNut(color), _smoothness, 0f);
        colorRenderer.materials[0].DOColor(Factory.Instance.GetColorNut(color), _baseColor, 0f);

    }
    private void SetSpecialComponent()
    {
        iceStep = data.isIce ? 3 : 0;
        //   SetIceVFX();

        InitBombComponent();
    }
    public void ChangeSpecialComponents()
    {
        BreakIce();
        UpdateBombCount();
    }
    public void ReturnToPool()
    {
        if (isExplore)
            ResetStateAfterExplore();
        Factory.Instance.ReturnNutToPool(gameObject, data.nutType);
    }

    #region Ice
    public void BreakIce()
    {
        if (iceStep > 0)
        {
            iceStep--;
            //   SetIceVFX();
            AudioManager.instance.PlaySFX(SoundType.Ice_Break);
            PlayIceBreakParticle();
            if (iceStep <= 0)
            {
                iceStep = 0;
                data.isIce = false;
            }
        }
    }
    public void ClearIce()
    {
        if (iceStep > 0)
        {
            iceStep = 0;
            data.isIce = false;
            //   SetIceVFX();
        }
    }
    //public void SetIceVFX()
    //{
    //    if (iceStep == 0)
    //    {
    //        iceVFX.SetActive(false);
    //    }
    //    else if (iceStep == 1)
    //    {
    //        iceVFX.SetActive(true);
    //        iceVFX.transform.GetChild(0).gameObject.SetActive(false);
    //        iceVFX.transform.GetChild(1).gameObject.SetActive(false);
    //    }
    //    else if (iceStep == 2)
    //    {
    //        iceVFX.SetActive(true);
    //        iceVFX.transform.GetChild(0).gameObject.SetActive(false);
    //        iceVFX.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //    else if (iceStep == 3)
    //    {
    //        iceVFX.SetActive(true);
    //        iceVFX.transform.GetChild(0).gameObject.SetActive(true);
    //        iceVFX.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //}
    public void PlayIceBreakParticle()
    {
        iceBreakParticle.Play();
    }
    #endregion

    #region 

    public void InitBombComponent()
    {
        bombComponent.SetActive(data.bombCount > 0);
        if (data.bombCount > 0)
            bombComponent.transform.GetChild(1).GetChild(1).GetComponent<ParticleSystem>().Play();
        bombCount.text = data.bombCount.ToString();
        bombCount.color = data.bombCount < 3 ? Color.red : Color.white;
        // Store the tween for bombCount animation


        Tween bombCountTween = bombCount.transform.DOScale(1.2f, 0.3f)
              .SetEase(Ease.Linear)
              .SetLoops(-1, LoopType.Yoyo);




        if (data.bombCount < 3)
            //   bombCount.GetComponent<DOTweenAnimation>().DOPlay();
            bombCountTween.Play();
        else
            //    bombCount.GetComponent<DOTweenAnimation>().DOPause();
            bombCountTween.Pause();
        //  bombCount.GetComponent<DOTweenAnimation>().DOPause();



    }
    public void UpdateBombCount()
    {
        if (data.bombCount > 0 && !isMoving)
        {
            data.bombCount--;
            bombCount.text = data.bombCount.ToString();
            bombCount.color = data.bombCount < 6 ? Color.red : Color.white;



            Tween bombCountTween = bombCount.transform.DOScale(1.2f, 0.3f)
          .SetEase(Ease.Linear)
          .SetLoops(-1, LoopType.Yoyo);

            if (data.bombCount < 6)
                bombCountTween.Play();
            else
                bombCountTween.Pause();

            if (data.bombCount <= 0)
            {
                PlayBombExploreVFX();
                GameManager.Instance.SetLose();
                //  HandleFirebase.Instance.LogEventWithString(HandleFirebase.Lose_By_end_level, "BombExplore");
            }
            else if (data.bombCount < 6)
            {
                bombCount.color = Color.red;
            }
        }
    }
    public void DefuseBomb()
    {
        if (data.bombCount > 0)
        {
            data.bombCount = 0;
            bombComponent.SetActive(false);
            AudioManager.instance.PlaySFX(SoundType.Bomb_Defuse);
        }
    }
    public void ResetStateAfterExplore()
    {
        isExplore = false;
        shadow2.SetActive(true);
        trailRenderer.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
        transform.localPosition = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
    }
    public void PlayBombExploreVFX()
    {
        bombComponent.SetActive(false);
        LevelManager.Instance.ActiveGroundCollider();
        EffectManager.Instance.PlayBombExploreEffect(transform.position);
        VibrationUtil.Vibrate(100);
        Physics.gravity = new Vector3(0, -40, 0);
        int layerMask = LayerMask.GetMask("Default");
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(1, 5, 1), Quaternion.identity, layerMask);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.GetComponentInParent<Nut>() && hit.gameObject.GetComponentInParent<Nut>().isMoving == false)
            {
                Nut nut = hit.gameObject.GetComponentInParent<Nut>();
                nut.shadow2.SetActive(false);
                nut.trailRenderer.enabled = false;
                nut.isExplore = true;

                Rigidbody rb = nut.gameObject.AddComponent<Rigidbody>();

                rb.mass = 0.5f;
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.AddExplosionForce(700, transform.position + new Vector3(1, 0, 1), 20, 5);
            }
        }
    }
    #endregion

    #region Rope

    public void SetConnectRope(Nut nut)
    {
        //firtAttachment.gameObject.SetActive(true);
        //firtAttachment.SetStartPoint(firtAttachment.transform);
        nut.secondAttachment.gameObject.SetActive(true);
        //firtAttachment.SetEndPoint(nut.secondAttachment);
        //firtAttachment.ropeLength = Vector3.Distance(firtAttachment.transform.position, nut.secondAttachment.transform.position) * 1.3f;
        connectedNut = nut;
        nut.connectedNut = this;
    }
    public void AdjustRopeLength()
    {
        //if ((transform.localPosition.y == 0 || connectedNut.transform.localPosition.y == 0) && firtAttachment.ropeLength > Vector3.Distance(firtAttachment.transform.position, connectedNut.secondAttachment.transform.position) * 1.05f)
        //{
        // //   DOTween.To(() => firtAttachment.ropeLength, x => firtAttachment.ropeLength = x, Vector3.Distance(firtAttachment.transform.position, connectedNut.secondAttachment.transform.position) * 1.05f, 0.1f);
        //}
    }
    public void CutRope()
    {
        if (connectedNut == null) return;

        if (transform.position.x < connectedNut.transform.position.x)
        {
            //  firtAttachment.gameObject.SetActive(false);
            connectedNut.secondAttachment.gameObject.SetActive(false);
        }
        else
        {
            secondAttachment.gameObject.SetActive(false);
            //  connectedNut.firtAttachment.gameObject.SetActive(false);
        }
        // Vector3 posVFX = (firtAttachment.transform.position + connectedNut.secondAttachment.transform.position) / 2;
        //    PlayCutRopeParticle(posVFX);
        AudioManager.instance.PlaySFX(SoundType.Rope_Cut);
        VibrationUtil.Vibrate(50);
        connectedNut.connectedNut = null;
        connectedNut = null;
    }

    public void PlayCutRopeParticle(Vector3 pos)
    {
        EffectManager.Instance.PlayCutRopeVfx(pos);
    }
    #endregion

    public void MoveToGoalScrew(GoalScrew goal, Screw screw, int indexInList, int indexMove, bool isBooster = false)
    {
        noSpaceTween?.Kill();
        if (data.isHidden)
            ShowHidden();
        isMoving = true;
        DefuseBomb();
        ClearIce();
        CutRope();
        goal.AddNut(this);
        moveTween?.Kill();
        float TimeA = 0.35f * 33 / LevelManager.Instance.speed;
        RotateOut(TimeA);
        transform.DOMove(screw.gate.position, TimeA).SetEase(LevelManager.Instance.easeStart).OnComplete(() =>
        {
            transform.parent = goal.itemHolder;
            float time2 = Vector3.Distance(screw.gate.position, goal.gate.position) / (LevelManager.Instance.speed * 1.1f);
            if (time2 < 0.25f) time2 = 0.25f;
            else if (time2 > 0.4f) time2 = 0.4f;
            transform.DOLocalMove(goal.gate.localPosition, time2).SetEase(LevelManager.Instance.easeMid).OnComplete(() =>
            {
                goal.SortNutPos(this, indexInList, isBooster);
            });
        });
    }

    public void UndoMove(Screw previousScrew, GoalScrew goal)
    {
        isMoving = true;
        moveTween?.Kill();
        previousScrew.AddNut(this);
        goal.RemoveNut(this);
        int index = previousScrew.myNutsList.Count - 1;
        //float time = 0.2f + (4 - goal.myNutsList.Count) * 0.05f;
        float timeB = 0.35f * 33 / LevelManager.Instance.speed;

        RotateOut(timeB);
        transform.DOMove(goal.gate.position + new Vector3(0, 0.5f, 0), timeB).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            goal.amountSortPre--;
            if (goal.amountSortPre == 0)
            {
                goal.color = goal.initColor;
                goal.gradientColor = goal.initColor;
                goal.ChangeColorMaterial();
                LevelManager.Instance.ChangeGoalGradientColor(goal.index, goal.color);
            }
            //float time = Vector3.Distance(previousScrew.gate.position, goal.gate.position) / speed;
            float timeC = Vector3.Distance(previousScrew.gate.position, goal.gate.position) / (LevelManager.Instance.speed * 1.1f);
            if (timeC < 0.25f) timeC = 0.25f;
            else if (timeC > 0.4f) timeC = 0.4f;
            AudioManager.instance.PlaySFX(SoundType.Undo_Move);
            transform.DOLocalMove(previousScrew.gate.localPosition - new Vector3(0, 0.75f, 0), timeC).SetEase(Ease.Linear).OnComplete(() =>
            {
                RotateIn();
                previousScrew.SortNutPos(this, index);
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    EffectManager.Instance.PlayNutSpecialEffect(transform.position);
                    visual.GetComponent<BoxCollider>().enabled = true;
                });
            });

        });
    }
    public void MoveWhenConnectRope()
    {
        if (!transform.parent.parent.parent.GetComponent<Screw>()) return;
        GoalScrew goal = LevelManager.Instance.GetGoalScrewByColor(data.color, data.nutType, true);
        goal.color = data.color;
        DOVirtual.DelayedCall(0.05f, () =>
        {
            transform.parent.parent.parent.GetComponent<Screw>().MoveConnectRopeNutToHolder();
        });
    }
    public void SetSpinAnim(bool isOn, string animationName = "")
    {
        if (isOn)
            spinAnim.Play();
        else
        {
            spinAnim.Stop();
            visual.eulerAngles = initAngle;
        }
    }
    public bool CheckCanMove()
    {
        if (!transform.parent.parent.parent.GetComponent<Screw>()) return false;

        if (LevelManager.Instance.GetGoalScrewByColor(data.color, data.nutType, true) == null) return false;

        return (transform.parent.GetChild(transform.parent.childCount - 1) == transform) && !data.isIce;

    }
    public void NoSpaceMove()
    {
        moveTween?.Kill();
        moveTween = TweenNoSpace();
    }
    public void FrozenMove()
    {
        AudioManager.instance.PlaySFX(SoundType.Rope_Reject);
        EffectManager.Instance.PlayEffectText("FROZEN", transform.position, true);
        //moveTween?.Kill();
        //moveTween = TweenFrozen();
    }
    public void RopeMove()
    {
        moveTween?.Kill();
        moveTween = TweenRope();
    }
    public void RotateOut(float time = 0.3f)
    {
        rotateTween?.Kill();
        rotateTween = TweenRotateOut(time);
    }
    public void RotateIn()
    {
        rotateTween?.Kill();
        rotateTween = TweenRotateIn();
    }
    public void SortPos(Vector3 pos, Action action)
    {
        moveTween?.Kill();
        moveTween = TweenSortPos(pos, action);
    }
    public Tween TweenRotateOut(float time = 0.3f)
    {
        if (LevelManager.Instance.hand.gameObject.activeSelf)
        {
            LevelManager.Instance.hand.gameObject.SetActive(false);
            LevelManager.Instance.isStartGame = true;
        }
        AudioManager.instance.PlaySFX(SoundType.Nut_Out_Of_Screw);
        float yRotate = visual.eulerAngles.y;
        return visual.DOLocalRotate(new Vector3(0, yRotate + 360, 0), time, RotateMode.FastBeyond360)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => visual.rotation = Quaternion.Euler(new Vector3(0, yRotate, 0)));
    }
    public Tween TweenRotateIn()
    {
        AudioManager.instance.PlaySFX(SoundType.Nut_Go_To_Screw);
        float yRotate = visual.eulerAngles.y;
        return visual.DOLocalRotate(new Vector3(0, yRotate - 360, 0), 0.4f, RotateMode.FastBeyond360)
                 .SetEase(Ease.InQuad)
                 .OnComplete(() => visual.rotation = Quaternion.Euler(new Vector3(0, yRotate, 0)));
    }
    public Tween TweenSortPos(Vector3 pos, Action action)
    {
        RotateIn();
        return transform.DOLocalMove(pos, 0.4f * 33 / LevelManager.Instance.speed).SetEase(LevelManager.Instance.easeEnd).OnComplete(() =>
        {
            AssignPos(pos);
            if (connectedNut != null)
                AdjustRopeLength();
            isMoving = false;
            if (action != null)
                action();
            if (LevelManager.Instance.isTutUndo)
            {
                GameManager.Instance.IsAction = false;
                LevelManager.Instance.isTutUndo = false;
                GameManager.Instance.disableUI = true;
                //TutorialController.Instance.OpenTutorial();
            }
        });
    }
    public Tween TweenNoSpace()
    {
        AudioManager.instance.PlaySFX(SoundType.Rope_Reject);
        EffectManager.Instance.PlayEffectText("NO SPACE", transform.position, true);
        SetSpinAnim(true, "Spin");
        return transform.DOLocalMove(noSpacePos, 0.13f).SetEase(Ease.Linear).OnComplete(() =>
        {
            noSpaceTween = transform.DOLocalMove(pos, 0.13f).SetEase(Ease.Linear).OnComplete(() =>
            {
                SetSpinAnim(false);
            });

        });
    }
    public Tween TweenFrozen()
    {
        AudioManager.instance.PlaySFX(SoundType.Rope_Reject);
        EffectManager.Instance.PlayEffectText("FROZEN", transform.position, true);
        return transform.DOLocalMove(noSpacePos, 0.13f).SetEase(Ease.Linear).OnComplete(() =>
        {
            noSpaceTween = transform.DOLocalMove(pos, 0.13f).SetEase(Ease.Linear);
        });
    }
    public Tween TweenRope()
    {
        AudioManager.instance.PlaySFX(SoundType.Rope_Reject);
        EffectManager.Instance.PlayEffectText("STUCK", transform.position, true);
        RotateOut();
        return transform.DOLocalMove(noSpacePos, 0.13f).SetEase(Ease.Linear).OnComplete(() =>
        {
            RotateIn();
            noSpaceTween = transform.DOLocalMove(pos, 0.13f).SetEase(Ease.Linear).OnComplete(() =>
            {
                rotateTween?.Kill();
                visual.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            });
        });
    }
    public void PlayCompleteParticle()
    {
        completeParticle.Play();
    }
    public void ShowHidden()
    {
        data.isHidden = false;
        GetComponentInChildren<MeshRenderer>().material = Factory.Instance.GetNutMaterial(data.color);
        //GetComponentInChildren<MeshRenderer>().material = colorMaterial;
        //InitColor(data.color);
        revealParticle.Play();
        AudioManager.instance.PlaySFX(SoundType.Show_Hidden);
    }
}
public enum ColorType
{
    None = 0,
    Orange = 1,
    Green = 2,
    Blue = 3,
    Violet = 4,
    Red = 5,
    Grey = 6,
    Pink = 7,
    Cyan = 8,
    Yellow = 9,
    Pink_2 = 10,
    Blue_2 = 11,
}