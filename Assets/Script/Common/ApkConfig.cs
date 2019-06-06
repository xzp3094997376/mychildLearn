using UnityEngine;
using System.Collections;
using System.IO;

public class ApkConfig
{ 
    public static string GetApkBuildPath()
    {
        //string path = Application.dataPath.Replace("Assets", "config.txt");
        //string[] context =File.ReadAllLines(path);
        return Application.dataPath /*context[0].Split('=')[1]*/;
    }

    public static string GetScreenTextureSavePath()
    {
        string path = Application.dataPath.Replace("Assets", "config.txt");
        string[] context = File.ReadAllLines(path);
        return context[1].Split('=')[1];
    }
}
