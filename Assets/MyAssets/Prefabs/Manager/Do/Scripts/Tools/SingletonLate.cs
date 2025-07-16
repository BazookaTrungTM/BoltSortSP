using UnityEngine;

namespace Do
{
    public class SingletonLate<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Start()
        {
            Instance = GetComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}
