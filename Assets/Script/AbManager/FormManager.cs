using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FormManager
{

    static Dictionary<int, config_apk> gDic_config_config_apk = null;
    public static Dictionary<int, config_apk> Config_config_apk
    {
        get
        {
            if (null == gDic_config_config_apk)
            {
                gDic_config_config_apk = new Dictionary<int, config_apk>();
                TextAsset text_asset = Resources.Load<TextAsset>("config_apk");
                string[] lines = text_asset.text.Split('\n');
                if (2 >= lines.Length)
                {
                    return gDic_config_config_apk;
                }
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                        continue;
                    config_apk data = config_apk.Convert(line);
                    gDic_config_config_apk.Add(data.m_apk_define, data);
                }
            }
            return gDic_config_config_apk;
        }
    }


    static Dictionary<SceneEnum, config_game> data_config_games = null;
    public static Dictionary<SceneEnum, config_game> config_games
    {
        get
        {
            if (null == data_config_games)
            {
                data_config_games = new Dictionary<SceneEnum, config_game>();
                TextAsset text_asset = Resources.Load<TextAsset>("txt/config_game");
                string[] lines = text_asset.text.Split('\n');
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                        continue;
                    config_game data = config_game.Convert(line);
                    data_config_games.Add((SceneEnum)Enum.Parse(typeof(SceneEnum), data.m_game_enum), data);
                }
            }
            return data_config_games;
        }
    }



    static Dictionary<int, config_tujian> data_config_tujians = null;
    public static Dictionary<int, config_tujian> config_tujians
    {
        get
        {
            if (null == data_config_tujians)
            {
                data_config_tujians = new Dictionary<int, config_tujian>();
                TextAsset text_asset = Resources.Load<TextAsset>("txt/config_tujian");
                string[] lines = text_asset.text.Split('\n');
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                        continue;
                    config_tujian data = config_tujian.Convert(line);
                    data_config_tujians.Add(data.m_nID, data);
                }
            }
            return data_config_tujians;
        }
    }

}
