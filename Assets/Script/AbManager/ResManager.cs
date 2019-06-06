using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResManager : MonoBehaviour
{
    static ResManager instance = null;

    //ugui
    static Dictionary<string, UguiSpriteControl> m_ugui_dic = new Dictionary<string, UguiSpriteControl>();
    public static void AddUguiSpriteControl(string ab_name, UguiSpriteControl com)
    {
        if(!m_ugui_dic.ContainsKey(ab_name))
        {
            m_ugui_dic.Add(ab_name, com);
        }
    }
    public static Sprite GetSprite(string ab_name, string sprite_name)
    {
        if(m_ugui_dic.ContainsKey(ab_name))
        {
            try
            {
                return m_ugui_dic[ab_name].msprite_dic[sprite_name];
            }
            catch
            {
                Debug.LogError("错误：没有这样的图片ab_nam=" + ab_name + ", sprite_name=" + sprite_name);
                return null;
            }
        }
        else
        {
            switch(ApkInfo.g_load_res_type)
            {
                case LoadResourcesEnum.Ab:
                    {
                        AssetBundle ab = AbManager.GetAB(AbEnum.sprite, ab_name);
                        GameObject obj = GameObject.Instantiate(ab.LoadAsset(ab_name + ".prefab")) as GameObject;
                        UguiSpriteControl com = obj.GetComponent<UguiSpriteControl>();
                        obj.transform.SetParent(Global.instance.transform);
                        obj.SetActive(false);
                        com.Init();
                        m_ugui_dic.Add(ab_name, com);
                        return com.msprite_dic[sprite_name];
                    }
                default:
                    {
                        GameObject obj = GameObject.Instantiate(Resources.Load("ResCopy/sprite/" + ab_name)) as GameObject;
                        UguiSpriteControl com = obj.GetComponent<UguiSpriteControl>();
                        obj.transform.SetParent(Global.instance.transform);
                        obj.SetActive(false);
                        com.Init();
                        m_ugui_dic.Add(ab_name, com);
                        return com.msprite_dic[sprite_name];

                    }
            }

        }
    }
    
    //texture
    public static Texture GetTexture(string ab_name, string texture_name)
    {
        switch(ApkInfo.g_load_res_type)
        {
            case LoadResourcesEnum.Ab:
                {
                    AssetBundle ab = AbManager.GetAB(AbEnum.texture, ab_name);
                    return ab.LoadAsset<Texture>(texture_name);
                }
            default:
                {
                    return Resources.Load<Texture>("ResCopy/texture/" + ab_name + "/" + texture_name);
                }
        }
    }
    public static Texture GetTextureInResources(string texture_name)
    {
        return Resources.Load<Texture>("texture/" + texture_name);
    }


    //prefab
    public static GameObject GetPrefab(string ab_name, string prefab_name)
    {
#if UNITY_EDITOR
        Object obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/ResPrefab/" + ab_name + "/" + prefab_name + ".prefab");
        if (null != obj)
        {
            return GameObject.Instantiate(obj) as GameObject;
        }
        else
        {
            Debug.LogError("没有资源 " + "Assets/ResPrefab/" + ab_name + "/" + prefab_name + ".prefab");
            return null;
        }

#elif UNITY_ANDROID
        switch(ApkInfo.g_load_res_type)
        {
            case LoadResourcesEnum.Ab:
                {
                    AssetBundle ab = AbManager.GetAB(AbEnum.prefab, ab_name);
                    GameObject obj = ab.LoadAsset<GameObject>(prefab_name);
                    if (null != obj)
                        return GameObject.Instantiate(obj) as GameObject;
                    else
                        return null;
                }
            default:
                {
                 
                    return GameObject.Instantiate(Resources.Load("ResCopy/prefab/" + ab_name + "/" + prefab_name)) as GameObject;
                }
        }

#else
        AssetBundle ab = AbManager.GetAB(AbEnum.prefab, ab_name);
        GameObject obj = ab.LoadAsset<GameObject>(prefab_name);
        if (null != obj)
            return GameObject.Instantiate(obj) as GameObject;
        else
            return null;
#endif
    }

    public static GameObject GetPrefab(string ab_name, string prefab_name, Transform _trans)
    {
        GameObject mmgo = GetPrefab(ab_name, prefab_name);
        if (mmgo != null)
        {
            mmgo.transform.SetParent(_trans);
            mmgo.transform.localPosition = Vector3.zero;
            mmgo.transform.localScale = Vector3.one;
        }
        return mmgo;
    }

    public static GameObject GetPrefabInResources(string prefab_pathf)
    {
        //Debug.LogError(prefab_pathf);
        Object obj = Resources.Load(prefab_pathf);
        if (null == obj)
            return null;
        else
            return GameObject.Instantiate(obj) as GameObject;
    }
    /*
    public static AssetBundleRequest GetPrefabAllAsync(string ab_name)
    {

#if UNITY_EDITOR

#elif UNITY_ANDROID
        AssetBundle ab = AbManager.GetAB(AbEnum.prefab, ab_name);
        return ab.LoadAllAssetsAsync<GameObject>();

#endif
        return null;
    }
    */

    //sound
    public static AudioClip GetClip(string ab_name, string sound_name)
    {
        //Debug.Log(sound_name);
        switch(ApkInfo.g_load_res_type)
        {
            case LoadResourcesEnum.Ab:
                {
                    AssetBundle ab = AbManager.GetAB(AbEnum.sound, ab_name);
                    AudioClip clip = ab.LoadAsset<AudioClip>(sound_name);
                    if (null == clip)
                        Debug.LogError("没有音效" + ab_name + ", " + sound_name);
                    return clip;
                }
            default:
                {
                    AudioClip clip = Resources.Load<AudioClip>("ResCopy/sound/" + ab_name + "/" + sound_name);
                    if (null == clip)
                        Debug.LogError("没有音效" + ab_name + ", " + sound_name);
                    return clip;
                }
        }
    }
    public static AudioClip GetClipInResources(string sound_name)
    {
        AudioClip clip = Resources.Load<AudioClip>("sound/" + sound_name);
        if (null == clip)
            Debug.LogError("没有音效 " + sound_name);
        return clip;
    }
    public static void GetClipAsync(string ab_name, string sound_name, System.Action<AudioClip> callback)
    {
        //instance.StartCoroutine(instance.LoadAssets_AudioClip(ab_name, sound_name, callback));

        callback(GetClip(ab_name, sound_name));

    }



    void Awake()
    {
        instance = this;
    }
    /*
    IEnumerator LoadAssets_AudioClip(string ab_name, string sound_name, System.Action<AudioClip> callback)
    {
        AssetBundle ab = AbManager.GetAB(AbEnum.sound, ab_name);
        AssetBundleRequest request = ab.LoadAssetAsync<AudioClip>(sound_name);
        while (!request.isDone)
            yield return null;

        if(null != callback)
        {
            callback(request.asset as AudioClip);
        }

    }
    */


}
