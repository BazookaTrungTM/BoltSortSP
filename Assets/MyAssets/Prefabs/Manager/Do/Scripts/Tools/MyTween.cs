using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace Do
{
    public class MyTween : Singleton<MyTween>
    {
        private readonly List<MyTweenStep> _myTweenSteps = new List<MyTweenStep>();
        private float _tweenFloat;

        protected override void Awake()
        {
            base.Awake();
            _myTweenSteps.Add(transform.GetChild(0).GetComponent<MyTweenStep>());
        }

        public void MyTween_Float(float Duration, Callback Complete)
        {
            GetTweenStep().My_Tween_Float(Duration, 0, Complete);
        }

        public void MyTween_Float(float Duration, float Delay, Callback Complete)
        {
            GetTweenStep().My_Tween_Float(Duration, Delay, Complete);
        }

        public DG.Tweening.Tween DoTween_Float(float Duration, float Delay = 0, Action Complete = null)
        {
            _tweenFloat = 0;
            var tween = DOTween.To(() => _tweenFloat, x => _tweenFloat = x, 1, Duration).SetDelay(Delay).OnComplete(delegate
            {
                Complete?.Invoke();
            });
            return tween;
        }

        private MyTweenStep GetTweenStep()
        {
            foreach (var myTweenStep in _myTweenSteps)
            {
                if (!myTweenStep.IsTween)
                    return myTweenStep;
            }
            var newTween = Instantiate(_myTweenSteps[0], transform);
            _myTweenSteps.Add(newTween);
            return newTween;
        }

        public void KillAll()
        {
            foreach (var tween in _myTweenSteps.Where(tween => tween.IsTween))
            {
                tween.Kill();
            }
        }
    }
}
