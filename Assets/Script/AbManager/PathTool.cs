using UnityEngine;
using System.Collections;

public class PathTool
{
    //不同平台下StreamingAssets的路径是不同的
    public static readonly string gStreamingAssets =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Application.streamingAssetsPath + "/";

#elif UNITY_IPHONE
		"file://" + Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
        Application.dataPath+"!assets/";
        
#else
            string.Empty;
#endif

    public static string String2DbText(string str)
    {
        return "'" + str + "'";
    }

    public static string Bool2Db(bool b)
    {
        return b ? "1" : "0";
    }

    public static void DebugAllPath()
    {
        //Debuger.Log("Application.dataPath = " + Application.dataPath);

    }

    public static string GetAudioAutoWavPath()
    {
        string path = "";
        if(Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
            path = path.Substring(0, path.IndexOf("Android/")) + "msc/";

            

        }
        else
        {
            path = Application.dataPath + "/";
        }
        //Debug.Log(path);

        return path;
    }

}
