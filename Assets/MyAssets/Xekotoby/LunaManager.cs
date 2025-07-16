
using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using DG.Tweening;

public class LunaManager : MonoBehaviour
{

    public static LunaManager instace;


    public Camera cameraGameplay;
    public Camera cameraUi;
    public GameObject bg;
    public bool isLandscape;
    public Transform winpopup;
    private float lastScreenWidth;
    private float lastScreenHeight;
    public Transform Rect14; private void Awake()
    {
        if (instace == null)
        {
            instace = this;

        }

        if (Screen.height >= Screen.width)
        {
            // // Debug.LogError("sssss");
            isLandscape = false;

        }
        else
        {
            // // Debug.LogError("ssss2s");
            isLandscape = true;


        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        CheckAndApplyOrientation();
    }

    public void CheckAndApplyOrientation()
    {
        bool isLandscape = Screen.width > Screen.height;

        if (isLandscape)
        {
            float aspect = (float)Screen.width / Screen.height;

            ZoomInCamera(GetOrthoSizeLandscape(aspect) - 0.3f);
            LevelManager.Instance.UpdateScrew(true);
            LevelManager.Instance.UpdateScrewHolder(true);
            bg.transform.localScale = new Vector3(3.5f, 2.8f, 3.5f);
            winpopup.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Rect14.localPosition = new Vector3(0f, 1.5f, 0f);
        }
        else
        {
            // Debug.LogError("CheckAndApplyOrientation");
            ZoomInCamera(14.5f);
            LevelManager.Instance.UpdateScrew(false);
            LevelManager.Instance.UpdateScrewHolder(false);
            bg.transform.localScale = new Vector3(2.5f, 2.8f, 2.5f);
            winpopup.localScale = Vector3.one;
            Rect14.localPosition = new Vector3(0f, 1.8f, 0f);

        }
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            CheckAndApplyOrientation();
        }
    }

    private float size1;
    private float size2;

    void ZoomInCamera(float target1)
    {

        float start1 = cameraGameplay.orthographicSize;
        float start2 = cameraUi.orthographicSize;

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
        Debug.Log("Play");
        Luna.Unity.Playable.InstallFullGame();
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
