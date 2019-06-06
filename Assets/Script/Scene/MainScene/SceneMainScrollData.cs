using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneMainScrollData
{
    public string title = string.Empty;
    public List<string> game_names = new List<string>();
    public List<string> sprite_names = new List<string>();

    public SceneMainScrollData(string _title, List<string> _game_names, List<string> _sprite_names)
    {
        Init(_title, _game_names, _sprite_names);
    }

    public void Init(string _title, List<string> _game_names, List<string> _sprite_names)
    {
        title = _title;
        game_names = _game_names;
        sprite_names = _sprite_names;

    }


}
