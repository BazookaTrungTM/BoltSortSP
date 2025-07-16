// using I2.Loc;
using UnityEngine;

namespace Do.Scripts.Tools
{
    public class LanguageManager : SingletonLate<LanguageManager>
    {
        private const string KEY_LANGUAGE = "KEY_LANGUAGE";
        public GameLanguage Language { get; private set; }

        private string _english, _vietnamese;
        public Callback OnChangeLanguage { private get; set; }

        protected override void Start()
        {
            base.Start();
            InitLanguage();
        }

        private void InitLanguage()
        {
            _english = SystemLanguage.English.ToString();
            _vietnamese = SystemLanguage.Vietnamese.ToString();
            var saveLanguage = MyPref.GetInt(KEY_LANGUAGE);
            if (saveLanguage == 0)
            {
                if (Application.systemLanguage == SystemLanguage.Vietnamese)
                {
                    MyPref.SetInt(KEY_LANGUAGE, 2);
                    Language = GameLanguage.Vie;
                    SetLanguageLocalize(_vietnamese);
                }
                else
                {
                    MyPref.SetInt(KEY_LANGUAGE, 1);
                    Language = GameLanguage.Eng;
                    SetLanguageLocalize(_english);
                }
            }
            else if (saveLanguage == 1)
            {
                Language = GameLanguage.Eng;
                SetLanguageLocalize(_english);
            }
            else if (saveLanguage == 2)
            {
                Language = GameLanguage.Vie;
                SetLanguageLocalize(_vietnamese);
            }
        }

        public void SetLanguage_English()
        {
            if (Language == GameLanguage.Eng)
                return;
            MyPref.SetInt(KEY_LANGUAGE, 1);
            SetLanguageLocalize(_english);
            Language = GameLanguage.Eng;
            OnChangeLanguage?.Invoke();
        }

        public void SetLanguage_Vietnamese()
        {
            if (Language == GameLanguage.Vie)
                return;
            MyPref.SetInt(KEY_LANGUAGE, 2);
            SetLanguageLocalize(_vietnamese);
            Language = GameLanguage.Vie;
            OnChangeLanguage?.Invoke();
        }

        private void SetLanguageLocalize(string language)
        {
            Debug.Log("Language Localize : " + language);
            // LocalizationManager.CurrentLanguage = language;
        }
    }

    public enum GameLanguage
    {
        Eng,
        Vie
    }
}
