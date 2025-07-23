using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Do
{
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] private EffectManager effectManager;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        public bool IsActive { get; private set; }

        public void ShowEffectText(string content, Vector3 position, int size, Callback complete, bool isNut)
        {
            IsActive = true;
            gameObject.SetActive(true);
            textMeshProUGUI.text = content;
            textMeshProUGUI.fontSize = size;
            textMeshProUGUI.fontMaterial = new Material(textMeshProUGUI.fontMaterial);
            textMeshProUGUI.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.4f);
            textMeshProUGUI.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
            if (isNut)
            {

                transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x, position.y + 150);
                //transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 1300 * GameConfig.Instance.RatioScaleScreen * GameConfig.Instance.RatioScaleScreen);
                //position = new Vector3 (transform.position.x, transform.position.y - 1.5f, transform.position.z + 1);
                position.y += 300f;
                transform.GetComponent<RectTransform>().DOAnchorPosY(position.y, 0.75f).SetEase(Ease.OutSine).OnComplete(delegate
                {
                    IsActive = false;
                    gameObject.SetActive(false);
                    complete?.Invoke();
                });
            }
            else
            {
                transform.position = new Vector3(position.x, position.y + 2f, position.z - 1);
                position.y += 3f;
                transform.DOMoveY(position.y, 0.75f).SetEase(Ease.OutSine).OnComplete(delegate
                {
                    IsActive = false;
                    gameObject.SetActive(false);
                    complete?.Invoke();
                });
            }

        }
    }
}
