using System.Collections.Generic;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public class TextEffectManager : SingletonLate<TextEffectManager>
    {
        public float speed;
        
        [SerializeField] private TextEffect prefab;

        private readonly List<TextEffect> _effectTexts = new List<TextEffect>();

        public void PlayEffectText(string content, Vector3 position = default(Vector3), int size = 5, Callback complete = null)
        {
            var effectText = GetEffectText();
            effectText.ShowEffectText(content, position, size, complete, false);
        }

        private TextEffect GetEffectText()
        {
            foreach (var effectText in _effectTexts)
            {
                if (!effectText.IsActive)
                    return effectText;
            }

            var newEffectText = Instantiate(prefab, transform);
            newEffectText.name = "Effect Text";
            _effectTexts.Add(newEffectText);
            return newEffectText;
        }
    }
}
