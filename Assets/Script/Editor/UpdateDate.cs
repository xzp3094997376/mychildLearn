using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class UpdateDate
{
    [MenuItem("Framework/UpdateData")]
    static void ReLoadDate()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/txt");
        AssetDatabase.CreateFolder("Resources", "txt");
        AssetDatabase.CopyAsset("Assets/ResourcesStatic/config/txt", "Assets/Resources/txt");
        AssetDatabase.SaveAssets();
        Debug.Log("Update data finish");
        AssetDatabase.Refresh();
    }
}
