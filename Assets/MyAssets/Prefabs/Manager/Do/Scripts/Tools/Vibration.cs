using Do;
using System;
using UnityEngine;

public static class VibrationUtil
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
      
        if (isAndroid())
        {
            try
            {
                Handheld.Vibrate();
            }
            catch
            {
                // ignored
            }
        }
        else
            Handheld.Vibrate();
    }


    public static void Vibrate(long milliseconds)
    {
       
        if (isAndroid())
        {
            try
            {
//                vibrator.Call("vibrate", milliseconds);
            }
            catch
            {
                // ignored
            }
        }
        else
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
        {
            try
            {
//                vibrator.Call("vibrate", pattern, repeat);
            }
            catch
            {
                // ignored
            }
        }
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (!isAndroid()) 
            return;
        try
        {
        //    vibrator.Call("cancel");
        }
        catch
        {
            // Debug.Log("Null");
            throw;
        }
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
