using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundTipData
{
    public List<string> ab_names;
    public List<string> sound_names;
    public string play_type;//only,short,list
    public SoundManager play_com { get; set; }
    public System.Action callback_func;

    public SoundTipData()
    {
        ab_names = new List<string>();
        sound_names = new List<string>();
        play_type = "";
        callback_func = null;
        play_com = null;
    }
    public void Clean()
    {
        ab_names.Clear();
        sound_names.Clear();
        play_type = "none";
        play_com = null;
        callback_func = null;
    }



    public void SetData(SoundManager _play_com, string _ab_name, string _sound_name)
    {
        ab_names.Clear();
        sound_names.Clear();
        callback_func = null;

        play_type = "tip";
        play_com = _play_com;
        ab_names.Add(_ab_name);
        sound_names.Add(_sound_name);

    }
    public void SetData(SoundManager _play_com, List<string> _ab_names, List<string> _sound_names)
    {
        ab_names.Clear();
        sound_names.Clear();
        callback_func = null;

        play_type = "list";
        play_com = _play_com;
        ab_names = _ab_names;
        sound_names = _sound_names;

    }
    public void SetData(System.Action callback_func)
    {
        ab_names.Clear();
        sound_names.Clear();
        play_type = "callback";
        play_com = null;
        this.callback_func = callback_func;

    }

   

}
