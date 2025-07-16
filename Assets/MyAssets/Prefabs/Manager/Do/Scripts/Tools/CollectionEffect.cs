using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public class CollectionEffect : MonoBehaviour
    {
        public new string name;
        [Range(0.02f, 0.2f)] [SerializeField] private float duration = 0.1f;
        [SerializeField] private CollectionEffectManger collectionEffectManger;
        [SerializeField] private List<Animator> animators;

        private List<Vector2> _between = new List<Vector2>();
        private float _count;
        private Vector2 _startScale;
        private Vector2 _target, _scale;
        private IEnumerator _routine;
        private readonly int _play = Animator.StringToHash("Play");
        private Callback _eventStep, _complete;
        private WaitForSeconds _waitForSeconds;
        public bool IsActive { get; private set; }

        private void Awake()
        {
            _waitForSeconds = Yield.GetTime(duration);
        }

        public void ShowEffect(Vector3 origin, Vector3 target, List<Vector2> between, Callback update, Callback complete, int count, float scale)
        {
            transform.position = origin;
            _startScale = transform.GetChild(0).localScale;
            _count = count;
            _target = target;
            _between = between;
            _eventStep = update;
            _complete = complete;
            _scale = _startScale;
            _scale.x += scale;
            _scale.y += scale;
            IsActive = true;
            gameObject.SetActive(true);
            _routine = InstanceAllObject();
            StartCoroutine(_routine);
        }

        private IEnumerator InstanceAllObject()
        {
            var current = 0;
            AudioManager.instance.PlaySFX(SoundType.Coin_Collect);
            while (current < _count)
            {
                var sort = _count - current;
                var child = transform.GetChild(current);
                child.gameObject.SetActive(true);
                Move(child, _between[current], sort <= 1);
                if (Mathf.Abs(_scale.x - _startScale.x) > 0)
                    Scale(child);
                if (animators.Count > 0)
                {
                    animators[current].SetTrigger(_play);
                    animators[current].speed = 1 / collectionEffectManger.speed;
                }
                current++;
                if (current < _count)
                    yield return _waitForSeconds;
            }
        }

        private void Move(Transform child, Vector3 between, bool isLast)
        {
            var speed = collectionEffectManger.speed;
            child.DOMove(between, speed / 4).SetEase(Ease.OutSine).OnComplete(delegate
            {
                child.DOMove(_target, speed / 2).SetDelay(speed / 4).SetEase(Ease.InSine).OnComplete(delegate
                {
                    _eventStep?.Invoke();
                    if (isLast)
                        DisableAllObject();
                });
            });
        }

        private void Scale(Transform child)
        {
            child.DOScale(_scale, collectionEffectManger.speed / 4).SetEase(Ease.InSine);
        }

        private void DisableAllObject()
        {
            _complete?.Invoke();
            gameObject.SetActive(false);
            for (var i = 0; i < _count; i++)
            {
                var child = transform.GetChild(i);
                child.localPosition = Vector3.zero;
                child.localScale = _startScale;
                child.gameObject.SetActive(false);
            }
            IsActive = false;
            _routine = null;
        }
    }
}
