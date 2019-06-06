using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;




public class BuildAssetBundle : EditorWindow
{
    
    [MenuItem("Framework/BuildAssetBundle")]
    static void Init()  
    {
        BuildAssetBundle window = (BuildAssetBundle)EditorWindow.GetWindow(typeof(BuildAssetBundle));
        window.Show();
    }
    [MenuItem("Assets/Build AB")]
    private static void BuildAB()
    {
        List<string> paths = new List<string>();
        foreach (string guid in Selection.assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("public"))
            {
                Debug.LogError("包含public,为公用资源不打包,通过Global引用调用" + path);
                continue;
            }
            paths.Add(path);
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
        }
        Build(paths, false);

    }
    [MenuItem("Assets/ResCopy")]
    private static void ResCopyBuild()
    {
        List<string> paths = new List<string>();
        foreach (string guid in Selection.assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("public"))
            {
                Debug.LogError("包含public,为公用资源不打包,通过Global引用调用" + path);
                continue;
            }
            paths.Add(path);
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
        }
        ResCopyBuild(paths, false);

    }

    [MenuItem("Framework/打开，配置apk信息")]
    static void OpenApkInfo()
    {
        Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Script/Sys/ApkInfo.cs");
        //Debug.Log(guid);
        AssetDatabase.OpenAsset(obj, -1);   

    }


    Vector2 scrollPos;
    static string apk_language = string.Empty;
    void OnGUI()
    {
        
        //GUILayout.BeginVertical();
        GUILayout.Label("当前应用:" + ApkInfo.g_begin_scene);
        
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(300), GUILayout.Height(300));
        List<string> ab_paths = ApkInfo.GetSceneAbPaths(ApkInfo.g_begin_scene);
        for(int i = 0; i < ab_paths.Count; i++)
        {
            GUILayout.Label(ab_paths[i]);
        }
        EditorGUILayout.EndScrollView();

        
        if (GUILayout.Button("打开，配置apk信息"))
        {
            OpenApkInfo();
        }
        if (GUILayout.Button("重新打包上面列表中所有资源"))
        {
            Build(ab_paths, true);

            //var t: Texture2D = AssetDatabase.LoadAssetAtPath("Assets/Textures/texture.jpg", Texture2D) as Texture2D;

            GameObject objScene = GameObject.Find("Canvas/scene");
            Transform tranScene = null == objScene ? null : objScene.transform;
            if (null != tranScene)
                DestroyImmediate(tranScene.gameObject);
            tranScene = (new GameObject()).transform;
            tranScene.gameObject.name = "scene";
            tranScene.transform.SetParent(GameObject.Find("Canvas").transform);
            tranScene.transform.SetAsFirstSibling();
            RectTransform rtranScene = tranScene.gameObject.AddComponent<RectTransform>();
                                   
            rtranScene.localScale = Vector3.one;
            rtranScene.anchorMin = new Vector2(0, 0);
            rtranScene.anchorMax = new Vector2(1, 1);
            rtranScene.anchoredPosition3D = Vector3.zero;
            rtranScene.offsetMin = new Vector2(0, 0);
            rtranScene.offsetMax = new Vector2(0, 0);



            string path = "Assets/ScenePrefab/" + ApkInfo.g_begin_scene.ToString() + ".prefab";
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            GameObject sceneObj = GameObject.Instantiate(obj);
            sceneObj.transform.SetParent(rtranScene);
            sceneObj.transform.localPosition = Vector3.zero;
            sceneObj.transform.localEulerAngles = Vector3.zero;
            sceneObj.transform.localScale = Vector3.one;
            sceneObj.name = ApkInfo.g_begin_scene.ToString();

        }
        if (GUILayout.Button("ResCopy上面列表中所有资源"))
        {

            ResCopyBuild(ab_paths, true);
            
            GameObject objScene = GameObject.Find("Canvas/scene");
            Transform tranScene = null == objScene ? null : objScene.transform;
            if (null != tranScene)
                DestroyImmediate(tranScene.gameObject);
            tranScene = (new GameObject()).transform;
            tranScene.gameObject.name = "scene";
            tranScene.transform.SetParent(GameObject.Find("Canvas").transform);
            tranScene.transform.SetAsFirstSibling();
            RectTransform rtranScene = tranScene.gameObject.AddComponent<RectTransform>();
            rtranScene.localScale = Vector3.one;
            rtranScene.anchorMin = new Vector2(0, 0);
            rtranScene.anchorMax = new Vector2(1, 1);
            rtranScene.anchoredPosition3D = Vector3.zero;
            rtranScene.offsetMin = new Vector2(0, 0);
            rtranScene.offsetMax = new Vector2(0, 0);



            string path = "Assets/ScenePrefab/" + ApkInfo.g_begin_scene.ToString() + ".prefab";
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            GameObject sceneObj = GameObject.Instantiate(obj);
            sceneObj.transform.SetParent(rtranScene);
            sceneObj.transform.localPosition = Vector3.zero;
            sceneObj.transform.localEulerAngles = Vector3.zero;
            sceneObj.transform.localScale = Vector3.one;
            sceneObj.name = ApkInfo.g_begin_scene.ToString();
            
        }

        GUILayout.Label("如果要打包请在MathLogic_Program同级目录创建文件夹bin，再点下面按钮");

        //EditorGUILayout.EndVertical();   
        if (GUILayout.Button("测试 SetPlaySetting"))
        {
            SetPlaySetting();
        }
        if (GUILayout.Button("打包apk"))
        {
            BuildApk();
        }
        if (GUILayout.Button("public"))
        {
            
                UGUISpritePacker.CreateUguiPrefab("Assets/ResSprite/public", "public");

            
        }



    }


    static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    static void ReCreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
        

    }


    static void Build(List<string> asset_paths, bool detele_all)
    {
        List<string> sprite_path = new List<string>();
        List<string> texture_path = new List<string>();
        List<string> prefab_path = new List<string>();
        List<string> prefab_sound = new List<string>();

        for (int i = 0; i < asset_paths.Count; i++)
        {
            if(asset_paths[i].Contains("ResSprite/"))
            {
                sprite_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResTexture/"))
            {
                texture_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResPrefab/"))
            {
                prefab_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResSound/"))
            {
                prefab_sound.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }


        }

        BuildSprite(sprite_path, detele_all);
        BuildTexture(texture_path, detele_all);
        BuildPrefab(prefab_path, detele_all);
        BuildSound(prefab_sound, detele_all);

        AssetDatabase.Refresh();

    }
    static void BuildSprite(List<string> sprite_paths, bool delete_all)
    {
        if (null == sprite_paths || 0 == sprite_paths.Count)
            return;


        string path_steamingAssets = Application.streamingAssetsPath + "/sprite";

        if (delete_all)
            ReCreateDirectory(path_steamingAssets);
        else
            CreateDirectory(path_steamingAssets);

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        for(int i = 0; i < sprite_paths.Count; i++)
        {
            string[] strs = sprite_paths[i].Split('/');

            AssetBundleBuild map = new AssetBundleBuild();

            map.assetBundleName = strs[strs.Length - 1];
            map.assetBundleVariant = "";
            map.assetNames = new string[1];
            map.assetNames[0] = sprite_paths[i] + "/" + strs[strs.Length - 1] + ".prefab";
            buildMap.Add(map);

            //Debug.Log("build ab: " + map.assetBundleName);
            //Debug.Log(map.assetNames[0]);
        }


        /*
        string path_in_ngui_texture = "Assets/" + apk_name + "/texture";
        if (Directory.Exists(path_in_ngui_texture))
        {
            DirectoryInfo dirInfo_texture = new DirectoryInfo(path_in_ngui_texture);
            foreach (DirectoryInfo directory_child in dirInfo_texture.GetDirectories())
            {
                string mat = directory_child.Name + ".mat";
                string png = directory_child.Name + ".png";
                string prefab = directory_child.Name + ".prefab";
                string mat_meta = directory_child.Name + ".mat.meta";
                string png_meta = directory_child.Name + ".png.meta";
                string prefab_meta = directory_child.Name + ".prefab.meta";

                if (!File.Exists(directory_child.FullName + "/" + mat)) continue;
                if (!File.Exists(directory_child.FullName + "/" + png)) continue;
                if (!File.Exists(directory_child.FullName + "/" + prefab)) continue;

                AssetBundleBuild map = new AssetBundleBuild();

                map.assetBundleName = directory_child.Name;
                map.assetBundleVariant = "";
                map.assetNames = new string[6];
                map.assetNames[0] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + mat;
                map.assetNames[1] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + png; ;
                map.assetNames[2] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + prefab; ;
                map.assetNames[3] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + mat_meta; ;
                map.assetNames[4] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + png_meta; ;
                map.assetNames[5] = "Assets/" + apk_name + "/texture/" + directory_child.Name + "/" + prefab_meta; ;

                buildMap.Add(map);

            }


        }
        */

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/sprite", buildMap.ToArray(), BuildAssetBundleOptions.CompleteAssets|BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);




    }
    static void BuildTexture(List<string> tex_paths, bool delete_all)
    {
        if (null == tex_paths || 0 == tex_paths.Count)
            return;


        string path_steamingAssets = Application.streamingAssetsPath + "/texture";
        if (delete_all)
            ReCreateDirectory(path_steamingAssets);
        else
            CreateDirectory(path_steamingAssets);

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        for (int i = 0; i < tex_paths.Count; i++)
        {
            string[] lookFor = new string[] { tex_paths[i] };
            string[] guids2 = AssetDatabase.FindAssets("t:texture2D", lookFor);
            //foreach (string guid in guids2)
            //{
            //    string sprite_path = AssetDatabase.GUIDToAssetPath(guid);
            //    Debug.Log(sprite_path);
            //}
            
            
            string[] strs = tex_paths[i].Split('/');
            AssetBundleBuild map = new AssetBundleBuild();
            map.assetBundleName = strs[strs.Length - 1];
            map.assetBundleVariant = "";
            map.assetNames = new string[guids2.Length];
            for(int j = 0; j < guids2.Length; j++)
            {
                //map.assetNames[0] = tex_paths[i] + "/" + strs[strs.Length - 1] + ".prefab";
                map.assetNames[j] = AssetDatabase.GUIDToAssetPath(guids2[j]);
                //map.assetNames[j * 2] = AssetDatabase.GUIDToAssetPath(guids2[j]);
                //map.assetNames[j * 2 + 1] = AssetDatabase.GUIDToAssetPath(guids2[j]) + ".meta";
                //Debug.Log(map.assetNames[j * 2]);
                //Debug.Log(map.assetNames[j * 2 + 1]);
                //Debug.Log(map.assetNames[j]);
            }
            buildMap.Add(map);

            Debug.Log("build ab: " + map.assetBundleName);


        }
        

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/texture", buildMap.ToArray(), BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);




    }
    static void BuildPrefab(List<string> prefab_paths, bool delete_all)
    {
        if (null == prefab_paths || 0 == prefab_paths.Count)
            return;


        string path_steamingAssets = Application.streamingAssetsPath + "/prefab";
        if(delete_all)
            ReCreateDirectory(path_steamingAssets);
        else
            CreateDirectory(path_steamingAssets);


        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        for (int i = 0; i < prefab_paths.Count; i++)
        {
            string[] lookFor = new string[] { prefab_paths[i] };
            string[] guids2 = AssetDatabase.FindAssets("t:prefab", lookFor);
            string[] strs = prefab_paths[i].Split('/');
            for(int j = 0; j < guids2.Length; j++)
            {
                //Debug.Log(AssetDatabase.GUIDToAssetPath(guids2[j]));
                string path = AssetDatabase.GUIDToAssetPath(guids2[j]);
                //if (!path.Contains(".prefab") || path.Contains(".meta"))
                //    continue;

                AssetBundleBuild map = new AssetBundleBuild();
                map.assetBundleName = strs[strs.Length - 1];
                map.assetBundleVariant = "";
                map.assetNames = new string[2];
                map.assetNames[0] = path;// prefab_paths[i] + "/" + strs[strs.Length - 1] + ".prefab";
                map.assetNames[1] = path + ".meta";// prefab_paths[i] + "/" + strs[strs.Length - 1] + ".prefab.meta";
                buildMap.Add(map);


                //Debug.Log(map.assetNames[0]);
                //Debug.Log(map.assetNames[1]);
            }



            //Debug.Log(map.assetNames[0]);
            //Debug.Log(map.assetNames[1]);
            //Debug.Log("build ab: " + map.assetBundleName);
        }
        
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/prefab", buildMap.ToArray(), BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);


    }
    static void BuildSound(List<string> sound_paths, bool delete_all)
    {
        if (null == sound_paths || 0 == sound_paths.Count)
            return;


        string path_steamingAssets = Application.streamingAssetsPath + "/sound";
        if (delete_all)
            ReCreateDirectory(path_steamingAssets);
        else
            CreateDirectory(path_steamingAssets);

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        for (int i = 0; i < sound_paths.Count; i++)
        {
            //Debug.Log("path = " + sound_paths[i]);
            string[] lookFor = new string[] { sound_paths[i] };
            string[] guids2 = AssetDatabase.FindAssets("t:audioclip", lookFor);
            //foreach (string guid in guids2)
            //{
            //    string sprite_path = AssetDatabase.GUIDToAssetPath(guid);
            //    Debug.Log(sprite_path);
            //}
            

            string[] strs = sound_paths[i].Split('/');
            AssetBundleBuild map = new AssetBundleBuild();
            map.assetBundleName = strs[strs.Length - 1];
            map.assetBundleVariant = "";
            map.assetNames = new string[guids2.Length];
            for (int j = 0; j < guids2.Length; j++)
            {
                map.assetNames[j] = AssetDatabase.GUIDToAssetPath(guids2[j]);
            }
            buildMap.Add(map);

            Debug.Log("build ab: " + map.assetBundleName);

            
        }
        
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/sound", buildMap.ToArray(), BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);

        

    }
    
    static void ResCopyBuild(List<string> asset_paths, bool detele_all)
    {
        if (detele_all)
            ReCreateDirectory("Assets/Resources/ResCopy");
        else 
            CreateDirectory("Assets/Resources/ResCopy");

        List<string> sprite_path = new List<string>();
        List<string> texture_path = new List<string>();
        List<string> prefab_path = new List<string>();
        List<string> prefab_sound = new List<string>();

        for (int i = 0; i < asset_paths.Count; i++)
        {
            if (asset_paths[i].Contains("ResSprite/"))
            {
                sprite_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResTexture/"))
            {
                texture_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResPrefab/"))
            {
                prefab_path.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }
            else if (asset_paths[i].Contains("ResSound/"))
            {
                prefab_sound.Add(asset_paths[i]);
                //Debug.Log("ass = " + asset_paths[i]);
            }

        }

        ResCopyBuildSprite(sprite_path, detele_all);
        ResCopyBuildTexture(texture_path, detele_all);
        ResCopyBuildPrefab(prefab_path, detele_all);
        ResCopyBuildSound(prefab_sound, detele_all);

        AssetDatabase.Refresh();

    }
    static void ResCopyBuildSprite(List<string> sprite_paths, bool delete_all)
    {
        if (null == sprite_paths || 0 == sprite_paths.Count)
            return;

        string path_recpoyAssets = "Assets/Resources/ResCopy/sprite";

        if (delete_all && AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.DeleteAsset(path_recpoyAssets);
        }
        if(!AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy", "sprite" );
        }

        
        for (int i = 0; i < sprite_paths.Count; i++)
        {
            string[] strs = sprite_paths[i].Split('/');
            string folder_name = strs[strs.Length - 1];
            string path1 = sprite_paths[i] + "/" + folder_name + ".prefab";
            string path2 = "Assets/Resources/ResCopy/sprite/" + folder_name + ".prefab";
            //Debug.Log(path1 + "\n" + path2);
            AssetDatabase.CopyAsset(path1, path2);

        }
        
    }
    static void ResCopyBuildTexture(List<string> tex_paths, bool delete_all)
    {
        if (null == tex_paths || 0 == tex_paths.Count)
            return;

        string path_recpoyAssets = "Assets/Resources/ResCopy/texture";

        if (delete_all && AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.DeleteAsset(path_recpoyAssets);
        }
        if (!AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy", "texture");
        }

        for (int i = 0; i < tex_paths.Count; i++)
        {
            string[] strs = tex_paths[i].Split('/');
            string folder_name = strs[strs.Length - 1];
            string path2 = "Assets/Resources/ResCopy/texture/" + folder_name;
            if (AssetDatabase.IsValidFolder(path2))
            {
                AssetDatabase.DeleteAsset(path2);
            }
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy/texture", folder_name);
            //Debug.Log(folder_name);
            //continue;
            string[] assets_guid = AssetDatabase.FindAssets("t:texture", new string[] { tex_paths[i] });
            for (int j = 0; j < assets_guid.Length; j++)
            {
                string asset_path = AssetDatabase.GUIDToAssetPath(assets_guid[j]);
                string[] asset_path_strs = asset_path.Split('/');
                string path = path2 + "/" + asset_path_strs[asset_path_strs.Length - 1];
                //Debug.Log(asset_path + "\n" + path);
                AssetDatabase.CopyAsset(asset_path, path);
            }


        }
    }
    static void ResCopyBuildPrefab(List<string> prefab_paths, bool delete_all)
    {
        if (null == prefab_paths || 0 == prefab_paths.Count)
            return;

        string path_recpoyAssets = "Assets/Resources/ResCopy/prefab";

        if (delete_all && AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.DeleteAsset(path_recpoyAssets);
        }
        if (!AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy", "prefab");
        }
        
        for (int i = 0; i < prefab_paths.Count; i++)
        {
            string[] strs = prefab_paths[i].Split('/');
            string folder_name = strs[strs.Length - 1];
            string path2 = "Assets/Resources/ResCopy/prefab/" + folder_name;
            if (AssetDatabase.IsValidFolder(path2))
            {
                AssetDatabase.DeleteAsset(path2);
            }
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy/prefab", folder_name);
            
            string[] assets_guid = AssetDatabase.FindAssets("t:prefab", new string[]{ prefab_paths[i]});
            for (int j = 0; j < assets_guid.Length; j++)
            {
                string asset_path = AssetDatabase.GUIDToAssetPath(assets_guid[j]);
                string[] asset_path_strs = asset_path.Split('/');
                string path = path2 + "/" + asset_path_strs[asset_path_strs.Length - 1];
                //Debug.Log(asset_path + "\n" + path);
                AssetDatabase.CopyAsset( asset_path, path);
            }


        }

    }
    static void ResCopyBuildSound(List<string> sound_paths, bool delete_all)
    {
        if (null == sound_paths || 0 == sound_paths.Count)
            return;

        string path_recpoyAssets = "Assets/Resources/ResCopy/sound";

        if (delete_all && AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.DeleteAsset(path_recpoyAssets);
        }
        if (!AssetDatabase.IsValidFolder(path_recpoyAssets))
        {
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy", "sound");
        }

        for (int i = 0; i < sound_paths.Count; i++)
        {
            string[] strs = sound_paths[i].Split('/');
            string folder_name = strs[strs.Length - 1];
            string path2 = "Assets/Resources/ResCopy/sound/" + folder_name;
            if (AssetDatabase.IsValidFolder(path2))
            {
                AssetDatabase.DeleteAsset(path2);
            }
            AssetDatabase.CreateFolder("Assets/Resources/ResCopy/sound", folder_name);

            string[] assets_guid = AssetDatabase.FindAssets("t:audioclip", new string[] { sound_paths[i] });
            //Debug.Log(sound_paths[i] + assets_guid.Length);

            for (int j = 0; j < assets_guid.Length; j++)
            {
                string asset_path = AssetDatabase.GUIDToAssetPath(assets_guid[j]);
                string[] asset_path_strs = asset_path.Split('/');
                string path = path2 + "/" + asset_path_strs[asset_path_strs.Length - 1];
                //Debug.Log(asset_path + "\n" + path);
                AssetDatabase.CopyAsset(asset_path, path);
            }


        }


    }



    static void BuildApk()
    {            
        SetPlaySetting();
        AssetDatabase.SaveAssets();

        string[] levels = new string[] { "Assets/Scene/Enter.unity" };

        string apkPath = ApkConfig.GetApkBuildPath();

        BuildPipeline.BuildPlayer(levels, apkPath + "/mathlogic_" + apk_language + ApkInfo.g_begin_scene.ToString().ToLower() + "_" + ApkInfo.g_version_single + ".apk",
                       BuildTarget.Android, BuildOptions.None);

        apkPath = "F:/Edu/test.txt";
        System.Diagnostics.Process.Start(apkPath);
    }
    

    public static void SaveTexture2DToPNG(Texture2D tex, string path, string pngName)
    {
        byte[] bytes = tex.EncodeToPNG();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        FileStream file = File.Open(path + "/" + pngName, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();

    }
    static void SetPlaySetting()
    {

        string define_str = ApkInfo.GetGameDefine(ApkInfo.g_begin_scene);
        if (!string.IsNullOrEmpty(define_str))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, define_str);
        }

        //设置替换闪屏
        Texture2D useTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScreenShort/" + ApkInfo.GetScreenShort(ApkInfo.g_begin_scene) + ".jpg");
        Texture2D setTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScreenShort/AScreenShort.jpg");
        if(null != useTex)
        {
            byte[] data = useTex.GetRawTextureData();
            setTex.LoadRawTextureData(data);
            setTex.Apply();
            
        }
        /*
        */

        //替换load图标
        apk_language = string.Empty;
        //中文
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/res/drawable-tvdpi/load_cn.png");
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/res/drawable-tvdpi/load_cn.jpg");
        Texture2D useTex_load = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScreenShort/" + ApkInfo.GetScreenShort(ApkInfo.g_begin_scene) + ".jpg");
        if (null != useTex_load)
        {
            Color[] temp_colors = useTex_load.GetPixels(71, 0, 1280, 800);
            Texture2D temp_load = new Texture2D(1280, 800);
            temp_load.SetPixels(temp_colors);
            temp_load.Apply();
            SaveTexture2DToPNG(temp_load, "Assets/Plugins/Android/res/drawable-tvdpi", "load_cn.png");
            apk_language = "_cn";
        }
        //英文
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/res/drawable-tvdpi/load_en.png");
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/res/drawable-tvdpi/load_en.jpg");
        Texture2D useTex_load1 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ScreenShort/" + ApkInfo.GetScreenShort(ApkInfo.g_begin_scene) + "en.jpg");
        if (null != useTex_load1)
        {
            Color[] temp_colors = useTex_load1.GetPixels(71, 0, 1280, 800);
            Texture2D temp_load = new Texture2D(1280, 800);
            temp_load.SetPixels(temp_colors);
            temp_load.Apply();
            SaveTexture2DToPNG(temp_load, "Assets/Plugins/Android/res/drawable-tvdpi", "load_en.png");
            apk_language += "_en";
        }
        if(!string.IsNullOrEmpty(apk_language))
        {
            apk_language += "_";
        }



        //替换icon图标
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/res/drawable-hdpi/ic_launcher.png");
        AssetDatabase.CopyAsset(
            "Assets/Icon/" + ApkInfo.GetScreenShort(ApkInfo.g_begin_scene) + ".png",
            "Assets/Plugins/Android/res/drawable-hdpi/ic_launcher.png");
            



        int version_code = ApkInfo.g_version_code_single;
        string version = ApkInfo.g_version_single;
        string pkg = GetPkg();
        string app_name = ApkInfo.GetSceenName(ApkInfo.g_begin_scene);
        string icon = "ic_launcher";

        if(ApkInfo.g_begin_scene.ToString().Contains("Test"))
        {
            pkg = "com.kimi." + ApkInfo.g_begin_scene.ToString().ToLower();
        }

        PlayerSettings.productName = app_name;
        PlayerSettings.applicationIdentifier = pkg;
        PlayerSettings.bundleVersion = version;
        PlayerSettings.Android.bundleVersionCode = version_code;

        //设置安卓文件
        string path_android_mainfest = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
        if (File.Exists(path_android_mainfest))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path_android_mainfest);

            XmlNode node = xml.SelectSingleNode("manifest");
            ((XmlElement)node).SetAttribute("package", pkg);
            ((XmlElement)node).SetAttribute("android:versionCode", version_code.ToString());
            ((XmlElement)node).SetAttribute("android:versionName", version);

            XmlNode node1 = xml.SelectSingleNode("manifest/application");
            ((XmlElement)node1).SetAttribute("android:icon", "@drawable/" + icon);
            ((XmlElement)node1).SetAttribute("android:label", app_name);

            //XmlNode node2 = xml.SelectSingleNode("manifest/application/activity");
            //((XmlElement)node2).SetAttribute("android:name", main_activity);

            //XmlNode node3 = xml.SelectSingleNode("manifest/application");
            //((XmlElement)node3).SetAttribute("android:label", "@string/" + lable);

            //XmlNode node4 = xml.SelectSingleNode("manifest/application/activity");
            //((XmlElement)node4).SetAttribute("android:theme", "@style/" + maintheme);

            //Debug.Log(((XmlElement)node2).GetAttribute("android:name"));
            //((XmlElement)node1).SetAttribute("android:icon", "@drawable/" + icon);
            //((XmlElement)node1).SetAttribute("android:label", "@string/app_name_kimi");

        


            xml.Save(path_android_mainfest);
        }

        //替换游戏名字语音
        //string wav_begin = "Assets/ResSound/aa_game_name/" + ApkInfo.GetSceenName(ApkInfo.g_begin_scene) + ".wav";
        //string wav_end = "Assets/Plugins/Android/res/raw/app_name.wav";
        //string wav_begin = Application.dataPath + "/ResSound/aa_game_name/" + ApkInfo.g_begin_scene.ToString().ToLower() + ".wav";
        //string wav_end = Application.dataPath  + "/Plugins/Android/res/raw/" + ApkInfo.g_begin_scene.ToString().ToLower() + ".wav";
        string wav_begin = Application.dataPath + "/ResSound/aa_game_name/" + ApkInfo.GetSceenName(ApkInfo.g_begin_scene) + ".wav";
        string wav_end = Application.dataPath + "/Plugins/Android/res/raw/app_name.wav";

        //Debug.Log(wav_begin + "\n" + wav_end);
        //AssetDatabase.CopyAsset(wav_begin, wav_end);
        File.Copy(wav_begin, wav_end, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 获取包名
    /// </summary>
    /// <returns></returns>
    public static string GetPkg()
    {
        
        


        switch (ApkInfo.g_begin_scene)
        {
            //case SceneEnum.PickPeach:
            case SceneEnum.MonkeySortOut:
            case SceneEnum.AnimalClassification:
            case SceneEnum.AnimalStatistics:
            case SceneEnum.RegularOrder:
            case SceneEnum.AnimalSort:
            case SceneEnum.BirthdayCake:
            case SceneEnum.AnimalCanFly:
            case SceneEnum.GoodFriendCar:
            case SceneEnum.TvRoom:
            case SceneEnum.SingleAndDualNumber:
            case SceneEnum.FunnyGroup:
            case SceneEnum.CatchFish:
            case SceneEnum.AnimalEquation:
            //case SceneEnum.ManyEquation:
            case SceneEnum.LineUpCtrl:
            //case SceneEnum.AnimalsHome:
            case SceneEnum.ChookPk:
            case SceneEnum.KnowCube:
            case SceneEnum.AnimalParty:
            case SceneEnum.AnimalRotate:
            case SceneEnum.FindHome:
            case SceneEnum.AnimalNumOnly:
            case SceneEnum.GroupCheck:
            case SceneEnum.LearnTime:
            case SceneEnum.KnowCalendar:
                {
                    return "com.mathlogic." + ApkInfo.g_begin_scene.ToString().ToLower();
                }
            default:
                {
                    StreamReader sr = File.OpenText(Application.dataPath + "/Resources/txt/config_game.txt");
                    string content = sr.ReadLine();
                    while (null != content)
                    {
                        if(content.Contains(ApkInfo.g_begin_scene.ToString()))
                        {
                            string msg = "";
                            if (content.Contains("小班"))
                                msg = "xiaoban";
                            else if (content.Contains("中班"))
                                msg = "zhongban";
                            else if (content.Contains("大班"))
                                msg = "daban";
                            else
                                Debug.LogError("配置表的班级错误");


                            if (content.Contains("，集合，"))
                                msg += "0.";
                            else if (content.Contains("，数，"))
                                msg += "1.";
                            else if (content.Contains("，量，"))
                                msg += "2.";
                            else if (content.Contains("，形，"))
                                msg += "3.";
                            else if (content.Contains("，时，"))
                                msg += "4.";
                            else if (content.Contains("，空，"))
                                msg += "5.";
                            else
                                Debug.LogError("配置表的模块错误");

                            //Debug.Log("com.mathlogic." + msg + ApkInfo.g_begin_scene.ToString().ToLower());
                            return "com.mathlogic." + msg + ApkInfo.g_begin_scene.ToString().ToLower();
                            
                        }
                        content = sr.ReadLine();
                    }
                    sr.Close();
                    
                    Debug.LogError("包名错误，没有找到" + ApkInfo.g_begin_scene + " 的配置表" );
                    return "com.mathlogic." + ApkInfo.g_begin_scene.ToString().ToLower();
                }
                

        }

        

    }


}
