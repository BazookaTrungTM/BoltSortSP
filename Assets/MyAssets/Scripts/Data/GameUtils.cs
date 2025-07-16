using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GameUtils
{
    #region Static Variables
    public static bool NewInstall
    {
        get => GetDataBool(NEWINSTALL);
        set => SetDataBool(NEWINSTALL, value);
    }
    public static int Level
    {
        get => GetDataInt(LEVEL_KEY, 1);
        set => SetDataInt(LEVEL_KEY, value);
    }
    public static int Level_Type
    {
        get => GetDataInt(LEVEL_TYPE_KEY, 0);
        set => SetDataInt(LEVEL_TYPE_KEY, value);
    }
    public static int Win_Streak
    {
        get => GetDataInt(WIN_STREAK, 0);
        set => SetDataInt(WIN_STREAK, value);
    }
    public static int Times_Play_Level
    {
        get => GetDataInt(TIMES_PLAY_LEVEL, 0);
        set => SetDataInt(TIMES_PLAY_LEVEL, value);
    }
    public static int Times_Lose_Level
    {
        get => GetDataInt(TIMES_LOSE_LEVEL, 0);
        set => SetDataInt(TIMES_LOSE_LEVEL, value);
    }
    public static string Play_Type
    {
        get => GetDataString(PLAY_TYPE, "");
        set => SetDataString(PLAY_TYPE, value);
    }
    public static int Times_Give_Up
    {
        get => GetDataInt(TIMES_GIVE_UP, 0);
        set => SetDataInt(TIMES_GIVE_UP, value);
    }
    public static int Times_Keep_playing
    {
        get => GetDataInt(TIMES_KEEP_PLAYING, 0);
        set => SetDataInt(TIMES_KEEP_PLAYING, value);
    }
    public static int Undo_Booster
    {
        get => GetDataInt(BOOSTER_UNDO, 0);
        set => SetDataInt(BOOSTER_UNDO, value);
    }
    public static int Fill_Hol_Booster
    {
        get => GetDataInt(BOOSTER_FILL_HOL, 0);
        set => SetDataInt(BOOSTER_FILL_HOL, value);
    }
    public static int Add_Hol_Booster
    {
        get => GetDataInt(BOOSTER_ADD_HOL, 0);
        set => SetDataInt(BOOSTER_ADD_HOL, value);
    }
    public static int Coin
    {
        get => GetDataInt(COIN_KEY, 250);
        set => SetDataInt(COIN_KEY, value);
    }
    public static int Heart
    {
        get => GetDataInt(HEART_KEY, 5);
        set
        {
            SetDataInt(HEART_KEY, value);
            OnHeartChanged?.Invoke();
        }
    }
    public static int Level_Parent_Island
    {
        get => GetDataInt(LEVEL_PARENT_ISLAND_KEY, 0);
        set => SetDataInt(LEVEL_PARENT_ISLAND_KEY, value);
    }
    public static int Level_Child_Island
    {
        get => GetDataInt(LEVEL_CHILD_ISLAND_KEY, 0);
        set => SetDataInt(LEVEL_CHILD_ISLAND_KEY, value);
    }
    public static int Cur_Island_Point
    {
        get => GetDataInt(CURRENT_ISLAND_POINT, 0);
        set => SetDataInt(CURRENT_ISLAND_POINT, value);
    }
    public static int Cur_User_Point
    {
      //  get => GetDataInt(CURRENT_USER_POINT, RootManager.Instance.isTestCoin ? 50000 : 0);
        set => SetDataInt(CURRENT_USER_POINT, value);
    }
    public static int Cur_Feature_Index
    {
        get => GetDataInt(CURRENT_FEATURE_INDEX, 0);
        set => SetDataInt(CURRENT_FEATURE_INDEX, value);
    }
    public static List<int> Feature_Level_Unlock = new List<int>()
    {
        15, 25, 35, 45, 60, 75, 90, 110, 130
    };
    public static string Heart_Time
    {
        get => GetDataString(HEART_TIME, "");
        set => SetDataString(HEART_TIME, value);
    }
    public static string Unlimited_Heart_Time
    {
        get => GetDataString(UNLIMIT_HEART_TIME, "");
        set => SetDataString(UNLIMIT_HEART_TIME, value);
    }
    public static int TotalUnlimitedHeartTimer
    {
        get => GetDataInt(TOTAL_UNLIMIT_HEART_TIME, 0);
        set => SetDataInt(TOTAL_UNLIMIT_HEART_TIME, value);
    }
    public static string Free_Gift_Time
    {
        get => GetDataString(FREE_GIFT_TIME, "");
        set => SetDataString(FREE_GIFT_TIME, value);
    }
    public static int Free_Gift_Claim_Times
    {
        get => GetDataInt(FREE_GIFT_CLAIM_TIMES, 0);
        set => SetDataInt(FREE_GIFT_CLAIM_TIMES, value);
    }
    public static string DayOnDaily
    {
        get => GetDataString(DAY, "09/16/2000 12:00:00");
        set => SetDataString(DAY, value);
    }
    public static string DayCheckSubscription
    {
        get => GetDataString(DAY_CHECK_SUB, "09/16/2000 12:00:00");
        set => SetDataString(DAY_CHECK_SUB, value);
    }
    public static bool Is_Buy_Weekly_Bundle
    {
        get => GetDataBool(IS_BUY_WEEKLY_PACKS, false);
        set => SetDataBool(IS_BUY_WEEKLY_PACKS, value);
    }
    public static int Weekly_Bundle_Count
    {
        get => GetDataInt(WEEKLY_BUNDLE_COUNT, 0);
        set => SetDataInt(WEEKLY_BUNDLE_COUNT, value);
    }
    public static bool Is_Buy_NoAds_Bundle
    {
        get => GetDataBool(IS_BUY_NO_ADS_BUNDLE, false);
        set => SetDataBool(IS_BUY_NO_ADS_BUNDLE, value);
    }
    public static bool Is_Buy_Starter_Pack
    {
        get => GetDataBool(IS_BUY_STARTER_PACKS, false);
        set => SetDataBool(IS_BUY_STARTER_PACKS, value);
    }
    public static bool Is_Buy_Special_Offer
    {
        get => GetDataBool(IS_BUY_SPECIAL_PACKS, false);
        set => SetDataBool(IS_BUY_SPECIAL_PACKS, value);
    }
    public static string TimeStartLevel
    {
        get => GetDataString(TIME_START_LEVEL, "09/16/2000 12:00:00");
        set => SetDataString(TIME_START_LEVEL, value);
    }
    public static string TimeStartAds
    {
        get => GetDataString(TIME_START_AD, "09/16/2000 12:00:00");
        set => SetDataString(TIME_START_AD, value);
    }
    public static string TimeStartSession
    {
        get => GetDataString(TIME_START_SESSION, "09/16/2000 12:00:00");
        set => SetDataString(TIME_START_SESSION, value);
    }
    public static int Time_play
    {
        get => GetDataInt(TIME_PLAY, 0);
        set => SetDataInt(TIME_PLAY, value);
    }
    public static int Iap_Count
    {
        get => GetDataInt(IAP_COUNT, 0);
        set => SetDataInt(IAP_COUNT, value);
    }
    public static Action OnHeartChanged;

    public static bool IsRemoveAds
    {
        get => GetDataBool(IS_BUY_NO_ADS, false);
        set => SetDataBool(IS_BUY_NO_ADS, value);
    }

    public static bool IsRemoveAdsWeekly
    {
        get => GetDataBool(IS_BUY_NO_ADS_WEEKLY, false);
        set => SetDataBool(IS_BUY_NO_ADS_WEEKLY, value);
    }
    public static bool FirstTimeOpenGame
    {
        get => GetDataBool(FIRST_TIME_PLAY, true);
        set => SetDataBool(FIRST_TIME_PLAY, value);
    }
    public static bool IsOpenGame
    {
        get => GetDataBool(OPEN_GAME, true);
        set => SetDataBool(OPEN_GAME, value);
    }
    #endregion


    #region String key

    public static string LEVEL_KEY = "LEVEL_KEY";
    public static string LEVEL_TYPE_KEY = "LEVEL_TYPE_KEY";
    public static string NEWINSTALL = "NEWINSTALL";
    public static string BOOSTER_UNDO = "BOOSTER_UNDO";
    public static string BOOSTER_FILL_HOL = "BOOSTER_FILL_HOL";
    public static string BOOSTER_ADD_HOL = "BOOSTER_ADD_HOL";
    public static string COIN_KEY = "COIN_KEY";
    public static string HEART_KEY = "HEART_KEY";
    public static string LEVEL_CHILD_ISLAND_KEY = "LEVEL_CHILD_ISLAND_KEY";
    public static string LEVEL_PARENT_ISLAND_KEY = "LEVEL_PARENT_ISLAND_KEY";
    public static string CURRENT_ISLAND_POINT = "CURRENT_ISLAND_POINT";
    public static string CURRENT_USER_POINT = "CURRENT_USER_POINT";
    public static string CURRENT_FEATURE_INDEX = "CURRENT_FEATURE_INDEX";
    public static string HEART_TIME = "HeartTime";
    public static string UNLIMIT_HEART_TIME = "UnlimitedHeartTime";
    public static string TOTAL_UNLIMIT_HEART_TIME = "TotalUnlimitedHeartTimer";
    public static string FREE_GIFT_TIME = "FreeGiftTime";
    public static string FREE_GIFT_CLAIM_TIMES = "FreeGiftClaimTimes";
    public static string DAY = "Day";
    public static string DAY_CHECK_SUB = "DAY_CHECK_SUB";
    public static string IS_BUY_NO_ADS = "IS_BUY_NO_ADS";
    public static string IS_BUY_NO_ADS_WEEKLY = "IS_BUY_NO_ADS_WEEKLY";
    public static string IS_BUY_NO_ADS_BUNDLE = "IS_BUY_NO_ADS_BUNDLE";
    public static string IS_BUY_WEEKLY_PACKS = "IS_BUY_WEEKLY";
    public static string WEEKLY_BUNDLE_COUNT = "WEEKLY_BUNDLE_COUNT";
    public static string IS_BUY_STARTER_PACKS = "IS_BUY_STARTER";
    public static string IS_BUY_SPECIAL_PACKS = "IS_BUY_SPECIAL_PACKS";
    public static string WIN_STREAK = "WIN_STREAK";
    public static string TIMES_PLAY_LEVEL = "TIMES_PLAY_LEVEL";
    public static string TIMES_LOSE_LEVEL = "TIMES_LOSE_LEVEL";
    public static string PLAY_TYPE = "PLAY_TYPE";
    public static string TIMES_GIVE_UP = "TIMES_GIVE_UP";
    public static string TIMES_KEEP_PLAYING = "TIMES_KEEP_PLAYING";
    public static string TIME_START_LEVEL = "TIME_START_LEVEL";
    public static string TIME_START_AD = "TIME_START_AD";
    public static string TIME_START_SESSION = "TIME_START_SESSION";
    public static string IAP_COUNT = "IAP_COUNT";
    public static string TIME_PLAY = "TIME_PLAY";
    public static string FIRST_TIME_PLAY = "FIRST_TIME_PLAY";
    public static string OPEN_GAME = "OPEN_GAME";

    #endregion

    public static bool IsConnectionNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Debug.Log("Error. Check internet connection!");
            return false;
        }

        return true;
    }




    #region ObscuredData

    public static void SetDataString(string key, string value, string child = "none")
    {
        PlayerPrefs.SetString(key + "_" + child + "_" + Application.identifier, value);
        PlayerPrefs.Save();
    }

    public static string GetDataString(string key, string defaultValue = "none", string child = "none")
    {
        return PlayerPrefs.GetString(key + "_" + child + "_" + Application.identifier, defaultValue);
    }

    public static void SetDataInt(string key, int value, string child = "none")
    {
        PlayerPrefs.SetInt(key + "_" + child + "_" + Application.identifier, value);
        PlayerPrefs.Save();
    }

    public static int GetDataInt(string key, int defaultValue = 0, string child = "none")
    {
        return PlayerPrefs.GetInt(key + "_" + child + "_" + Application.identifier, defaultValue);
    }

    private static void SetDataFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key + "_" + Application.identifier, value);
        PlayerPrefs.Save();
    }

    private static float GetDataFloat(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(key + "_" + Application.identifier, defaultValue);
    }

    private static void SetDataDouble(string key, double value)
    {
        PlayerPrefs.SetFloat(key + "_" + Application.identifier, (float)value); // Convert double to float
        PlayerPrefs.Save();
    }

    private static double GetDataDouble(string key, float defaultValue = 0, string child = "none")
    {
        return PlayerPrefs.GetFloat(key + "_" + child + "_" + Application.identifier, defaultValue);
    }

    public static void SetDataBool(string key, bool value, string child = "none")
    {
        PlayerPrefs.SetInt(key + "_" + child + "_" + Application.identifier, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetDataBool(string key, bool defaultValue = false, string child = "none")
    {
        return PlayerPrefs.GetInt(key + "_" + child + "_" + Application.identifier, defaultValue ? 1 : 0) == 1;
    }

    public static bool IsHashkey(string key)
    {
        return PlayerPrefs.HasKey(key + "_none_" + Application.identifier);
    }

    public static bool IsHashkeyData(string key, string child = "none")
    {
        return PlayerPrefs.HasKey(key + "_" + child + "_" + Application.identifier);
    }

#endregion
}
