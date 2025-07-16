using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public enum PoolId
    {
        Buff,
        Defense,
        Hit,
        Bullet,
        Dash,
        Blink,
        BulletHit
    }
    public class ObjectPoolManager : SingletonLate<ObjectPoolManager>
    {
        private readonly Dictionary<PoolId, ObjectPool> _dictionary = new Dictionary<PoolId, ObjectPool>();

        [SerializeField] private List<ObjectSource> objectSources = new List<ObjectSource>();

        private void Initialized()
        {
            if (objectSources.Count <= 0)
                return;
            foreach (var objectSource in objectSources)
            {
                InstancePool(objectSource);
            }
        }

        protected override void Start()
        {
            base.Start();
            Initialized();
        }

        private void InstancePool(ObjectSource source)
        {
            if (_dictionary.ContainsKey(source.id))
            {
                // Debug.LogError("Error : Contains Key " + source.id);
                return;
            }
            var go = new GameObject(source.id + " Pools");
            var objectPool = go.AddComponent<ObjectPool>();
            go.transform.parent = gameObject.transform;
            objectPool.Initialize(source);
            _dictionary.Add(source.id, objectPool);
        }

        public GameObject GetPooledObject(PoolId id)
        {
            if (_dictionary.ContainsKey(id))
                return _dictionary[id].GetPooledObject();
            // Debug.LogError("Cannot Find Pool " + id);
            return null;
        }

        public void DisableAllObject()
        {
            var list = _dictionary.Values.ToList();
            foreach (var objectPool in list.Where(objectPool => objectPool != null))
            {
                objectPool.DisableAllObject();
            }
        }
    }
}
