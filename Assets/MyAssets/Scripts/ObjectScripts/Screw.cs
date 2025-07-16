using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using Unity.VisualScripting;
//using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Screw : MonoBehaviour
{
    [SerializeField] Transform itemHolder;
    [SerializeField] Animator towelAnimator;
    [SerializeField] List<Sprite> towelColorSprites = new List<Sprite>();
    [SerializeField] SpriteRenderer towelColorSR;
    public Transform gate;
    public int size;
    public float delta;
    public List<Nut> myNutsList = new List<Nut>();
    public ScrewState state;
    public int amountSortPre;
    public List<int> glassBlockIndex = new List<int>();
    public ColorType towelColor;
    Tween sortPosTween;
    Tween ropeTween;
    private static readonly WaitForSeconds waitBetweenNutMove = new WaitForSeconds(0.085f);
    private static readonly WaitForSeconds waitEndFX = new WaitForSeconds(0.3f);
    private static readonly WaitForSeconds waitInit = new WaitForSeconds(0.3f);
    private List<Nut> listSortChange = new List<Nut>();


    public void InitNutsInside(ScrewData data, bool isNextLevel = false)
    {
        myNutsList.Clear();
        amountSortPre = 0;
        glassBlockIndex.Clear();
        for (int i = 0; i < data.nutDatas.Count; i++)
        {
            string[] values = data.nutDatas[i].Split('/');
            ColorType colorNut = (ColorType)(int.Parse(values[1]) > 12 ? int.Parse(values[1]) - 12 : int.Parse(values[1]));
            if (colorNut == ColorType.Pink_2)
                colorNut = ColorType.Pink;
            NutData nutData = new NutData()
            {
                nutType = (NutType) int.Parse(values[0]),
                isHidden = int.Parse(values[1]) > 12,
                color = (ColorType)(int.Parse(values[1]) > 12 ? int.Parse(values[1]) - 12 : int.Parse(values[1])),
                bombCount = int.Parse(values[2]),
                isIce = int.Parse(values[3]) > 0
            };
            Nut nut = Factory.Instance.GetNutByType(nutData, itemHolder);
            LevelManager.Instance.nutsInScene.Add(nut);
            nut.transform.localPosition = new Vector3(0, i * delta, 0);
            nut.AssignPos(new Vector3(0, i * delta, 0));
            nut.screwParent = this;
            if (isNextLevel)
                nut.trailRenderer.enabled = false;
            myNutsList.Add(nut);
            amountSortPre++;
        }
        state = ScrewState.IDLE;
    }
    private void AnimationInit()
    {
        gameObject.SetActive(false);
        for (int i = myNutsList.Count - 1; i >= 0; i--)
        {
            myNutsList[i].transform.localScale = Vector3.zero;
            myNutsList[i].transform.position = gate.position;
        }
        StartCoroutine(IEInit());
    }

    IEnumerator IEInit()
    {
        gameObject.SetActive(true);
        yield return waitInit;
        for (int i = 0; i < myNutsList.Count; i++)
        {
            yield return waitInit; 
            myNutsList[i].transform.DOScale(1, 0.2f).OnComplete(() =>
            {
                SortNutPos(myNutsList[i], i);
            });
        }
    }

    public void Dispose()
    {
        for (int i = myNutsList.Count - 1; i >= 0; i--)
        {
            myNutsList[i].GetComponent<Nut>().ReturnToPool();
        }
        Factory.Instance.ReturnScrewToPool(size, gameObject);
    }
    public void EnableNutTrail()
    {
        foreach (Nut nut in myNutsList)
        {
            nut.trailRenderer.enabled = true;
        }
    }
    //public void OnMouseDown()
    //{
    //    if (!GameManager.Instance.CheckCanAction() && !GameManager.Instance.IsTutShow)
    //    {
    //        return;
    //    }
    //    OnClick();
    //}
    public void OnClick()
    {
        LevelManager.Instance.AddLevelSequence("T");
        if (state == ScrewState.ACTION || state == ScrewState.EMPTY || (glassBlockIndex.Count > 0 && amountSortPre <= glassBlockIndex[0]) || towelColor != ColorType.None)
        {
            return;
        }

        VibrationUtil.Vibrate(20);
        Nut lastNut_ScrewChange = myNutsList[myNutsList.Count - 1];

        if (lastNut_ScrewChange.iceStep > 0)
        {
            lastNut_ScrewChange.FrozenMove();
            return;
        }

        GoalScrew goal = LevelManager.Instance.GetGoalScrewByColor(lastNut_ScrewChange.data.color, lastNut_ScrewChange.data.nutType);

        if (goal != null)
        {
            int amount = goal.size - goal.amountSortPre;
            listSortChange.Clear();
            listSortChange.Add(lastNut_ScrewChange);
            goal.color = lastNut_ScrewChange.data.color;
            goal.ropeCount++;
            if (lastNut_ScrewChange.connectedNut == null)
            {
                lastNut_ScrewChange.visual.GetComponent<BoxCollider>().enabled = false;
                lastNut_ScrewChange.isMoving = true;
                amount--;
            }
            else
            {
                if (!lastNut_ScrewChange.connectedNut.CheckCanMove())
                {
                    if (goal.amountSortPre == 0)
                        goal.color = goal.initColor;
                    lastNut_ScrewChange.RopeMove();
                    goal.ropeCount = 0;
                    return;
                }
                else
                {
                    lastNut_ScrewChange.isMoving = true;
                    if (lastNut_ScrewChange.connectedNut.data.color == lastNut_ScrewChange.data.color)
                    {
                        amount -= 2;
                        if (amount > 0)
                        {
                            lastNut_ScrewChange.connectedNut.MoveWhenConnectRope();
                            lastNut_ScrewChange.connectedNut.visual.GetComponent<BoxCollider>().enabled = false;
                            goal.ropeCount++;

                        }
                        else if (amount == 0)
                        {
                            lastNut_ScrewChange.connectedNut.MoveWhenConnectRope();
                            lastNut_ScrewChange.connectedNut.visual.GetComponent<BoxCollider>().enabled = false;
                            goal.ropeCount = 0;
                        }
                    }
                    else
                    {
                        amount--;
                        lastNut_ScrewChange.connectedNut.MoveWhenConnectRope();
                        lastNut_ScrewChange.connectedNut.visual.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
            if (myNutsList.Count > 1)
            {
                for (int i = myNutsList.Count - 2; i >= 0; i--)
                {
                    if (glassBlockIndex.Count > 0 && i < glassBlockIndex[0])
                        break;
                    Nut item = myNutsList[i];
                    if (amount > 0 && !item.data.isHidden && !item.data.isIce 
                        && 
                        item.data.color == lastNut_ScrewChange.data.color && item.data.nutType == lastNut_ScrewChange.data.nutType) 
                    {
                        goal.ropeCount++;
                        if (item.connectedNut == null)
                        {
                            item.visual.GetComponent<BoxCollider>().enabled = false;
                            listSortChange.Add(item);
                            item.isMoving = true;
                            amount--;
                        }
                        else
                        {
                            if (!item.connectedNut.CheckCanMove())
                            {
                                ropeTween?.Kill();
                                ropeTween = DOVirtual.DelayedCall(0.16f * (myNutsList.Count - 1 - i) * 33 / LevelManager.Instance.speed, () =>
                                {
                                    item.RopeMove();
                                });
                                break;
                            }
                            else
                            {
                                listSortChange.Add(item);
                                item.visual.GetComponent<BoxCollider>().enabled = false;
                                item.isMoving = true;
                                amount--;
                                if (amount > 0 && item.connectedNut.data.color == item.data.color)
                                    amount--;
                                item.connectedNut.MoveWhenConnectRope();
                                goal.ropeCount++;
                                item.connectedNut.visual.GetComponent<BoxCollider>().enabled = false;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                goal.ropeCount = 0;
            }
            MoveNutToGoalScrew(goal, listSortChange);

        }
        else
        {
            lastNut_ScrewChange.NoSpaceMove();
        }

    }

    public void MoveNutToGoalScrew(GoalScrew goal, List<Nut> listNuts)
    {
        //List<Nut> listUndo = new List<Nut>(listNuts);

        amountSortPre -= listNuts.Count;
        goal.amountSortPre += listNuts.Count;


        StopAllCoroutines();

        BoosterController.Instance.AddUndoData(new UndoData()
        {
            undoNuts = new List<Nut>(listNuts),
            undoScrew = this,
            undoHolder = goal,

        });

        LevelManager.Instance.ChangeSpecialNutComponentInLevel();
        LevelManager.Instance.CheckBreakGlass();
        StartCoroutine(FXRemoveSort(listNuts, goal));

        //if (listInfoUndos.Count > 0)
        //{
        //    GameController.instance.btnUndo.interactable = true;
        //}
    }

    public IEnumerator FXRemoveSort(List<Nut> listNutChange, GoalScrew goal)
    {

        state = ScrewState.ACTION;
        int countWhile = 0;
        int lengthListCheck = listNutChange.Count;

        while (countWhile < lengthListCheck)
        {
            if (countWhile >= 1)
            {
                yield return waitBetweenNutMove;
            }
            countWhile++;
            var moveNut = listNutChange[0];
            listNutChange.Remove(moveNut);
            RemoveNut(moveNut);

            moveNut.MoveToGoalScrew(goal, this, goal.myNutsList.Count, countWhile);

            yield return null;
        }

        if (amountSortPre > 0 && myNutsList[myNutsList.Count - 1].data.isHidden)
        {
            ColorType color = myNutsList[myNutsList.Count - 1].data.color;
            for (int i = myNutsList.Count - 1; i >= 0; i--)
            {
                if (myNutsList[i].data.isHidden && myNutsList[i].data.color == color)
                {
                    myNutsList[i].ShowHidden();
                }
                else
                    break;
            }
        }
        yield return waitEndFX;
        //SortAllNutPos();
        state = amountSortPre == 0 ? ScrewState.EMPTY : ScrewState.IDLE;

        LevelManager.Instance.CheckLoseLevel();
    }
    public void MoveConnectRopeNutToHolder()
    {
        Nut lastNut_ScrewChange = myNutsList[myNutsList.Count - 1];
        GoalScrew goal = LevelManager.Instance.GetGoalScrewByColor(lastNut_ScrewChange.data.color, lastNut_ScrewChange.data.nutType, true);
        lastNut_ScrewChange.isMoving = true;
        MoveNutToGoalScrew(goal, new List<Nut> { lastNut_ScrewChange });

    }

    public void SortNutPos(Nut nut, int index)
    {
        state = ScrewState.ACTION;
        nut.SortPos(new Vector3(0, index * delta, 0), null);
        sortPosTween?.Kill();
        sortPosTween = DOVirtual.DelayedCall(0.4f, () => 
        { 
            state = ScrewState.IDLE;
        });
    }
    public void SortAllNutPos()
    {
        if (myNutsList.Count > 0)
        {
            for (int i = 0; i < myNutsList.Count; i++)
            {
                SortNutPos(myNutsList[i].GetComponent<Nut>(), i);
                if (i == myNutsList.Count - 1 && myNutsList[i].GetComponent<Nut>().data.isHidden)
                    myNutsList[i].GetComponent<Nut>().ShowHidden();
            }
        }
    }
    public void AddNut(Nut nut)
    {
        nut.transform.parent = itemHolder;
        nut.transform.localScale = Vector3.one;
        if (!myNutsList.Contains(nut))
            myNutsList.Add(nut);
    }
    public void RemoveNut(Nut nut)
    {
        if (myNutsList.Contains(nut))
            myNutsList.Remove(nut);       
    }
    public void ChangeSpecialNutComponents()
    {
        foreach (var nut in myNutsList)
        {
            nut.ChangeSpecialComponents();
        }
    }
    public void BreakIceNuts()
    {
        foreach (var nut in myNutsList)
        {
            nut.BreakIce();
        }
    }
    public void InitTowel(int towelColor)
    {
        if (towelColor <= 0)
        {
            towelAnimator.gameObject.SetActive(false);
            this.towelColor = ColorType.None;
            //AnimationInit();
        }
        else
        {
            towelAnimator.gameObject.SetActive(true);
            towelColorSR.sprite = towelColorSprites[towelColor - 1];
            this.towelColor = (ColorType) towelColor;
            towelAnimator.SetTrigger("Root");
            towelAnimator.Rebind();
        }
    }
    public void CheckOpenTowel(ColorType color)
    {
        if (towelAnimator.gameObject.activeInHierarchy && color == towelColor)
        {
            OpenTowel();
        }
    }
    public void OpenTowel()
    {
        this.towelColor = ColorType.None;
        state = ScrewState.ACTION;
        towelAnimator.ResetTrigger("Complete");
        towelAnimator.CrossFade("Complete", 0f);
        towelAnimator.SetTrigger("Complete");
        DOVirtual.DelayedCall(1.6f, () => 
        { 
            state = ScrewState.IDLE;
            //StartCoroutine(FadeOutBox());
            //towelColorSR.DOFade(0, 0.3f);
            //towelAnimator.SetTrigger("Root");
            //towelAnimator.gameObject.SetActive(false);
            //Destroy(towelAnimator.gameObject);
        });
    }
    private IEnumerator FadeOutBox()
    {
        foreach (SkinnedMeshRenderer mesh in towelAnimator.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Material mat in mesh.materials)
            {
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_Surface", 1);
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                // mat.SetShaderPassEnabled("DepthOnly", false);
                // mat.SetShaderPassEnabled("SHADOWCASTER", true);

              //  mat.SetOverrideTag("RenderType", "Transparent");
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

                //mat.DOFade(0, 0.5f).OnComplete(() =>
                //{
                //    towelAnimator.gameObject.SetActive(false);

                //});

            }
        }

        float time = 0;
        float a = towelAnimator.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color.a;
        while (towelAnimator.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color.a > 0)
        {
            foreach (SkinnedMeshRenderer mesh in towelAnimator.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (Material mat in mesh.materials)
                {
                    if (mat.HasProperty("_BaseColor"))
                    {
                        mat.color = new Color(
                            mat.color.r,
                            mat.color.g,
                            mat.color.b,
                            Mathf.Lerp(a, 0, time * 12)
                        );
                    }
                }
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}

public enum ScrewState
{
    IDLE = 0,
    ACTION = 1,
    EMPTY = 2,
}
