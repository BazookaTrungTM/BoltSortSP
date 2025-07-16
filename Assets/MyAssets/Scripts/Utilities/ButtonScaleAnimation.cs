using DG.Tweening;
using Do;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonScaleAnimation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] [Range(0.05f, 0.2f)] private float scale = 0.1f;
    [SerializeField] private UnityEvent onPointerClick;
    [SerializeField] private UnityEvent onPointerDown;
    [SerializeField] private UnityEvent onPointerUp;
    
    public UnityEvent OnClick => onPointerClick;
        
    public Vector3 _origin, _target;

    private Tween _tween;
    private void Start()
    {
        Assign();
    }
    public void Assign()
    {
        _origin = transform.localScale;
        _target = _origin;
        //_target.x -= _origin.x > 0 ? scale : -scale;
        //_target.y -= _origin.y > 0 ? scale : -scale;
        _target.x += scale;
        _target.y += scale;
        _target.z += scale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _tween?.Kill();
        //if (GetComponent<DOTweenAnimation>() != null)
        //{
        //    GetComponent<DOTweenAnimation>().DOPause();
        //}

        DOTween.Pause(gameObject);
        Assign();
        onPointerDown?.Invoke();
        _tween = transform.DOScale(_target, 0.1f).OnComplete(delegate { _tween = null; });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _tween?.Kill();
        onPointerUp?.Invoke();
        _tween = transform.DOScale(_origin, 0.1f).OnComplete(delegate { _tween = null; });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        VibrationUtil.Vibrate(30);
        AudioManager.instance.PlaySFX(SoundType.Click_UI);
        onPointerClick?.Invoke();
    }

    private void OnDisable()
    {
        if (_tween == null)
            return;
        transform.localScale = _origin;
        _tween.Kill();
        _tween = null;
    }
}