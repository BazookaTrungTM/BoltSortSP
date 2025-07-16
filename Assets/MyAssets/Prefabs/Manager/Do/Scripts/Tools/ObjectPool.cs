using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public class ObjectPool : MonoBehaviour
    {
        private ObjectSource _objectSource;
        private List<GameObject> PooledObjects;
        public void Initialize(ObjectSource source)
        {
            _objectSource = source;
            PooledObjects = new List<GameObject>();
            if (_objectSource.state == PoolState.Waiting)
                return;
            CreatePool();
        }

        public void DisableAllObject()
        {
            foreach (var go in PooledObjects.Where(go => go.activeSelf))
            {
                go.SetActive(false);
            }
        }

        public GameObject GetPooledObject()
        {
            if (_objectSource.state == PoolState.Waiting)
            {
                _objectSource.state = PoolState.Instance;
                CreatePool();
                Debug.Log("Create Waiting Pool " + _objectSource.id);
                return PooledObjects[0];
            }
            foreach (var go in PooledObjects.Where(go => !go.activeSelf))
            {
                return go;
            }
            var indexToReturn = PooledObjects.Count;
            CreateObjectInPool();
            return PooledObjects[indexToReturn];
        }

        public bool CheckPoolObjectActive()
        {
            return PooledObjects.Any(go => go.activeSelf);
        }

        private void CreatePool()
        {
            var poolLength = _objectSource.lenght > 0 ? _objectSource.lenght : 1;
            for (var i = 0; i < poolLength; i++)
            {
                CreateObjectInPool();
            }
        }

        private void CreateObjectInPool()
        {
            var go = _objectSource.prefab == null ? new GameObject(this.name + " PooledObject") : Instantiate(_objectSource.prefab);
            go.SetActive(false);
            PooledObjects.Add(go);
            go.transform.parent = transform;
        }
    }

    [Serializable]
    public class ObjectSource
    {
        public PoolId id;
        public GameObject prefab;
        public int lenght;
        public PoolState state;
    }

    public enum PoolState
    {
        Instance,
        Waiting
    }
}