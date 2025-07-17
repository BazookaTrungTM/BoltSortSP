using DG.Tweening;
using Do;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : Singleton<LevelManager>
{
    public float speed = 33;
    public Ease easeStart = Ease.OutQuad;
    public Ease easeMid = Ease.Linear;
    public Ease easeEnd = Ease.InQuad;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera uiCamera;
    [SerializeField] Transform bg;
    [SerializeField] Transform screwParent;
    [SerializeField] Transform fakeScrewParent;
    [SerializeField] Transform goalScrewParent;
    [SerializeField] Transform glassParent;
    [SerializeField] GameObject plane;

    public List<Screw> screwsInScene = new List<Screw>();
    public List<Screw> oldScrews = new List<Screw>();
    public List<Nut> nutsInScene = new List<Nut>();
    public List<Glass> glassInScene = new List<Glass>();
    public Queue<int> colorQueue = new Queue<int>();
    public List<GoalScrew> currentGoal = new List<GoalScrew>();
    public List<SpriteRenderer> goalGradients = new List<SpriteRenderer>();
    public int currentLevel;
    public float ratioRope = 5;
    public int keepPlayingTime = 0;
    public int totalLevelHolder = 0;
    public int totalUndo, totalFill, totalExtraHolder;
    public string actionSequence;
    Tween handTut6;
    private float ratioScale_2450 = 1.276f;
    private string[] urlLevels = new string[2] { "Level_Minh/", "Level_Huyen/" };
    Tween filterTween;
    public TextAsset jsonFileLuna;

    #region MonoBehaviour Func
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Initialize
    public void InitLevel(bool isReplay = false)
    {
        GameManager.Instance.ResetState();
        ClearLevel();

        //currentLevel = level;
        //int realLevel = currentLevel;
        //string url = urlLevels[typeLevel];
        //TextAsset jsonFile = Resources.Load<TextAsset>(url + realLevel.ToString());



        TextAsset jsonFile = jsonFileLuna;
        if (jsonFile == null)
        {
            // Debug.LogError("JSON does not exit!");
        }
        else
        {
            InitBG();
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonFile.text);
            InitScrews(levelData.screwList);
            InitAllGoalScrews(levelData.targetColorList, levelData.goalNum);
            InitRopesInLevel(levelData.ropeMap);
            InitGlassesInLevel(levelData.glassMap);
            UIManager.Instance.Init(levelData.targetColorList.Count, isReplay);
            // BoosterController.Instance.InitBooster();

            StopTutLevel1();

            if (GameUtils.Level == 1)
            {
                OpenTutLevel1();
            }

        }
    }
    public void NextLevel()
    {
        GameManager.Instance.ResetState();
        ClearLevelForNextLevel();
        screwParent.localPosition += new Vector3(20, 0, 0) / transform.localScale.x;
        //goalScrewParent.localPosition += new Vector3(20, 0, 0) / transform.localScale.x;
        goalScrewParent.SetParent(transform.parent);
        glassParent.localPosition += new Vector3(20, 0, 0) / transform.localScale.x;

        currentLevel = GameUtils.Level;
        int realLevel = GameUtils.Level > 200 ? 100 + (GameUtils.Level % 100) : currentLevel;
        string url = GameUtils.Level > 150 ? urlLevels[0] : urlLevels[GameUtils.Level_Type];
        TextAsset jsonFile = Resources.Load<TextAsset>(url + realLevel.ToString());
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonFile.text);
        if (jsonFile == null)
        {
            // Debug.LogError("JSON does not exit!");
        }
        else
        {
            //InitBG();
            InitScrews(levelData.screwList, true);
            InitAllGoalScrews(levelData.targetColorList, levelData.goalNum);
            InitRopesInLevel(levelData.ropeMap);
            InitGlassesInLevel(levelData.glassMap);
            StopTutLevel1();
        }
        GameManager.Instance.IsAction = true;

        //if ((GameUtils.Level > 10 && GameUtils.Level % 10 == 4) || GameUtils.Level % 10 == 9)
        //{
        //    UIManager.Instance.Init(levelData.targetColorList.Count);
        //    DOVirtual.DelayedCall(4, () =>
        //    {
        //        foreach (GoalScrew goalScrew in currentGoal)
        //        {
        //            goalScrew.AnimOpen();
        //        }
        //        transform.DOLocalMoveX(-20, .6f).SetRelative(true).SetEase(Ease.OutBack).OnComplete(() =>
        //        {
        //            GameManager.Instance.disableUI = false;
        //            transform.localPosition += new Vector3(20, 0, 0);
        //            screwParent.localPosition -= new Vector3(20, 0, 0) / transform.localScale.x;
        //            goalScrewParent.SetParent(transform);
        //            glassParent.localPosition -= new Vector3(20, 0, 0) / transform.localScale.x;
        //            BoosterController.Instance.InitBooster();
        //            ClearOldScrews();
        //            EnableNutTrailRenderer();
        //            GameManager.Instance.IsAction = false;
        //        });
        //    });
        //}
        //else 
        //{
        foreach (GoalScrew goalScrew in currentGoal)
        {
            goalScrew.AnimOpen();
        }
        transform.DOLocalMoveX(-20, .5f).SetRelative(true).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            GameManager.Instance.disableUI = false;
            transform.localPosition += new Vector3(20, 0, 0);
            screwParent.localPosition -= new Vector3(20, 0, 0) / transform.localScale.x;
            //goalScrewParent.localPosition -= new Vector3(20, 0, 0) / transform.localScale.x;
            goalScrewParent.SetParent(transform);
            glassParent.localPosition -= new Vector3(20, 0, 0) / transform.localScale.x;
            BoosterController.Instance.InitBooster();
            UIManager.Instance.Init(levelData.targetColorList.Count);
            ClearOldScrews();
            EnableNutTrailRenderer();
            GameManager.Instance.IsAction = false;
        });
        //}

    }

    public void ClearLevel()
    {
        ClearLevelObject();
        ClearLevelData();
    }
    public void ClearLevelForNextLevel()
    {
        ClearLevelObjectForNextLevel();
        ClearLevelData();
    }
    private void ClearLevelObject()
    {
        mainCamera.DOOrthoSize(GameConfig.Instance.RatioScaleScreen < 1 ? 14.5f : 13.5f, 0.1f);
        for (int i = screwsInScene.Count - 1; i >= 0; i--)
        {
            screwsInScene[i].Dispose();
        }
        if (goalScrewParent.childCount > 0)
        {
            for (int i = goalScrewParent.childCount - 1; i >= 0; i--)
            {
                goalScrewParent.GetChild(i)?.GetComponent<GoalScrew>().PlayCloseEffect();
            }
        }
        if (glassParent.childCount > 0)
        {
            for (int i = glassParent.childCount - 1; i >= 0; i--)
            {
                glassParent.GetChild(i)?.GetComponent<Glass>().ReturnToPool();
            }
        }
        goalGradients[3].DOFade(0, 0f);
        goalGradients[3].transform.parent.localPosition = new Vector3(0, 10.83f, 10.83f);
        foreach (var gradient in goalGradients)
        {
            gradient.gameObject.SetActive(false);
        }
        goalScrewParent.localPosition = new Vector3(0, 0, 8f);
    }
    private void ClearLevelObjectForNextLevel()
    {
        mainCamera.DOOrthoSize(GameConfig.Instance.RatioScaleScreen < 1 ? 14.5f : 13.5f, 0.5f);
        for (int i = screwsInScene.Count - 1; i >= 0; i--)
        {
            screwsInScene[i].transform.SetParent(fakeScrewParent);
            oldScrews.Add(screwsInScene[i]);
        }
        if (goalScrewParent.childCount > 0)
        {
            for (int i = goalScrewParent.childCount - 1; i >= 0; i--)
            {
                goalScrewParent.GetChild(i)?.GetComponent<GoalScrew>().PlayCloseEffect();
            }
        }
        if (glassParent.childCount > 0)
        {
            for (int i = glassParent.childCount - 1; i >= 0; i--)
            {
                glassParent.GetChild(i)?.GetComponent<Glass>().ReturnToPool();
            }
        }
        goalGradients[3].DOFade(0, 0f);
        goalGradients[3].transform.parent.localPosition = new Vector3(0, 10.83f, 10.83f);
        foreach (var gradient in goalGradients)
        {
            gradient.gameObject.SetActive(false);
        }
        goalScrewParent.localPosition = new Vector3(0, 0, 8f);
    }
    private void ClearOldScrews()
    {
        for (int i = oldScrews.Count - 1; i >= 0; i--)
        {
            oldScrews[i].Dispose();
            oldScrews.RemoveAt(oldScrews.Count - 1);
        }
    }
    private void ClearLevelData()
    {
        screwsInScene.Clear();
        currentGoal.Clear();
        colorQueue.Clear();
        keepPlayingTime = 0;
        totalUndo = 0;
        totalFill = 0;
        totalExtraHolder = 0;
        actionSequence = string.Empty;
    }
    public void SetGameView(float ratio)
    {
        if (ratio < 1)
        {
            transform.localScale = Vector3.one * ratio;
            goalGradients[0].transform.parent.parent.localScale = Vector3.one * ratio;
            EffectManager.Instance.transform.localScale = Vector3.one * ratio;
        }
    }
    public void InitScrews(List<ScrewData> listScrewDatas, bool isNextLevel = false)
    {
        for (int i = 0; i < listScrewDatas.Count; i++)
        {
            Screw screw = Factory.Instance.GetScrewBySize(listScrewDatas[i].size, screwParent);
            screw.InitNutsInside(listScrewDatas[i], isNextLevel);
            screw.InitTowel(listScrewDatas[i].towelColor);
            screwsInScene.Add(screw);
        }


        UpdateScrew(Screen.width > Screen.height);



        //SortScrewPositions();
    }

    public void UpdateScrew(bool isLandScape)
    {
        if (isLandScape)
        {
            SortScrewPositionsLandScape(isLandScape);
        }
        else
        {
            SortScrewPositions();
        }


    }


    public void UpdateScrewHolder(bool isLandScape)
    {
        if (isLandScape)
        {
            for (int i = 0; i < currentGoal.Count; i++)
            {
                currentGoal[i].transform.localPosition = new Vector3(currentGoal[i].transform.localPosition.x, currentGoal[i].transform.localPosition.y, -1f);
            }
        }
        else
        {
            for (int i = 0; i < currentGoal.Count; i++)
            {
                currentGoal[i].transform.localPosition = new Vector3(currentGoal[i].transform.localPosition.x, currentGoal[i].transform.localPosition.y, 0f);

            }
        }


    }
    public void InitAllGoalScrews(List<int> colors, int goalNum)
    {
        totalLevelHolder = colors.Count;
        for (int i = 0; i < colors.Count; i++)
        {
            colorQueue.Enqueue(colors[i]);
        }
        for (int i = 0; i < goalNum; i++)
        {
            goalGradients[i].gameObject.SetActive(true);
            DOTween.Kill(goalGradients[i]);
            goalGradients[i].DOFade(0, 0);
            goalGradients[i].DOFade(120 / 255f, 0.5f);
            InitGoal(i, goalScrewPosDic[goalNum][i]);
            goalGradients[i].transform.localPosition = goalScrewPosDic[goalNum][i];
            EffectManager.Instance.InitFillEffect(i, goalScrewPosDic[goalNum][i]);
        }

        UpdateScrewHolder(Screen.width > Screen.height);


    }
    public void InitGoal(int index, Vector3 pos, bool isRecycle = false)
    {
        int colorKey = colorQueue.Dequeue();
        NutType type = (NutType)(colorKey / 12);
        ColorType color = (ColorType)(colorKey - (int)type * 12);
        GoalScrew goal = Factory.Instance.GetGoalScrewBySize(type, goalScrewParent);
        goal.index = index;
        goal.transform.localPosition = pos;
        goal.color = color;
        goal.initColor = color;
        goal.gradientColor = color;
        goal.ChangeColorMaterial(true);
        ChangeGoalGradientColor(index, color);
        goal.Init(isRecycle);
        currentGoal.Add(goal);
    }
    public void ReInitGoal(int index, Vector3 pos)
    {

        // Debug.LogError("ReInitGoal1");

        if (colorQueue.Count > 0)
        {
            // Debug.LogError("ReInitGoal2");
            InitGoal(index, pos, true);
        }
        else
        {
            // Debug.LogError("ReInitGoal3");
            goalGradients[index].DOFade(0, 0.7f);
        }

        if (GameManager.Instance.delayCheckLose)
        {
            // Debug.LogError("ReInitGoal4");
            GameManager.Instance.delayCheckLose = false;
            CheckLoseLevel();
        }

    }
    public void ChangeGoalGradientColor(int index, ColorType type)
    {
        goalGradients[index].DOColor(Factory.Instance.GetGradientColorByType(type), 0.5f).SetEase(Ease.Linear);
    }
    public void InitRopesInLevel(List<RopeData> ropeMap)
    {
        foreach (var rope in ropeMap)
        {
            if (rope.datas.Count < 4)
            {
                // Debug.LogError("Wrong Rope Data");
                continue;
            }
            Nut nut1 = screwsInScene[rope.datas[0]].myNutsList[rope.datas[1]];
            Nut nut2 = screwsInScene[rope.datas[2]].myNutsList[rope.datas[3]];
            nut1.SetConnectRope(nut2);
        }
    }
    public void InitGlassesInLevel(List<GlassData> glassMap)
    {
        //DisableGroundCollider();
        foreach (var glassData in glassMap)
        {
            if (glassData.datas.Count < 4)
            {
                // Debug.LogError("Wrong Glass Data");
                continue;
            }

            int screwNum = glassData.datas[2] < 100 ? 3 : 2;

            var glass = Factory.Instance.GetGlassBySize(screwNum, glassParent);

            if (!glassInScene.Contains(glass))
                glassInScene.Add(glass);

            Screw screw1 = screwsInScene[glassData.datas[0]];
            Screw screw2 = screwsInScene[glassData.datas[1]];

            Screw screw3 = screwNum == 3 ? screwsInScene[glassData.datas[2]] : null;

            float tanAngle = Mathf.Abs(screw1.transform.localPosition.z - screw2.transform.localPosition.z) / Mathf.Abs(screw1.transform.localPosition.x - screw2.transform.localPosition.x);
            float angle = Mathf.Atan(tanAngle) * Mathf.Rad2Deg;
            if (screw1.transform.position.x > screw2.transform.position.x)
                angle *= -1;

            if (screwNum == 2)
            {
                float distance = Vector3.Distance(screw1.transform.position, screw2.transform.position);
                float posX = (screw1.transform.localPosition.x + screw2.transform.localPosition.x) / 2f;
                float posZ = (screw1.transform.localPosition.z + screw2.transform.localPosition.z) / 2f;
                glass.InitGlass(posX, posZ, glassData.datas[3], distance, angle, new List<Screw> { screw1, screw2 });
            }
            else if (screwNum == 3)
            {
                float distance = Vector3.Distance(screw1.transform.position, screw3.transform.position);
                glass.InitGlass(screw2.transform.localPosition.x, screw2.transform.localPosition.z, glassData.datas[3], distance, angle, new List<Screw> { screw1, screw2, screw3 });
            }
            //ActiveGroundCollider();
        }
    }
    public void ActiveGroundCollider()
    {
        if (!plane.activeSelf)
            plane.SetActive(true);
    }
    public void DisableGroundCollider()
    {
        plane.SetActive(false);
    }
    public void InitBG()
    {
        if (GameConfig.Instance.RatioScaleScreen > 1)
        {
            float delta1 = (2.7f - 2.5f) / (ratioScale_2450 - 1);
            float delta2 = (1.1f - 0.9f) / (ratioScale_2450 - 1);
            float screenScale = 2.5f + delta1 * (GameConfig.Instance.RatioScaleScreen - 1);
            float posBG = 0.9f + delta2 * (GameConfig.Instance.RatioScaleScreen - 1);
            bg.localScale = Vector3.one * screenScale;
            bg.localPosition = new Vector3(0, posBG, posBG);
            screwParent.localPosition = new Vector3(0, 0, -4 * GameConfig.Instance.RatioScaleScreen);
            glassParent.localPosition = new Vector3(0, 0, -4 * GameConfig.Instance.RatioScaleScreen);
        }

    }
    public void EnableNutTrailRenderer()
    {
        foreach (Screw screw in screwsInScene)
        {
            screw.EnableNutTrail();
        }
    }
    #endregion

    #region Object Positions

    private Dictionary<int, List<Vector3>> screwPosDic_5 = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0) } },
        {3, new List<Vector3>() { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0) } },
        {4, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {5, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(0, 0, 0), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {6, new List<Vector3>() { new Vector3(-4, 0, 3), new Vector3(0, 0, 3), new Vector3(4, 0, 3), new Vector3(-4, 0, -4), new Vector3(0, 0, -4), new Vector3(4, 0, -4) } },
        {7, new List<Vector3>() { new Vector3(-2.5f, 0, 5), new Vector3(2.5f, 0, 5), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -5), new Vector3(2.5f, 0, -5) } },
        {8, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-2, 0, 1.5f), new Vector3(2, 0, 1.5f), new Vector3(-2, 0, -3.5f), new Vector3(2, 0, -3.5f), new Vector3(-4.5f, 0, -7), new Vector3(4.5f, 0, -7) } },
        {9, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(0, 0, 5), new Vector3(4, 0, 5), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0), new Vector3(-4, 0, -5), new Vector3(0, 0, -5), new Vector3(4, 0, -5) } },
        {10, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(4f, 0, 5), new Vector3(-2, 0, 2), new Vector3(2, 0, 2), new Vector3(-5, 0, -0.5f), new Vector3(5, 0, -0.5f), new Vector3(-2, 0, -3), new Vector3(2, 0, -3), new Vector3(-4f, 0, -6), new Vector3(4f, 0, -6) } },
        {11, new List<Vector3>() { new Vector3(-5, 0, 6), new Vector3(5, 0, 6), new Vector3(-2.5f, 0, 3), new Vector3(2.5f, 0, 3), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -3), new Vector3(2.5f, 0, -3f), new Vector3(-5, 0, -6f), new Vector3(5, 0, -6f) } },
        {12, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(-1.5f, 0, 5), new Vector3(1.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-4.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(4.5f, 0, 0), new Vector3(-4.5f, 0, -5), new Vector3(-1.5f, 0, -5), new Vector3(1.5f, 0, -5), new Vector3(4.5f, 0, -5) } },
        {13, new List<Vector3>() { new Vector3(-4f, 0, 6), new Vector3(0f, 0, 6), new Vector3(4f, 0, 6), new Vector3(-2f, 0, 3), new Vector3(2f, 0, 3), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4f, 0, 0), new Vector3(-2f, 0, -3), new Vector3(2f, 0, -3), new Vector3(-4f, 0, -6), new Vector3(0f, 0, -6), new Vector3(4f, 0, -6) } }
    };
    private Dictionary<int, List<Vector3>> screwPosDic_10 = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0) } },
        {3, new List<Vector3>() { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0) } },
        {4, new List<Vector3>() { new Vector3(-4.5f, 0, -3), new Vector3(-1.5f, 0, 1), new Vector3(1.5f, 0, -3), new Vector3(4.5f, 0, 1) } },
        {5, new List<Vector3>() { new Vector3(-4, 0, 2.5f), new Vector3(4, 0, 2.5f), new Vector3(0, 0, -2f), new Vector3(-4, 0, -6.5f), new Vector3(4, 0, -6.5f) } },
        {6, new List<Vector3>() { new Vector3(-4, 0, 2), new Vector3(0, 0, 2), new Vector3(4, 0, 2), new Vector3(-4, 0, -6.5f), new Vector3(0, 0, -6.5f), new Vector3(4, 0, -6.5f) } },
        {7, new List<Vector3>() { new Vector3(-5, 0, 4f), new Vector3(5, 0, 4f), new Vector3(-3, 0, -1), new Vector3(0, 0, -1), new Vector3(3, 0, -1), new Vector3(-5, 0, -6.5f), new Vector3(5, 0, -6.5f) } },
        {8, new List<Vector3>() { new Vector3(-4.5f, 0, 3), new Vector3(-1.5f, 0, 3), new Vector3(1.5f, 0, 3), new Vector3(4.5f, 0, 3), new Vector3(-4.5f, 0, -5.5f), new Vector3(-1.5f, 0, -5.5f), new Vector3(1.5f, 0, -5.5f), new Vector3(4.5f, 0, -5.5f) } },
        {9, new List<Vector3>() { new Vector3(-5.5f, 0, 5.5f), new Vector3(5.5f, 0, 5.5f), new Vector3(-2.5f, 0, 3.5f), new Vector3(2.5f, 0, 3.5f), new Vector3(0, 0, -0.5f), new Vector3(-2.5f, 0, -5f), new Vector3(2.5f, 0, -5f), new Vector3(-5.5f, 0, -6.5f), new Vector3(5.5f, 0, -6.5f) } },
        {10, new List<Vector3>() { new Vector3(-5.25f, 0, 3), new Vector3(-1.25f, 0, 3), new Vector3(1.25f, 0, 3), new Vector3(5.25f, 0, 3), new Vector3(-3.25f, 0, -1f), new Vector3(3.25f, 0, -1f), new Vector3(-5.25f, 0, -5.5f), new Vector3(-1.25f, 0, -5.5f), new Vector3(1.25f, 0, -5.5f), new Vector3(5.25f, 0, -5.5f) } },
        {11, new List<Vector3>() { new Vector3(-5, 0, 6), new Vector3(5, 0, 6), new Vector3(-2.5f, 0, 3), new Vector3(2.5f, 0, 3), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -3), new Vector3(2.5f, 0, -3f), new Vector3(-5, 0, -6f), new Vector3(5, 0, -6f) } },
        {12, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(-1.5f, 0, 5), new Vector3(1.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-4.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(4.5f, 0, 0), new Vector3(-4.5f, 0, -5), new Vector3(-1.5f, 0, -5), new Vector3(1.5f, 0, -5), new Vector3(4.5f, 0, -5) } },
        {13, new List<Vector3>() { new Vector3(-4f, 0, 6), new Vector3(0f, 0, 6), new Vector3(4f, 0, 6), new Vector3(-2f, 0, 3), new Vector3(2f, 0, 3), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4f, 0, 0), new Vector3(-2f, 0, -3), new Vector3(2f, 0, -3), new Vector3(-4f, 0, -6), new Vector3(0f, 0, -6), new Vector3(4f, 0, -6) } }
    };
    private Dictionary<int, List<Vector3>> screwPosDic_10_2 = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0) } },
        {3, new List<Vector3>() { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0) } },
        {4, new List<Vector3>() { new Vector3(-4.5f, 0, -3), new Vector3(-1.5f, 0, 1), new Vector3(1.5f, 0, -3), new Vector3(4.5f, 0, 1) } },
        {5, new List<Vector3>() { new Vector3(-4, 0, 2.5f), new Vector3(4, 0, 2.5f), new Vector3(0, 0, -2f), new Vector3(-4, 0, -6.5f), new Vector3(4, 0, -6.5f) } },
        {6, new List<Vector3>() { new Vector3(-5, 0, 3), new Vector3(5, 0, 3), new Vector3(-2, 0, -1), new Vector3(2, 0, -1), new Vector3(-5, 0, -5), new Vector3(5, 0, -5) } },
        {7, new List<Vector3>() { new Vector3(-5, 0, 2.5f), new Vector3(5, 0, 2.5f), new Vector3(-3, 0, -1), new Vector3(0, 0, -1), new Vector3(3, 0, -1), new Vector3(-5, 0, -6.5f), new Vector3(5, 0, -6.5f) } },
        {8, new List<Vector3>() { new Vector3(-4.5f, 0, 3), new Vector3(-1.5f, 0, 3), new Vector3(1.5f, 0, 3), new Vector3(4.5f, 0, 3), new Vector3(-4.5f, 0, -5.5f), new Vector3(-1.5f, 0, -5.5f), new Vector3(1.5f, 0, -5.5f), new Vector3(4.5f, 0, -5.5f) } },
        {9, new List<Vector3>() { new Vector3(-5.5f, 0, 5.5f), new Vector3(5.5f, 0, 5.5f), new Vector3(-2.5f, 0, 3.5f), new Vector3(2.5f, 0, 3.5f), new Vector3(0, 0, -0.5f), new Vector3(-2.5f, 0, -5f), new Vector3(2.5f, 0, -5f), new Vector3(-5.5f, 0, -6.5f), new Vector3(5.5f, 0, -6.5f) } },
        {10, new List<Vector3>() { new Vector3(-5.25f, 0, 3), new Vector3(-1.25f, 0, 3), new Vector3(1.25f, 0, 3), new Vector3(5.25f, 0, 3), new Vector3(-3.25f, 0, -1f), new Vector3(3.25f, 0, -1f), new Vector3(-5.25f, 0, -5.5f), new Vector3(-1.25f, 0, -5.5f), new Vector3(1.25f, 0, -5.5f), new Vector3(5.25f, 0, -5.5f) } },
        {11, new List<Vector3>() { new Vector3(-5, 0, 6), new Vector3(5, 0, 6), new Vector3(-2.5f, 0, 3), new Vector3(2.5f, 0, 3), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -3), new Vector3(2.5f, 0, -3f), new Vector3(-5, 0, -6f), new Vector3(5, 0, -6f) } },
        {12, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(-1.5f, 0, 5), new Vector3(1.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-4.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(4.5f, 0, 0), new Vector3(-4.5f, 0, -5), new Vector3(-1.5f, 0, -5), new Vector3(1.5f, 0, -5), new Vector3(4.5f, 0, -5) } },
        {13, new List<Vector3>() { new Vector3(-4f, 0, 6), new Vector3(0f, 0, 6), new Vector3(4f, 0, 6), new Vector3(-2f, 0, 3), new Vector3(2f, 0, 3), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4f, 0, 0), new Vector3(-2f, 0, -3), new Vector3(2f, 0, -3), new Vector3(-4f, 0, -6), new Vector3(0f, 0, -6), new Vector3(4f, 0, -6) } }
    };

    private Dictionary<int, List<Vector3>> goalScrewPosDic = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2, 0, 0), new Vector3(2, 0, 0) }},
        {3,  new List<Vector3>() { new Vector3(-3.5f, 0, 0), new Vector3(0, 0, 0), new Vector3(3.5f, 0, 0) }},
        {4,  new List<Vector3>() { new Vector3(-3.5f, 0, 0), new Vector3(0, 0, 0), new Vector3(3.5f, 0, 0), new Vector3(7, 0, 0) }},
    };




    private Dictionary<int, List<Vector3>> screwPosLandscape = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0) } },
        {3, new List<Vector3>() { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0) } },
        {4, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {5, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(0, 0, 0), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {6, new List<Vector3>() { new Vector3(-4, 0, 3), new Vector3(0, 0, 3), new Vector3(4, 0, 3), new Vector3(-4, 0, -4), new Vector3(0, 0, -4), new Vector3(4, 0, -4) } },
        {7, new List<Vector3>() { new Vector3(-2.5f, 0, 5), new Vector3(2.5f, 0, 5), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -5), new Vector3(2.5f, 0, -5) } },
        {8, new List<Vector3>() { new Vector3(-9.5f, 0, 3), new Vector3(9.5f, 0, 3), new Vector3(-4.5f, 0, 1.5f), new Vector3(4.5f, 0, 1.5f), new Vector3(-2, 0, -3.5f), new Vector3(2, 0, -3.5f), new Vector3(-9.5f, 0, -7), new Vector3(9.5f, 0, -7) } },
        {9, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(0, 0, 5), new Vector3(4, 0, 5), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0), new Vector3(-4, 0, -5), new Vector3(0, 0, -5), new Vector3(4, 0, -5) } },
        {10, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(4f, 0, 5), new Vector3(-2, 0, 2), new Vector3(2, 0, 2), new Vector3(-5, 0, -0.5f), new Vector3(5, 0, -0.5f), new Vector3(-2, 0, -3), new Vector3(2, 0, -3), new Vector3(-4f, 0, -6), new Vector3(4f, 0, -6) } },
        {11, new List<Vector3>() {
            new Vector3(-9.5f, 0, 5), new Vector3(0f, 0, 5), new Vector3(9.5f, 0, 5),      // 3 điểm trên cùng
            new Vector3(-4.5f, 0, 2.5f), new Vector3(0f, 0, 0.5f), new Vector3(4.5f, 0, 2.5f), // thêm 1 điểm giữa vào dòng 2 (từ 2 → 3 điểm)
            new Vector3(-4.5f, 0, -2.5f), new Vector3(4.5f, 0, -2.5f),                         // dòng 3 như cũ
            new Vector3(-9.5f, 0, -6f), new Vector3(0f, 0, -6f), new Vector3(9.5f, 0, -6f) // dòng cuối thêm điểm giữa
        }},
        {12, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(-1.5f, 0, 5), new Vector3(1.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-4.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(4.5f, 0, 0), new Vector3(-4.5f, 0, -5), new Vector3(-1.5f, 0, -5), new Vector3(1.5f, 0, -5), new Vector3(4.5f, 0, -5) } },
        {13, new List<Vector3>() { new Vector3(-4f, 0, 6), new Vector3(0f, 0, 6), new Vector3(4f, 0, 6), new Vector3(-2f, 0, 3), new Vector3(2f, 0, 3), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4f, 0, 0), new Vector3(-2f, 0, -3), new Vector3(2f, 0, -3), new Vector3(-4f, 0, -6), new Vector3(0f, 0, -6), new Vector3(4f, 0, -6) } }
    };
    public void SortScrewPositions()
    {
        if (screwsInScene.Find(screw => screw.size == 10) != null)
        {
            if (GameUtils.Level_Type == 1 && GameUtils.Level <= 100)
            {
                for (int i = 0; i < screwsInScene.Count; i++)
                {
                    if (GameConfig.Instance.RatioScaleScreen > 1)
                    {
                        screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_10_2[screwsInScene.Count][i].x, screwPosDic_10_2[screwsInScene.Count][i].y, screwPosDic_10_2[screwsInScene.Count][i].z * GameConfig.Instance.RatioScaleScreen);

                    }
                    else
                    {
                        screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_10_2[screwsInScene.Count][i].x, screwPosDic_10_2[screwsInScene.Count][i].y, screwPosDic_10_2[screwsInScene.Count][i].z);
                    }
                    if (GameUtils.Level == 51)
                    {
                        screwsInScene[i].transform.localPosition -= new Vector3(0, 0, 1.5f);
                    }
                }

            }
            else
            {
                for (int i = 0; i < screwsInScene.Count; i++)
                {
                    if (GameConfig.Instance.RatioScaleScreen > 1)
                    {
                        screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_10[screwsInScene.Count][i].x, screwPosDic_10[screwsInScene.Count][i].y, screwPosDic_10[screwsInScene.Count][i].z * GameConfig.Instance.RatioScaleScreen);
                    }
                    else
                    {
                        screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_10[screwsInScene.Count][i].x, screwPosDic_10[screwsInScene.Count][i].y, screwPosDic_10[screwsInScene.Count][i].z);
                    }
                    if (GameUtils.Level == 108)
                    {
                        screwsInScene[i].transform.localPosition += new Vector3(0, 0, 2);
                    }
                }
            }

        }
        else
        {


            for (int i = 0; i < screwsInScene.Count; i++)
            {
                if (GameConfig.Instance.RatioScaleScreen > 1)
                {
                    screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_5[screwsInScene.Count][i].x, screwPosDic_5[screwsInScene.Count][i].y, screwPosDic_5[screwsInScene.Count][i].z * GameConfig.Instance.RatioScaleScreen);
                }
                else
                {
                    screwsInScene[i].transform.localPosition = new Vector3(screwPosDic_5[screwsInScene.Count][i].x, screwPosDic_5[screwsInScene.Count][i].y, screwPosDic_5[screwsInScene.Count][i].z);
                }
            }
        }
    }


    public void SortScrewPositionsLandScape(bool isLandScape)
    {

        for (int i = 0; i < screwsInScene.Count; i++)
        {
            if (GameConfig.Instance.RatioScaleScreen > 1)
            {
                screwsInScene[i].transform.localPosition = new Vector3(screwPosLandscape[screwsInScene.Count][i].x, screwPosLandscape[screwsInScene.Count][i].y, screwPosLandscape[screwsInScene.Count][i].z * GameConfig.Instance.RatioScaleScreen);
            }
            else
            {
                screwsInScene[i].transform.localPosition = new Vector3(screwPosLandscape[screwsInScene.Count][i].x, screwPosLandscape[screwsInScene.Count][i].y, screwPosLandscape[screwsInScene.Count][i].z);
            }
        }

    }

    public float addColOffset = 2f;

    #endregion

    #region Game Logic

    public void CheckLoseLevel()
    {
        bool isLose = true;


        // Debug.LogError(currentGoal.Count+ " CheckLoseLevel");
        for (int i = 0; i < currentGoal.Count; i++)
        {
            GoalScrew goal = currentGoal[i];
            if (goal.amountSortPre == goal.size)
            {
                GameManager.Instance.delayCheckLose = true;
                return;

            }
        }
        //if (currentGoal.Find(goal => goal.amountSortPre == goal.size))
        //{
        //    GameManager.Instance.delayCheckLose = true;
        //    return;
        //}

        if (currentGoal.Count == 0)
            return;

        for (int i = 0; i < screwsInScene.Count; i++)
        {
            if (screwsInScene[i].amountSortPre == 0 || (screwsInScene[i].glassBlockIndex.Count > 0 && screwsInScene[i].amountSortPre <= screwsInScene[i].glassBlockIndex[0]) || screwsInScene[i].towelColor != ColorType.None) continue;
            Nut lastNut_ScrewChange = screwsInScene[i].myNutsList[screwsInScene[i].amountSortPre - 1];
            GoalScrew goal = GetGoalScrewByColor(lastNut_ScrewChange.data.color, lastNut_ScrewChange.data.nutType);
            if (goal != null)
            {
                if (lastNut_ScrewChange.connectedNut != null)
                {
                    if (lastNut_ScrewChange.data.color == lastNut_ScrewChange.connectedNut.data.color)
                    {
                        foreach (GoalScrew goal1 in currentGoal)
                        {
                            if (goal1.color == ColorType.None)
                            {
                                isLose = false;
                                break;
                            }
                            if (goal1.color == lastNut_ScrewChange.data.color && goal1.type == lastNut_ScrewChange.data.nutType)
                            {
                                goal1.ropeCount = 1;
                                if (lastNut_ScrewChange.connectedNut.CheckCanMove())
                                {
                                    isLose = false;
                                    goal1.ropeCount = 0;
                                    break;
                                }
                                else
                                    goal1.ropeCount = 0;
                            }
                        }
                        if (!isLose)
                            break;
                    }
                    else
                    {
                        if (!lastNut_ScrewChange.connectedNut.CheckCanMove())
                            continue;
                    }
                }
                else
                {
                    isLose = false;
                    break;
                }
            }
            else
            {
                continue;
            }
        }
        if (isLose)
        {
            GameManager.Instance.SetLose();
            //   HandleFirebase.Instance.LogEventWithString(HandleFirebase.Lose_By_end_level, "OutOfSpace");
        }

    }
    public void CheckWinLevel()
    {
        // Debug.LogError("CheckWinLevel");
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            Screw screw = screwsInScene[i];
            if (screw.state == ScrewState.IDLE || screw.state == ScrewState.ACTION)
                return;
        }

        if (GameUtils.Level == 1)
        {
            //  TutorialController.Instance.FadeOutTutText();
        }






        GameManager.Instance.SetWin();
        return;
        //}
    }
    public void KeepPlayingLevel()
    {
        keepPlayingTime++;
        CheckResetStateAfterExplore();
        if (keepPlayingTime == 1)
        {
            BoosterController.Instance.Fill_Holder(currentGoal[0]);
        }
        else if (keepPlayingTime == 2)
        {
            BoosterController.Instance.Add_Holder_KeepPlaying();
        }
    }

    public bool CheckExistScrewByColor(ColorType color)
    {
        return screwsInScene.Find(screw => screw.state != ScrewState.EMPTY && screw.amountSortPre > 0 && screw.myNutsList[screw.amountSortPre - 1].data.color == color) != null;
    }
    //public GoalScrew GetGoalScrewByColor(ColorType color, NutType type)
    //{
    //    if (currentGoal.Find(goal => goal.amountSortPre < goal.size && goal.type == type && goal.color == color) != null)
    //        return currentGoal.Find(goal =>  goal.amountSortPre < goal.size && goal.type == type && goal.color == color);
    //    else
    //        return currentGoal.Find(goal => goal.color == ColorType.None && goal.type == type);

    //}
    public GoalScrew GetGoalScrewByColor(ColorType color, NutType type, bool isForRope = false)
    {
        // Tìm goal có color và type khớp, còn slot
        for (int i = 0; i < currentGoal.Count; i++)
        {
            var goal = currentGoal[i];
            if (isForRope)
            {
                if (goal.amountSortPre + goal.ropeCount < goal.size && goal.type == type && goal.color == color)
                {
                    return goal;
                }
            }
            else
            {
                if (goal.amountSortPre < goal.size && goal.type == type && goal.color == color)
                {
                    return goal;
                }
            }

        }

        // Nếu không có, tìm goal có color = None và đúng type
        for (int i = 0; i < currentGoal.Count; i++)
        {
            var goal = currentGoal[i];
            if (goal.type == type && goal.color == ColorType.None)
                return goal;
        }

        return null;
    }
    //public List<Screw> GetListScrewByColor(ColorType color)
    //{
    //    List<Screw > list = new List<Screw>();

    //    foreach (var screw in screwsInScene)
    //    {
    //        if (screw.myNutsList.Find(nut => nut.data.color == color) != null)
    //            list.Add(screw);
    //    }

    //    return list;
    //}
    public List<Screw> GetListScrewByColorAndType(ColorType color, NutType type)
    {
        List<Screw> list = new List<Screw>();
        screwsInScene.Sort((a, b) =>
        {
            bool aActive = a.towelColor == ColorType.None;
            bool bActive = b.towelColor == ColorType.None;

            return bActive.CompareTo(aActive);
        });

        for (int i = 0; i < screwsInScene.Count; i++)
        {
            Screw screw = screwsInScene[i];
            List<Nut> nuts = screw.myNutsList;

            for (int j = 0; j < nuts.Count; j++)
            {
                if (nuts[j].data.color == color && nuts[j].data.nutType == type)
                {
                    if (!list.Contains(screw))
                        list.Add(screw);
                    break;
                }
            }
        }
        return list;
    }
    public void ChangeSpecialNutComponentInLevel()
    {
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            screwsInScene[i].ChangeSpecialNutComponents();
        }
    }
    public void BreakIceNuts()
    {
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            screwsInScene[i].BreakIceNuts();
        }
    }
    public void CheckBreakGlass()
    {
        for (int i = 0; i < glassInScene.Count; i++)
        {
            glassInScene[i].CheckBreak();
        }
    }
    public void CheckOpenTowel(ColorType color)
    {
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            screwsInScene[i].CheckOpenTowel(color);
        }
    }
    public void CheckResetStateAfterExplore()
    {
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            if (screwsInScene[i].amountSortPre > 0 && screwsInScene[i].myNutsList[screwsInScene[i].amountSortPre - 1].isExplore)
            {
                for (int j = 0; j < screwsInScene[i].myNutsList.Count; j++)
                {
                    screwsInScene[i].myNutsList[j].ResetStateAfterExplore();
                    screwsInScene[i].myNutsList[j].transform.localPosition = new Vector3(0, screwsInScene[i].delta * j, 0);
                }
            }
        }
        DisableGroundCollider();
    }
    #endregion

    #region Booster
    public ColorType GetValidNutToFill(NutType type)
    {
        Dictionary<ColorType, int> checkDict = new Dictionary<ColorType, int>();
        for (int i = 0; i < screwsInScene.Count; i++)
        {
            for (int j = 0; j < screwsInScene[i].myNutsList.Count; j++)
            {
                if (screwsInScene[i].myNutsList[j].data.nutType != type)
                    continue;

                if (!checkDict.ContainsKey(screwsInScene[i].myNutsList[j].data.color))
                    checkDict.Add(screwsInScene[i].myNutsList[j].data.color, 1);
                else
                {
                    checkDict[screwsInScene[i].myNutsList[j].data.color]++;
                    if (checkDict[screwsInScene[i].myNutsList[j].data.color] == 5)
                        return screwsInScene[i].myNutsList[j].data.color;
                }
            }
        }

        return ColorType.None;
    }
    public void EnableFillBoosterMode()
    {
        GameManager.Instance.IsAction = true;
        //TutorialController.Instance.filterTween?.Kill();
        filterTween?.Kill();
        filterTween = transform.DOMove(new Vector3(0, 38, -38), 0.02f).OnComplete(() =>
        {
            foreach (var goal in currentGoal)
            {
                goal.OpenFillBoosterMode();
            }

        });
        //transform.position = new Vector3(0, 30, -30);
    }
    public void DisableFillBoosterMode()
    {
        filterTween?.Kill();
        foreach (var goal in currentGoal)
        {
            goal.CloseFillBoosterMode();
        }

        transform.DOMove(new Vector3(0, 0, 0), 0.02f).OnComplete(() =>
        {
            GameManager.Instance.IsAction = false;
        });
        //transform.position = new Vector3(0, 0, 0);

    }
    public void MoveObjFrontOfUI()
    {
        filterTween?.Kill();
        filterTween = transform.DOMove(new Vector3(0, 38, -38), 0.02f).OnComplete(() =>
        {

        });
    }
    public void MoveObjBackOfUI()
    {
        filterTween?.Kill();
        transform.DOMove(new Vector3(0, 0, 0), 0.02f).OnComplete(() =>
        {

        });
        //transform.position = new Vector3(0, 0, 0);
    }
    public bool CanUseBooster()
    {
        return nutsInScene.Find(nut => nut.isMoving) == null;
    }
    public void InitHolderByBooster()
    {
        InitFourthHolder();
    }
    public void InitFourthHolder()
    {
        mainCamera.DOOrthoSize(GameConfig.Instance.RatioScaleScreen < 1 ? 14.7f : 13.7f, 0.3f).OnComplete(() =>
        {
            if (IsEnoughNutForFourthHolder())
            {
                InitGoal(3, goalScrewPosDic[4][3]);
            }
            foreach (var goal in currentGoal)
            {
                EffectManager.Instance.InitFillEffect(goal.index, goalScrewPosDic[4][goal.index] + new Vector3(-1.75f, 0, 0));
            }
            EffectManager.Instance.PlayAddColEffect((goalScrewParent.localPosition + goalScrewPosDic[4][3]));
            goalGradients[3].gameObject.SetActive(true);
            goalGradients[3].DOFade(120 / 255f, 1f);
        });

        goalGradients[0].transform.parent.DOLocalMove(new Vector3(-1.75f, 10.83f, 10.83f), 0.5f);
        goalScrewParent.DOLocalMove(new Vector3(-1.75f, 0, 8f), 0.5f);
    }
    public bool IsEnoughNutForFourthHolder()
    {
        int num = 0;
        foreach (var screw in screwsInScene)
        {
            if (screw.state != ScrewState.IDLE)
                continue;

            num += screw.amountSortPre;
        }
        foreach (var holder in currentGoal)
        {
            num -= (holder.size - holder.amountSortPre);
        }

        return num > 0;
    }
    #endregion

    #region Tutorial
    private float timeTut_1 = 1.5f;
    public bool isTutUndo = false;
    private Coroutine tutCoroutine;

    public void StartTutLevel1()
    {
        tutCoroutine = StartCoroutine(TutCountdownCoroutine());
    }
    private IEnumerator TutCountdownCoroutine()
    {
        float time = 0f;
        while (time < timeTut_1)
        {
            if (GameManager.Instance.CheckCanAction())
            {
                time += Time.deltaTime;
            }
            yield return null;
        }

        OpenHandTutLevel1();
        //tutCoroutine = null; 
    }
    public void ResetTutHand_1()
    {

        if (tutCoroutine != null)
        {
            StopCoroutine(tutCoroutine);
            tutCoroutine = null;
            //TutorialController.Instance.FadeOutHandTut();
        }
        tutCoroutine = StartCoroutine(TutCountdownCoroutine());
    }
    public void StopTutLevel1()
    {
        if (tutCoroutine != null)
        {
            StopCoroutine(tutCoroutine);
            tutCoroutine = null;
            //  TutorialController.Instance.FadeOutHandTut();
        }
    }
    public void OpenTutLevel1()
    {
        //  TutorialController.Instance.SuggestByText("SORT 5 NUTS");
        tutCoroutine = StartCoroutine(TutCountdownCoroutine());
    }

    public void OpenHandTutLevel1()
    {
        Vector3 pos = Vector3.zero;
        if (screwsInScene[0].amountSortPre == 5)
            pos = new Vector3(-180, -30 * GameConfig.Instance.RatioScaleScreen, -0);
        else if (screwsInScene[1].amountSortPre == 5)
            pos = new Vector3(180, -30 * GameConfig.Instance.RatioScaleScreen, -0);
        else if (screwsInScene[0].amountSortPre > 0)
            pos = new Vector3(-180, -130 * GameConfig.Instance.RatioScaleScreen, 0);
        else
            pos = new Vector3(180, -130 * GameConfig.Instance.RatioScaleScreen, 0);
        //TutorialController.Instance.SuggestByClick(pos);
    }
    public void OpenHandTutLevel6()
    {
        isTutUndo = true;
        handTut6 = DOVirtual.DelayedCall(0.5f, () =>
        {
            Vector3 pos = new Vector3(-300, 110 / GameConfig.Instance.RatioScaleScreen, -0);
            // TutorialController.Instance.SuggestByClick(pos);
        });
    }
    public void KillTweenTut6()
    {
        if (handTut6 != null)
            handTut6.Kill();
    }

    #endregion

    #region Firebase
    public string GetTotalNut()
    {
        int count = 0;
        foreach (Screw screw in screwsInScene)
        {
            count += screw.size;
        }

        return count.ToString();
    }
    public string GetClearedNut()
    {
        int count = 0;
        foreach (Screw screw in screwsInScene)
        {
            count += (screw.size - screw.amountSortPre);
        }

        return count.ToString();
    }
    public string GetTotalHolder()
    {
        return totalLevelHolder.ToString();
    }
    public string GetClearerHolder()
    {
        return (totalLevelHolder - colorQueue.Count - currentGoal.Count).ToString();
    }
    public void AddLevelSequence(string t)
    {
        if (actionSequence == string.Empty)
            actionSequence += t;
        else
        {
            actionSequence += ",";
            actionSequence += t;
        }
    }
    #endregion
    #region TMT code
    public List<ParticleSystem> screwCloseEffect;
    public List<FXHolderCellComplete> fxScrewCloseEffect;
    #endregion TMT code
}
