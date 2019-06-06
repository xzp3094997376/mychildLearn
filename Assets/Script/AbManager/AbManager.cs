using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbManager
{

    static List<string> m_can_destroy_abs = new List<string>();
    static Dictionary<string, AssetBundle> m_all_abs = new Dictionary<string, AssetBundle>();


    static AssetBundle LoadAB(AbEnum ab_enum, string ab_name)
    {
        string path = PathTool.gStreamingAssets + ab_enum.ToString() + "/" + ab_name;
        try
        {
            return AssetBundle.LoadFromFile(path);
        }
        catch
        {
            Debug.LogError("获取音效失败：ab_enum=" + ab_enum + "  ab_name=" + ab_name);
            return null;
        }
    }
    public static AssetBundle GetAB(AbEnum ab_enum, string ab_name, bool can_destroy = true)
    {
        if(m_all_abs.ContainsKey(ab_name))
        {
            return m_all_abs[ab_name];
        }
        else
        {
            AssetBundle ab = LoadAB(ab_enum, ab_name);
            m_all_abs.Add(ab_name, ab);
            if(can_destroy)
            {
                m_can_destroy_abs.Add(ab_name);
            }
            return ab;
        }
    }

    
	


}

