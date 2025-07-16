using System;
using System.Collections;
using System.Collections.Generic;
using Do.Scripts.Tools;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

using Spine.Unity;

namespace Do.Scripts.Loading
{
    public class LoadingManager : MonoBehaviour
    {

        #region Variable

        private static LoadingManager instance;

        private const string SCENE_LOADING = "Loading";
        public const string SCENE_PLUGINS = "Plugins";
        public const string SCENE_HOME = "Home";
        public const string SCENE_GAME = "Game";

        public static LoadingManager Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                //#if UNITY_EDITOR
                //                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Do/Source/LoadingManager.prefab");
                //                instance = Instantiate(prefab.GetComponent<LoadingManager>());
                //                DontDestroyOnLoad(instance);
                //#endif
                return instance;
            }
        }
        [Header("UI")][SerializeField] private GameObject canvasLoading;
        [SerializeField] private Image sliderLoading;

        [SerializeField] private TextMeshProUGUI text, contentText;
        private int _textCount;
        private string _content;
        private Tween _tween;
        private IEnumerator _routineTitle;
        private static bool _loadingFake;
        private readonly List<Action> _earlyEvents = new List<Action>();
        private readonly List<Action> _lateEvents = new List<Action>();
        private readonly List<Action> _closeEvents = new List<Action>();

        [SerializeField] private List<LoadingContent> contents;
        public SkeletonGraphic titleSpine;
        private int _pickContent;

        #endregion

        public bool IsLoading => SceneManager.GetActiveScene().name == SCENE_LOADING;

        private void Awake()
        {
            if (instance != null)
                return;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!_loadingFake || _tween != null)
                return;
#if UNITY_EDITOR
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Do/Source/Do_Manager.prefab");
            Instantiate(prefab);
#endif
            FakeLoading();
        }

        private void FakeLoading()
        {
#if UNITY_EDITOR
            Debug.Log("Loading Fake");
#endif
            _routineTitle = PlayTitleText();
            StartCoroutine(_routineTitle);
            PlayAnimationOnce();
        }

        public void Pause()
        {
            _tween?.Pause();
#if UNITY_EDITOR
            // Debug.LogError("Pause");
#endif
        }

        public void Continue()
        {
            _tween?.Play();
#if UNITY_EDITOR
            // Debug.LogError("Continue");
#endif
        }

        private void LoadingProgress(float target, float delay, float duration, Action complete)
        {
            //Debug.Log($"Loading : {target} {duration}");
            _tween = sliderLoading.DOFillAmount(target, duration).SetEase(Ease.Linear).SetDelay(delay).OnComplete(() =>
            {
                complete?.Invoke();
            });
        }

        /// <summary>
        /// /////////
        /// </summary>

        #region LoadOnceScene

        public void LoadScene(string nameScene, float totalTime = 0.5f)
        {
            PlayAnimationOnce(nameScene, totalTime);
            _routineTitle = PlayTitleText();
            System.GC.Collect();
            StartCoroutine(_routineTitle);
            // AdsManager.Instance.HideBanner();
            // AdsManager.Instance.ShowBannerMRec();
            // RegisterLateEvent(AdsManager.Instance.HideBannerMRec);
        }

        private void PlayAnimationOnce(string nameScene = "", float totalTime = 2f)
        {
            canvasLoading.SetActive(true);
            sliderLoading.fillAmount = 0;
            var target = Random.Range(0.05f, 0.1f);
            LoadingProgress(target, 0.1f * totalTime, (target - sliderLoading.fillAmount) * totalTime, () =>
            {
                target = Random.Range(0.85f, 0.9f);
                LoadingProgress(target, 0, (target - sliderLoading.fillAmount) * totalTime, () =>
                {
                    target = 1f;
                    LoadingProgress(1f, 0f, (target - sliderLoading.fillAmount) * totalTime, () =>
                    {
                        if (!string.IsNullOrEmpty(nameScene))
                            SceneManager.LoadScene(nameScene);
                        DOVirtual.DelayedCall(0.3f, () =>
                        {
                            canvasLoading.SetActive(false);
                            CallCloseEvents();
                            _tween = null;
                        });
                        CallLateEvents();
                    });
                    CallEarlyEvents();
                });
            });
        }

        #endregion

        /// <summary>
        /// /////////
        /// </summary>

        #region LoadDoubleScene

        public void LoadScene(string scene1, string scene2, float scene1Time = 2f, float scene2Time = 2f)
        {
            // _listLoadData.Clear();
            // _listEndLoad.Clear();
            PlayAnimationDouble(scene1, scene2, scene1Time, scene2Time);
            _routineTitle = PlayTitleText();
            StartCoroutine(_routineTitle);
        }

        private void PlayAnimationDouble(string scene1, string scene2, float scene1Time = 2f, float scene2Time = 2f)
        {
            canvasLoading.SetActive(true);
            sliderLoading.fillAmount = 0;
            var middle = scene1Time / (scene1Time + scene2Time);
            var target = 0.315f * middle;
            LoadingProgress(target, 0.1f * scene1Time, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
            {
                if (!string.IsNullOrEmpty(scene1))
                    SceneManager.LoadScene(scene1);
                target = Random.Range(0.7f, 0.9f) * middle;
                LoadingProgress(target, 0, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
                {
                    target = middle;
                    LoadingProgress(middle, 0f, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
                    {
                        target = Random.Range(0.05f, 0.1f) * (1 - middle) + middle;
                        LoadingProgress(target, 0.1f * scene2Time, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
                        {
                            target = Random.Range(0.7f, 0.9f) * (1 - middle) + middle;
                            LoadingProgress(target, 0, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
                            {
                                if (!string.IsNullOrEmpty(scene2))
                                    SceneManager.LoadScene(scene2);
                                target = 1f;
                                LoadingProgress(target, 0f, (target - sliderLoading.fillAmount) * (scene1Time + scene2Time), () =>
                                {
                                    DOVirtual.DelayedCall(0.3f, () =>
                                    {
                                        canvasLoading.SetActive(false);
                                        CallCloseEvents();
                                        _tween = null;
                                    });
                                    CallLateEvents();
                                });
                                CallEarlyEvents();
                            });
                        });
                        CallLateEvents();
                    });
                    CallEarlyEvents();
                });
            });
        }

        #endregion

        /// <summary>
        /// /////////
        /// </summary>


        #region CallBackEvent

        public void RegisterEarlyEvent(Action callback)
        {
            _earlyEvents.Add(callback);
        }

        public void RegisterLateEvent(Action callback)
        {
            _lateEvents.Add(callback);
        }

        public void RegisterCloseEvent(Action callback)
        {
            _closeEvents.Add(callback);
        }

        private void CallEarlyEvents()
        {
            //Debug.Log("Loading : Call Early Events");
            if (_earlyEvents.Count <= 0)
                return;
            var subList = new List<Action>();
            subList.AddRange(_earlyEvents);
            _earlyEvents.Clear();
            foreach (var callback in subList)
            {
                callback?.Invoke();
            }
        }

        private void CallLateEvents()
        {
            //Debug.Log("Loading : Call Late Events");
            if (_lateEvents.Count <= 0)
                return;
            var subList = new List<Action>();
            subList.AddRange(_lateEvents);
            _lateEvents.Clear();
            foreach (var callback in subList)
            {
                callback?.Invoke();
            }
        }

        private void CallCloseEvents()
        {
            //Debug.Log("Loading : Call Close Events");
            if (_closeEvents.Count <= 0)
                return;
            var subList = new List<Action>();
            subList.AddRange(_closeEvents);
            _closeEvents.Clear();
            foreach (var callback in subList)
            {
                callback?.Invoke();
            }
        }

        #endregion

        private IEnumerator PlayTitleText()
        {
            yield return Yield.EndFrame;
            var yield = Yield.GetTime(0.5f);
            _textCount = 0;
            _pickContent = -1;
            // RandomContent();
            _content = "Loading";
            // _content = LanguageManager.Instance.Language == Language.Eng ? "Loading" : "Đang tải";
            while (canvasLoading.activeSelf)
            {
                switch (_textCount)
                {
                    case 0:
                        text.text = _content + ".";
                        _textCount++;
                        break;
                    case 1:
                        text.text = _content + "..";
                        _textCount++;
                        break;
                    case 2:
                        text.text = _content + "...";
                        _textCount = 0;
                        break;
                }

                yield return yield;
            }
        }

        private void RandomContent()
        {
            var list = new List<int>();
            for (var i = 0; i < contents.Count; i++)
            {
                if (i != _pickContent)
                    list.Add(i);
            }

            _pickContent = list[Random.Range(0, list.Count)];
            contentText.text = LanguageManager.Instance.Language == GameLanguage.Eng
                ? contents[_pickContent].Eng
                : contents[_pickContent].Vie;
        }
        public void CMPStarted()
        {
            //isCMPActive = true;
            //Debug.Log("CMP started. Pausing loading.");
            //Pause();
        }

        // CMP kết thúc
        public void CMPFinished()
        {
            //isCMPActive = false;
            //Debug.Log("CMP finished. Resuming loading.");
            //Continue();
        }
    }

    [Serializable]
    public class LoadingContent
    {
        public string Eng;
        public string Vie;
    }
}
