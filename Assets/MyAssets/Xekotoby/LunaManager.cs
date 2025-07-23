
using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using DG.Tweening;
using UnityEngine.UI;

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
    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
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
            ZoomInCamera(14);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            canvasScaler.matchWidthOrHeight = 0;
            Vector2 sizeBoxBooster = boxBooster.sizeDelta;
            sizeBoxBooster.y = 540;
            boxBooster.sizeDelta = sizeBoxBooster;
        }
        if (screenAspect >= 0.55f && screenAspect < 0.65f)
        {
            // Debug.LogError("CheckAndApplyOrientation");
            ZoomInCamera(12);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            canvasScaler.matchWidthOrHeight = 0;
            Vector2 sizeBoxBooster = boxBooster.sizeDelta;
            sizeBoxBooster.y = 540;
            boxBooster.sizeDelta = sizeBoxBooster;
        }
        else if (screenAspect >= 0.65f && screenAspect < 0.8f)
        {
            ZoomInCamera(11);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            canvasScaler.matchWidthOrHeight = 1;
            Vector2 sizeBoxBooster = boxBooster.sizeDelta;
            sizeBoxBooster.y = 540;
            boxBooster.sizeDelta = sizeBoxBooster;
        }
        else if (screenAspect >= 0.80f && screenAspect < 1)
        {
            ZoomInCamera(11);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            canvasScaler.matchWidthOrHeight = 1;
            Vector2 sizeBoxBooster = boxBooster.sizeDelta;
            sizeBoxBooster.y = 540;
            boxBooster.sizeDelta = sizeBoxBooster;
        }
        else if (screenAspect >= 1)
        {
            ZoomInCamera(11);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            canvasScaler.matchWidthOrHeight = 1;
            Vector2 sizeBoxBooster = boxBooster.sizeDelta;
            sizeBoxBooster.y = 540;
            boxBooster.sizeDelta = sizeBoxBooster;
        }
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
