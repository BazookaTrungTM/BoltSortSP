using UnityEngine;

namespace Do
{
    public class MyTweenStep : MonoBehaviour
    {
        public bool IsTween { get; private set; }

        private float _duration, _delay;
        private bool _isDelay;
        private Callback _complete;
        private float _value;

        public void My_Tween_Float(float Duration = 0f, float Delay = 0f, Callback Complete = null)
        {
            _duration = Duration;
            _delay = Delay;
            _complete = Complete;
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _isDelay = _delay > 0;
            IsTween = true;
        }

        private void Update()
        {
            if (!IsTween)
                return;
            var deltaTime = Time.deltaTime;
            if (_isDelay)
            {
                _delay -= deltaTime;
                if (_delay <= 0)
                    _isDelay = false;
            }
            else
            {
                _duration -= deltaTime;
                if(_duration <= 0)
                    Disable();
            }
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            IsTween = false;
            _complete?.Invoke();
        }

        public void Kill()
        {
            IsTween = false;
            gameObject.SetActive(false);
            _complete = null;
        }
    }
}
