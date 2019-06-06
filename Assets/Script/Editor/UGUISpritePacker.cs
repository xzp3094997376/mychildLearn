using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UGUISpritePacker : EditorWindow
{
    public static List<string> g_sprite_folder_name = new List<string>();


    [MenuItem("Framework/Sprite Packer")]
    static void Init()
    {
        g_sprite_folder_name.Clear();
        string[] folder_names = AssetDatabase.GetSubFolders("Assets/ResSprite");
        if (null != folder_names && 0 < folder_names.Length)
        {
            for (int i = 0; i < folder_names.Length; i++)
            {
                g_sprite_folder_name.Add(folder_names[i]);
                Debug.Log(folder_names[i]);
            }
        }


        UGUISpritePacker window = (UGUISpritePacker)EditorWindow.GetWindow(typeof(UGUISpritePacker));
        window.Show();
    }
    public static List<string> h_sprite_folder_name = new List<string>();
    [MenuItem("Framework/debug")]
    static void DebugS()
    {
        h_sprite_folder_name.Add("xzp");
        UGUISpritePacker window = EditorWindow.GetWindow<UGUISpritePacker>();        
        window.Show();
    }
    [MenuItem("Assets/Create UGUI Prefab")]
    private static void BuildAB()
    {
        List<string> paths = new List<string>();
        foreach (string guid in Selection.assetGUIDs)
        {
            paths.Add(AssetDatabase.GUIDToAssetPath(guid));
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
        }
        for(int i = 0; i < paths.Count; i++)
        {
            string[] strs = paths[i].Split('/');
            CreateUguiPrefab(paths[i], strs[strs.Length - 1]);
        }

    }

    void OnGUI()
    {
        for (int i = 0; i < g_sprite_folder_name.Count; i++)
        {
            string[] temp = g_sprite_folder_name[i].Split('/');
            if (temp.Length < 3)
                continue;

            if(GUI.Button(new Rect(i % 3 * 150, i / 3 * 24, 150, 20), temp[2]))
            {                
                CreateUguiPrefab(g_sprite_folder_name[i], temp[2]);
            }
            
        }
        

    }

    public static void CreateUguiPrefab(string path_folder, string prefab_name)
    {
        //Debug.Log("path_folder = " + path_folder + " prefab_name = " + prefab_name);
        GameObject obj = new GameObject();
        obj.name = prefab_name;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        UguiSpriteControl com = obj.AddComponent<UguiSpriteControl>();

        string[] lookFor = new string[] { path_folder };
        string[] guids2 = AssetDatabase.FindAssets("t:texture2D", lookFor);
        foreach (string guid in guids2)
        {
            string sprite_path = AssetDatabase.GUIDToAssetPath(guid);
            com.msprites_list.Add(AssetDatabase.LoadAssetAtPath<Sprite>(sprite_path));           
        }

        Object prefab = PrefabUtility.CreateEmptyPrefab(path_folder + "/" + prefab_name + ".prefab");
        GameObject prefab_obj = PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
        
        if(prefab_name.Equals("public"))
        {
            //GameObject.Find("Global").GetComponent<Global>().mPublic = prefab_obj.GetComponent<UguiSpriteControl>();
        }


        DestroyImmediate(obj);

        //BuildPipeline.BuildAssetBundle(prefab, )

    }


}
