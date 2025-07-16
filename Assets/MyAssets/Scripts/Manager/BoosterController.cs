using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Do;
using UnityEngine.Device;
using UnityEngine.UI;
using DG.Tweening;


public class BoosterController : Singleton<BoosterController>
{
   // [SerializeField] List<Transform> boosters = new List<Transform>();
    [SerializeField] List<int> levelUnlocks = new List<int>();
    [SerializeField] List<int> boosterPrices = new List<int>();
    [SerializeField] Image darkFilter;
    [SerializeField] List<Sprite> numLayoutSprites = new List<Sprite>();
    [SerializeField] List<Image> numLayouts = new List<Image>();
    public bool isUsingBooster = false;
    public bool isFreeBoooster = false;
    public bool isTutBooster = false;
    Tween filterTween, textFilterTween;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #region Call Func
    public void InitBooster()
    {
        isFillingHolder = false;
        foreach (var type in Enum.GetValues(typeof(BoosterType)))
        {
            BoosterType booster = (BoosterType)type;
            SetStatusBooster(booster);
        }

        if (levelUnlocks.Contains(GameUtils.Level))
        {
            GameManager.Instance.disableUI = true;
            GameManager.Instance.IsAction = true;
            UIManager.Instance.OpenBoosterUnlockPopUp((BoosterType)levelUnlocks.IndexOf(GameUtils.Level));
        }

    }
    public void SetStatusBooster(BoosterType booster)
    {
        int key = (int)booster;
        //boosters[key].GetChild(2).gameObject.SetActive(false);
        //boosters[key].GetComponent<DOTweenAnimation>().DOPause();

        if (GameUtils.Level < levelUnlocks[key])
        {
            //boosters[key].GetChild(1).gameObject.SetActive(true);
            //boosters[key].GetChild(4).gameObject.SetActive(false);
            //boosters[key].GetChild(6).gameObject.SetActive(false);
            //if (GetBoosterCount(booster) > 0)
            //{
            //    numLayouts[key].sprite = numLayoutSprites[0];
            //    boosters[key].GetChild(3).gameObject.SetActive(true);
            //    boosters[key].GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetBoosterCount(booster).ToString();
            //}
            //else
            //{
            //    boosters[key].GetChild(3).gameObject.SetActive(false);
            //}
        }
        else
        {
            //boosters[key].GetChild(3).gameObject.SetActive(true);
            //boosters[key].GetChild(1).gameObject.SetActive(false);
            //boosters[key].GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetBoosterCount(booster).ToString();
            //if (GetBoosterCount(booster) > 0)
            //{
            //    numLayouts[key].sprite = numLayoutSprites[0];
            //    boosters[key].GetChild(4).gameObject.SetActive(false);
            //    boosters[key].GetChild(6).gameObject.SetActive(false);
            //}
            //else
            //{
            //    numLayouts[key].gameObject.SetActive(false);
            //    if (GameUtils.Coin >= boosterPrices[key])
            //    {
            //        boosters[key].GetChild(4).gameObject.SetActive(true);
            //        boosters[key].GetChild(6).gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        boosters[key].GetChild(4).gameObject.SetActive(false);
            //        boosters[key].GetChild(6).gameObject.SetActive(true);
            //    }

            //}
        }
    }
    public void CheckBoosterInActive(BoosterType booster)
    {
        //switch (booster)
        //{
        //    case BoosterType.Undo:
        //        boosters[(int)booster].GetChild(1).gameObject.SetActive(undoDatas.Count <= 0);
        //        return;
        //    case BoosterType.Fill:
        //        return;
        //    case BoosterType.Add_Holder:
        //        boosters[(int)booster].GetChild(1).gameObject.SetActive(LevelManager.Instance.currentGoal.Count >= 4);
        //        return;
        //    default:
        //        return;
        //}
    }
    public void OnClickBooster(int index)
    {
        if ((GameManager.Instance.CheckCanAction() && !isFillingHolder) || isTutBooster)
            Call_Booster((BoosterType)index);
    }
    public void Call_Booster(BoosterType booster)
    {
        Action Call;
        switch (booster)
        {
            case BoosterType.Undo:
                Call = Undo;
                break;
            case BoosterType.Fill:
                Call = SelectHolderToFill;
                break;
            case BoosterType.Add_Holder:
                Call = Add_Holder;
                break;
            default:
                Call = null;
                break;
        }
        if (GameUtils.Level < levelUnlocks[(int)booster])
            return;
        if (!LevelManager.Instance.CanUseBooster())
        {
            if (booster == BoosterType.Undo)
                return;
            else
            {
             //   EffectManager.Instance.PlayEffectText("NOT READY", boosters[(int)booster].position);
                return;
            }
        }

        if (isTutBooster)
        {
           // TutorialController.Instance.CloseTutorial();
            isFreeBoooster = true;
            Call();
        }
        else
        {
            if (GetBoosterCount(booster) > 0)
            {
                Call();
            }
            else
            {
                if (GameUtils.Coin >= boosterPrices[(int)booster])
                {
                    Call();
                }
                else
                {
                    UIManager.Instance.OpenPopUpBuyBooster(booster);
                    //EffectManager.Instance.PlayEffectText("OUT OF COIN", boosters[(int)booster].position);
                }
            }
        }
    }

    public int GetBoosterCount(BoosterType type)
    {
        switch (type)
        {
            case BoosterType.Undo:
                return GameUtils.Undo_Booster;
            case BoosterType.Fill:
                return GameUtils.Fill_Hol_Booster;
            case BoosterType.Add_Holder:
                return GameUtils.Add_Hol_Booster;
            default:
                return 0;
        }
    }
    public void SetBoosterCount(BoosterType type, int value)
    {
        switch (type)
        {
            case BoosterType.Undo:
                GameUtils.Undo_Booster = value;
                break;
            case BoosterType.Fill:
                GameUtils.Fill_Hol_Booster = value;
                break;
            case BoosterType.Add_Holder:
                GameUtils.Add_Hol_Booster = value;
                break;
            default:
                break;
        }
        //if (value > 0)
        //    boosters[(int)type].GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetBoosterCount(type).ToString();
        //else
        //{
        //    boosters[(int)type].GetChild(3).gameObject.SetActive(false);
        //    if (GameUtils.Coin >= boosterPrices[(int)type])
        //    {
        //        boosters[(int)type].GetChild(4).gameObject.SetActive(true);
        //        boosters[(int)type].GetChild(6).gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        boosters[(int)type].GetChild(4).gameObject.SetActive(false);
        //        boosters[(int)type].GetChild(6).gameObject.SetActive(true);
        //    }
        //}
    }

    public void SetBoosterInTut(int type, bool isEnable)
    {
        isTutBooster = isEnable;
        if (isEnable)
        {
            //boosters[type].GetComponent<DOTweenAnimation>().DOPlay();
            //boosters[type].GetComponent<Canvas>().sortingOrder = 300;
            //boosters[type].transform.GetChild(5).gameObject.SetActive(true);
            //boosters[type].GetChild(3).gameObject.SetActive(false);
            //boosters[type].GetChild(4).gameObject.SetActive(false);
            //boosters[type].GetChild(6).gameObject.SetActive(false);
            //if (GetBoosterCount((BoosterType) type) == 0)
            //    SetBoosterCount((BoosterType)type, 1);
        }
        else
        {
            //boosters[type].GetComponent<DOTweenAnimation>().DOPause();
            //boosters[type].GetComponent<Canvas>().sortingOrder = 30;
            //boosters[type].transform.GetChild(5).gameObject.SetActive(false);
            SetStatusBooster((BoosterType)type);
        }
    }
    #endregion

    #region Undo
    public List<UndoData> undoDatas = new List<UndoData>();

    public void Undo()
    {
        if (undoDatas.Count <= 0)
        {
            //EffectManager.Instance.PlayEffectText("NOT READY", new Vector3(-300, -690, 300), true);
          //  EffectManager.Instance.PlayEffectText("NOT READY", boosters[0].position);
            return;
        }
        if (LevelManager.Instance.currentGoal.Find(goal => goal.amountSortPre > 0) == null)
        {
            undoDatas.Clear();
         //   EffectManager.Instance.PlayEffectText("NOT READY", boosters[0].position);
            return;
        }
        if (!isFreeBoooster)
        {
            if (GetBoosterCount(BoosterType.Undo) > 0)
            {
                SetBoosterCount(BoosterType.Undo, GetBoosterCount(BoosterType.Undo) - 1);
               
            }
            else
            {
                GameUtils.Coin -= boosterPrices[(int)BoosterType.Undo];
                SetStatusBooster(BoosterType.Undo);
                SetStatusBooster(BoosterType.Fill);
                SetStatusBooster(BoosterType.Add_Holder);
                UIManager.Instance.UpdateCoin();
            
            }
        }
        else
            isFreeBoooster = false;
        LevelManager.Instance.totalUndo++;
        LevelManager.Instance.AddLevelSequence("B");
        UndoData undoData = undoDatas[undoDatas.Count - 1];
        undoDatas.Remove(undoData);
        CheckBoosterInActive(BoosterType.Undo);

        StartCoroutine(SequenceUndo(undoData));
    }

    IEnumerator SequenceUndo(UndoData undoData)
    {
        undoData.undoScrew.state = ScrewState.ACTION;
        undoData.undoScrew.amountSortPre += undoData.undoNuts.Count;

        for (int i = undoData.undoNuts.Count - 1; i >= 0; i--)
        {
            undoData.undoNuts[i].UndoMove(undoData.undoScrew, undoData.undoHolder);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void AddUndoData(UndoData undoData)
    {
        if (!undoDatas.Contains(undoData))
            undoDatas.Add(undoData);
        CheckBoosterInActive(BoosterType.Undo);
    }
    public void RemoveUndoData(GoalScrew goal)
    {
        foreach (var nut in goal.myNutsList)
        {
            UndoData undoData = undoDatas.Find(data => data.undoNuts.Contains(nut));
            if (undoData != null)
                undoDatas.Remove(undoData);
        }

    }
    #endregion

    #region Fill

    public bool isFillingHolder = false;
    public void Fill_Holder(GoalScrew goal)
    {
        LevelManager.Instance.totalFill++;
        LevelManager.Instance.AddLevelSequence("B");
        ColorType color = LevelManager.Instance.GetValidNutToFill(goal.type);
        if (goal.color != ColorType.None)
        {
            color = goal.color;
        }
        List<Screw> screws = LevelManager.Instance.GetListScrewByColorAndType(color, goal.type);
        int num = goal.size - goal.amountSortPre;
        goal.amountSortPre = goal.size;
        StartCoroutine(Fx_Fill_Holder(goal, screws, num, color, goal.type));

    }

    IEnumerator Fx_Fill_Holder(GoalScrew goal, List<Screw> screws, int num, ColorType color, NutType type)
    {
        GameManager.Instance.IsAction = true;
        Dictionary<Nut, Screw> useDictionary = new Dictionary<Nut, Screw>();
        int count = 0;
        while (count < num)
        {
            count++;
            var screw = screws[0];
            var moveNut = screw.myNutsList.Find(nut => nut.data.color == color && nut.data.nutType == type);
            screw.amountSortPre--;
            moveNut.visual.GetComponent<BoxCollider>().enabled = false;
            useDictionary.Add(moveNut, screw);
            screw.RemoveNut(moveNut);
            if (screw.myNutsList.Find(nut => nut.data.color == color && nut.data.nutType == type) == null)
                screws.RemoveAt(0);
        }
        LevelManager.Instance.CheckBreakGlass();

        yield return new WaitForSeconds(0.085f);

        foreach (var variable in useDictionary)
        {
            variable.Key.MoveToGoalScrew(goal, variable.Value, goal.myNutsList.Count, 1, true);
            goal.color = variable.Key.data.color;
            LevelManager.Instance.BreakIceNuts();
            yield return new WaitForSeconds(0.3f);
            variable.Value.state = variable.Value.amountSortPre == 0 ? ScrewState.EMPTY : ScrewState.IDLE;
        }


        LevelManager.Instance.CheckLoseLevel();

        yield return new WaitForSeconds(0.3f);

        GameManager.Instance.IsAction = false;

        foreach (var screw in useDictionary.Values)
        {
            screw.SortAllNutPos();
        }
    }
    public void CheckClickFillHolder()
    {
        if (!isFillingHolder) return;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 touchPosition;

            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
            }
            else
            {
                touchPosition = Input.mousePosition;
            }

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Holder"))
            {
                UnSelectHolderToFill();
                if (!isFreeBoooster)
                {
                    if (GetBoosterCount(BoosterType.Fill) > 0)
                    {
                        SetBoosterCount(BoosterType.Fill, GetBoosterCount(BoosterType.Fill) - 1);
                   
                    }
                    else
                    {
                        GameUtils.Coin -= boosterPrices[(int)BoosterType.Fill];
                        UIManager.Instance.UpdateCoin();
                        SetStatusBooster(BoosterType.Undo);
                        SetStatusBooster(BoosterType.Fill);
                        SetStatusBooster(BoosterType.Add_Holder);
                       
                    }
                }
                else
                    isFreeBoooster = false;
                Fill_Holder(hit.collider.gameObject.GetComponent<GoalScrew>());
            }
            else
                UnSelectHolderToFill();
        }
    }

    //public void ClickHolderToFill(GoalScrew holder)
    //{
    //    UnSelectHolderToFill();
    //    if (!isTutBooster)
    //    {
    //        if (GetBoosterCount(BoosterType.Fill) > 0)
    //        {
    //            SetBoosterCount(BoosterType.Fill, GetBoosterCount(BoosterType.Fill) - 1);
    //        }
    //        else
    //        {
    //            GameUtils.Coin -= boosterPrices[(int)BoosterType.Fill];
    //            UIManager.Instance.UpdateCoin();
    //        }
    //    }
    //    Fill_Holder(holder);
    //}

    public void SelectHolderToFill()
    {
        AudioManager.instance.PlaySFX(SoundType.Booster_Fill);
        LevelManager.Instance.EnableFillBoosterMode();
     //   boosters[1].GetChild(2).gameObject.SetActive(true);
        numLayouts[1].gameObject.SetActive(false);

        darkFilter.gameObject.SetActive(true);
        darkFilter.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GameConfig.Instance.RatioScaleScreen < 1 ? -40 : -100);
        darkFilter.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0f);
        filterTween?.Kill();
        textFilterTween?.Kill();
        darkFilter.transform.GetChild(0).localScale = Vector3.zero;
        filterTween = darkFilter.DOFade(250 / 255f, 0.3f);
        //textFilterTween = darkFilter.transform.GetChild(0).DOScale(1f, 0.7f).OnComplete(() =>
        //{
        //   darkFilter.transform.GetChild(0).GetComponent<DOTweenAnimation>().DORestart();
        //});
        isFillingHolder = true;
    }
    public void UnSelectHolderToFill()
    {
        LevelManager.Instance.DisableFillBoosterMode();
     //   boosters[1].GetChild(2).gameObject.SetActive(false);
        SetStatusBooster(BoosterType.Fill);
        filterTween?.Kill();
        textFilterTween?.Kill();
        filterTween = darkFilter.DOFade(0, 0.3f).OnComplete(() =>
        {
            isFillingHolder = false;
            darkFilter.gameObject.SetActive(false);
        });
      //  darkFilter.transform.GetChild(0).GetComponent<DOTweenAnimation>().DOPause();
        textFilterTween = darkFilter.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.3f);
    }

    #endregion

    #region Add

    public void Add_Holder()
    {
        if (LevelManager.Instance.currentGoal.Count >= 4 || !LevelManager.Instance.IsEnoughNutForFourthHolder())
        {
            //EffectManager.Instance.PlayEffectText("NOT READY", new Vector3(300, -690, 300), true);
         //   EffectManager.Instance.PlayEffectText("NOT READY", boosters[2].position);
            return;
        }
        Debug.Log("0 " + isTutBooster);
        if (!isFreeBoooster)
        {
            if (GetBoosterCount(BoosterType.Add_Holder) > 0)
            {
                SetBoosterCount(BoosterType.Add_Holder, GetBoosterCount(BoosterType.Add_Holder) - 1);
             
            }
            else
            {
                GameUtils.Coin -= boosterPrices[(int)BoosterType.Add_Holder];
                UIManager.Instance.UpdateCoin();
                SetStatusBooster(BoosterType.Undo);
                SetStatusBooster(BoosterType.Fill);
                SetStatusBooster(BoosterType.Add_Holder);
               
            }
        }
        else
            isFreeBoooster = false;
        LevelManager.Instance.totalExtraHolder++;
        LevelManager.Instance.AddLevelSequence("B");
        LevelManager.Instance.InitHolderByBooster();
        AudioManager.instance.PlaySFX(SoundType.Booster_Add_Holder);
        CheckBoosterInActive(BoosterType.Add_Holder);
    }
    public void Add_Holder_KeepPlaying()
    {
        if (LevelManager.Instance.currentGoal.Count >= 4)
        {
            return;
        }

        LevelManager.Instance.InitHolderByBooster();
        CheckBoosterInActive(BoosterType.Add_Holder);
    }


    #endregion


}
public class UndoData
{
    public List<Nut> undoNuts = new List<Nut>();
    public Screw undoScrew;
    public GoalScrew undoHolder;
}
public enum BoosterType
{
    Undo = 0,
    Fill = 1,
    Add_Holder = 2,
}

