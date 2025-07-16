using UnityEngine;

namespace Do
{
    public static class MyPref
    {
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(Application.identifier + key);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(Application.identifier + key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(Application.identifier + key, value);
            PlayerPrefs.Save();
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(Application.identifier + key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(Application.identifier + key, value);
            PlayerPrefs.Save();
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(Application.identifier + key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(Application.identifier + key, value);
            PlayerPrefs.Save();
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(Application.identifier + key, defaultValue ? 1 : 0) == 1;
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(Application.identifier + key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(Application.identifier + key);
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}