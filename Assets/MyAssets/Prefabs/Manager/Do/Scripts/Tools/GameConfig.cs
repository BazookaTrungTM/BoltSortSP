using System;
using DG.Tweening;
using Do.Scripts.Loading;

//using Plugins;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Do
{
    public class GameConfig : Singleton<GameConfig>
    {
        public bool isFirst;
        private enum ScreenType
        {
            Horizontal,
            Vertical
        }
        [SerializeField] private ScreenType screenType;

        private Vector2 screenSize = Vector2.zero;
        private float ratioScaleScreen = 0f;

        public Vector2 ScreenSize
        {
            get
            {
                if (screenSize.x > 0)
                    return screenSize;
                screenSize.x = Screen.width;
                screenSize.y = Screen.height;
                return screenSize;
            }
            private set => screenSize = value;
        }

        public float RatioScaleScreen
        {
            get
            {
                if (ratioScaleScreen > 0)
                    return ratioScaleScreen;
                ratioScaleScreen = GetRatio();
                return ratioScaleScreen;
            }
            private set => ratioScaleScreen = value;
        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Initialize();



            //Application.targetFrameRate = 60;



            //xekotoby
            //if ( !LoadingManager.Instance.IsLoading)
            //    return;
            ////updateofjura
            //if (RootManager.Instance.isTestAD)
            //{
            //    if (GameUtils.Level == 1)
            //        //LoadingManager.Instance.LoadScene(LoadingManager.SCENE_PLUGINS, LoadingManager.SCENE_GAME, 1, 1);
            //        LoadingManager.Instance.LoadScene(LoadingManager.SCENE_GAME, 2);

            //    else
            //        //LoadingManager.Instance.LoadScene(LoadingManager.SCENE_PLUGINS, LoadingManager.SCENE_HOME, 1, 1f);
            //        LoadingManager.Instance.LoadScene(LoadingManager.SCENE_HOME, 2);

            //}
            //else
            //{
            //    if (GameUtils.Level == 1)
            //        //LoadingManager.Instance.LoadScene(LoadingManager.SCENE_PLUGINS, LoadingManager.SCENE_GAME, 1, 6f);
            //        LoadingManager.Instance.LoadScene(LoadingManager.SCENE_GAME, 7f);
            //    else
            //        //LoadingManager.Instance.LoadScene(LoadingManager.SCENE_PLUGINS, LoadingManager.SCENE_HOME, 1, 6f);
            //        LoadingManager.Instance.LoadScene(LoadingManager.SCENE_HOME, 7f);
            //}
            ////LoadingManager.Instance.titleSpine.gameObject.SetActive(false);
            ///





            //DOVirtual.DelayedCall(1, () =>
            //{
            //    LoadingManager.Instance.titleSpine.gameObject.SetActive(true);
            //    LoadingManager.Instance.particle.Play();

            //xekotoby
            //LoadingManager.Instance.titleSpine.AnimationState.SetAnimation(0, "appear", false);
            ////});
            //DOVirtual.DelayedCall(1.233f, () =>
            //{
            //    LoadingManager.Instance.titleSpine.AnimationState.SetAnimation(0, "idle", true);
            //});

#if UNITY_EDITOR
            //LoadingManager.Instance.RegisterEarlyEvent(delegate
            //{
            //    //RootManager.Instance.isTestAds = true;
            //});
#endif
        }

        private void Initialize()
        {
            RatioScaleScreen = GetRatio();
            Debug.Log("Screen Size -- " + ScreenSize + " -- " + RatioScaleScreen);
#if UNITY_EDITOR
            Application.targetFrameRate = -1;
            Debug.Log("Editor");
#elif UNITY_ANDROID
            Application.targetFrameRate = 60;
            Debug.Log("Android");
#endif
        }

        private float GetRatio()
        {
            if (screenType == ScreenType.Vertical)
            {
                var deviceRatio = ScreenSize.x / ScreenSize.y;
                var deviceWidth = 1080 / deviceRatio;
                return deviceWidth / 1920;
            }
            else
            {
                var deviceRatio = ScreenSize.x / ScreenSize.y;
                var deviceHeight = 1920 / deviceRatio;
                return deviceHeight / 1080;
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKey(KeyCode.P))
            {
                UnityEditor.EditorApplication.isPaused = !UnityEditor.EditorApplication.isPaused;
            }
        }
#endif
    }
}
