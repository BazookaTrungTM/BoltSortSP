using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Do.Scripts.Tools
{
    public class CollectionEffectManger : SingletonLate<CollectionEffectManger>
    {
        public float speed;
        [Range(0.5f, 5f)] [SerializeField] private float range = 2f;
        [Range(0.5f, 90f)] [SerializeField] private float rotate = 45f;
        [SerializeField] private CollectionEffect prefabCoinEffect, prefabGemEffect;
        [SerializeField] private List<CollectionEffect> listEffectCoin, listEffectGem;
        [SerializeField] Camera cameraUI;

        private Transform _coinParent, _gemParent;
        private CollectionEffect _collectionEffect;

        protected override void Start()
        {
            base.Start();
            _coinParent = new GameObject("Golds").transform;
            _coinParent.SetParent(transform);
            _gemParent = new GameObject("Gems").transform;
            _gemParent.SetParent(transform);
        }
        
        public void PlayEffectGold(Vector3 origin, Vector3 target, Callback callbackStep, Callback complete = null, int count = 5, float scale = 0.1f)
        {
            _collectionEffect = GetEffectCoin();
            var between = GetBetween(origin, target, count);
            _collectionEffect.ShowEffect(origin, target, between, callbackStep, complete, count, scale);
        }

        private CollectionEffect GetEffectCoin()
        {
            foreach (var effectCoin in listEffectCoin)
            {
                if (!effectCoin.IsActive)
                    return effectCoin;
            }

            var newEffectCoin = Instantiate(prefabCoinEffect, _coinParent);
            newEffectCoin.gameObject.name = "Effect " + newEffectCoin.name;
            newEffectCoin.transform.localScale = Vector3.one;
            listEffectCoin.Add(newEffectCoin);
            return newEffectCoin;
        }
        
        public void PlayEffectGem(Vector2 origin, Vector2 target, Callback callbackStep, Callback complete = null, int count = 5, float scale = 0.1f)
        {
            // AudioManager.Instance.Play(SoundType.Coin);
            _collectionEffect = GetEffectGem();
            var between = GetBetween(origin, target, count);
            _collectionEffect.ShowEffect(origin, target, between, callbackStep, complete, count, scale);
        }

        private CollectionEffect GetEffectGem()
        {
            foreach (var effectGem in listEffectGem)
            {
                if (!effectGem.IsActive)
                    return effectGem;
            }

            var newEffectGem = Instantiate(prefabGemEffect, _gemParent);
            newEffectGem.gameObject.name = "Effect " + newEffectGem.name;
            listEffectGem.Add(newEffectGem);
            return newEffectGem;
        }

        private List<Vector2> GetBetween(Vector2 origin, Vector2 target, int count)
        {
            var between = new List<Vector2>();
            var direction = origin - target;
            direction.Normalize();
            direction *= range;
            for (var i = 0; i < count; i++)
            {
                between.Add(origin + MyMathf.RotateVector2(direction, Random.Range(-rotate, rotate)));
            }
            return between;
        }
    }
}
