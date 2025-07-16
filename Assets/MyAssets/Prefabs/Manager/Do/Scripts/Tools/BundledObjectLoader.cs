//using System;
//using System.Collections;
//using UnityEngine;
//using Addre
//using Object = UnityEngine.Object;

//namespace Do
//{
//    public class BundledObjectLoader : SingletonLate<BundledObjectLoader>
//    {
//        [SerializeField] private GameObject canvas;
//        public void LoadFromFile(AssetReference assetReference, Action<GameObject> onComplete)
//        {
//            StartCoroutine(LoadFile(assetReference, onComplete, false));
//        }

//        public void LoadFromFileWithLoading(AssetReference assetReference, Action<GameObject> onComplete)
//        {
//            StartCoroutine(LoadFile(assetReference, onComplete, true));
//        }

//        private IEnumerator LoadFile(AssetReference assetReference, Action<GameObject> onComplete, bool loading)
//        {
//            if (loading)
//                canvas.SetActive(true);
//            var async = Addressables.LoadAssetAsync<GameObject>(assetReference);
//            yield return async;
//            if (loading)
//                canvas.SetActive(false);
//            var result = async.Result;
//            if (result == null)
//            {
//                // Debug.LogError("Fail to load " + assetReference.Asset.name);
//                yield break;
//            }
//            onComplete(result);
//            // Debug.LogError("Complete load " + result.name);
//            Addressables.ClearDependencyCacheAsync(assetReference);
//        }


//        public void LoadObjectFromFile(AssetReference assetReference, Action<Object> onComplete)
//        {
//            StartCoroutine(LoadFile(assetReference, onComplete, false));
//        }

//        public void LoadObjectFromFileWithLoading(AssetReference assetReference, Action<Object> onComplete)
//        {
//            StartCoroutine(LoadFile(assetReference, onComplete, true));
//        }

//        private IEnumerator LoadFile(AssetReference assetReference, Action<Object> onComplete, bool loading)
//        {
//            if (loading)
//                canvas.SetActive(true);
//            var async = Addressables.LoadAssetAsync<Object>(assetReference);
//            yield return async;
//            if (loading)
//                canvas.SetActive(false);
//            var result = async.Result;
//            if (result == null)
//            {
//                // Debug.LogError("Fail to load " + assetReference.Asset.name);
//                yield break;
//            }
//            onComplete(result);
//            // Debug.LogError("Complete load " + result.name);
//            Addressables.ClearDependencyCacheAsync(assetReference);
//            Debug.Log("asdasdasdasdasdasdasdas");
//        }
//    }
//}
