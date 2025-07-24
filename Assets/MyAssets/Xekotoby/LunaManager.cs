
using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class LunaManager : MonoBehaviour
{
    public static LunaManager instace;
    public Camera cameraGameplay;
    public Camera cameraUi;
    public GameObject bg;
    public float screenAspect;
    public Transform winpopup;
    private float lastScreenWidth;
    private float lastScreenHeight;
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] RectTransform boxBooster;
    public Transform Rect14;
    LevelManager levelManager;
    [Header("Time Zone")]
    [SerializeField] RectTransform boxTimer;
    [SerializeField] RectTransform posTimer1;
    [SerializeField] RectTransform posTimer2;
    [SerializeField] List<Transform> objGameList = new List<Transform>();
    [SerializeField] RectTransform boxIQ;
    [SerializeField] RectTransform posIQ1;
    [SerializeField] RectTransform posIQ2;
    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        levelManager = LevelManager.Instance;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        CheckAndApplyOrientation();
    }

    private void Update()
    {
        if (Screen.width == lastScreenWidth && Screen.height == lastScreenHeight) return;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        CheckAndApplyOrientation();
    }

    public void CheckAndApplyOrientation()
    {
        screenAspect = (float)Screen.width / (float)Screen.height;
        if (screenAspect < 0.55f)
        {
            ChangeObjWithCam(posTimer1, posIQ1, levelManager.isHasBooster ? 15.5f : 14, levelManager.isHasBooster ? -1.5f : -3.5f);
        }
        if (screenAspect >= 0.55f && screenAspect < 0.65f)
        {
            ChangeObjWithCam(posTimer1, posIQ1, levelManager.isHasBooster ? 16.5f : 13, levelManager.isHasBooster ? -1.5f : -3.5f);
        }
        else if (screenAspect >= 0.65f && screenAspect < 0.8f)
        {
            ChangeObjWithCam(posTimer1, posIQ1, levelManager.isHasBooster ? 14.8f : 12, levelManager.isHasBooster ? -1.5f : -3.5f, 1);
        }
        else if (screenAspect >= 0.80f)
        {
            ChangeObjWithCam(posTimer2, posIQ2, levelManager.isHasBooster ? 13.8f : 11, levelManager.isHasBooster ? -0.8f : -2.8f, 1);
        }
        levelManager.CallFillImgFreeze();
    }

    void ChangeObjWithCam(RectTransform targetRect, RectTransform targetRectIQ, float camSize = 14, float objGamePosY = -3.5f, int match = 0)
    {
        ZoomInCamera(camSize);
        levelManager.UpdateScrew(false);
        levelManager.UpdateScrewHolder(false);
        canvasScaler.matchWidthOrHeight = match;
        Vector2 sizeBoxBooster = boxBooster.sizeDelta;
        sizeBoxBooster.y = 540;
        boxBooster.sizeDelta = sizeBoxBooster;
        boxTimer.anchorMin = targetRect.anchorMin;
        boxTimer.anchorMax = targetRect.anchorMax;
        boxTimer.pivot = targetRect.pivot;
        boxTimer.position = targetRect.position;
        foreach (var item in objGameList)
        {
            item.position = new Vector3(0, objGamePosY, 0);
        }
        boxIQ.anchorMin = levelManager.isTimer ? targetRectIQ.anchorMin : posIQ2.anchorMin;
        boxIQ.anchorMax = levelManager.isTimer ? targetRectIQ.anchorMax : posIQ2.anchorMax;
        boxIQ.pivot = levelManager.isTimer ? targetRectIQ.pivot : posIQ2.pivot;
        boxIQ.position = levelManager.isTimer ? targetRectIQ.position : posIQ2.position;

    }

    void ZoomInCamera(float target1)
    {
        cameraGameplay.DOOrthoSize(target1, 0.2f).SetEase(Ease.OutQuad);
        cameraUi.DOOrthoSize(target1 - 2f, 0.2f).SetEase(Ease.OutQuad);
    }

    public float GetOrthoSizeLandscape(float aspect)
    {
        // Mặc định: 16:9 → aspect ≈ 1.77, size ≈ 9.68
        // Rộng hơn: 21:9 → aspect ≈ 2.33 → có thể nhỏ hơn chút
        return Mathf.Lerp(10f, 9f, Mathf.InverseLerp(1.6f, 2.3f, aspect));
    }

    public void OnPlayButtonClick()
    {
        Luna.Unity.Playable.InstallFullGame();
    }
    public void Btn_GameEnded()
    {
        Luna.Unity.LifeCycle.GameEnded();
    }

    private void OnEnable()
    {
        Luna.Unity.LifeCycle.OnPause += PauseGameplay;
        Luna.Unity.LifeCycle.OnResume += ResumeGameplay;
    }

    private void OnDisable()
    {
        Luna.Unity.LifeCycle.OnPause -= PauseGameplay;
        Luna.Unity.LifeCycle.OnResume -= ResumeGameplay;
    }

    private void ResumeGameplay()
    {
        Time.timeScale = 1f;
    }

    private void PauseGameplay()
    {
        Time.timeScale = 0;
    }
}
