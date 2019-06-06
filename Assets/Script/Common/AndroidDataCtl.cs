using UnityEngine;
using System.Collections;

public class AndroidDataCtl
{ 
    public static T GetDataFromAndroid<T>(string func_name)
    {
        T result;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                result = jo.Call<T>(func_name);
            }
        }
        return result;
    }

    public static T GetDataFromAndroid<T>(string func_name, string param)
    {
        T result;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                result = jo.Call<T>(func_name, param);
            }
        }
        return result;
    }

    public static T GetDataFromAndroid<T>(string func_name, params object[] param)
    {
        T result;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                result = jo.Call<T>(func_name, param);
            }
        }
        return result;
    }


    public static void DoAndroidFunc(string func_name)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.Call(func_name);
            }
        }
#endif
    }

    public static void DoAndroidFunc(string func_name, params object[] param)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.Call(func_name, param);
            }
        }
#endif
    }

   
}
