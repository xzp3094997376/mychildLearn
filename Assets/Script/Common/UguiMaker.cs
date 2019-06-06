using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UguiMaker// : MonoBehaviour
{

    public static GameObject InitGameObj(GameObject tar, Transform _parent, string _name, Vector3 _pos, Vector3 _scale)
    {
        RectTransform rtran = tar.GetComponent<RectTransform>();
        if (null == rtran)
            rtran = tar.AddComponent<RectTransform>();

        tar.name = _name;
        tar.transform.SetParent(_parent);
        rtran.anchoredPosition3D = _pos;
        tar.transform.localScale = _scale;

        return tar;
    }

    public static GameObject newGameObject(string n, Transform p)
    {
        RectTransform rtran = new GameObject().AddComponent<RectTransform>(); ;
        rtran.gameObject.name = n;
        rtran.SetParent(p);
        rtran.localScale = Vector3.one;
        rtran.anchoredPosition3D = Vector3.zero;
        return rtran.gameObject;
    }

    public static Image newImage(string _name, Transform _parent, string atlas_name, string sprite_name, bool raycast = true)
    {
        GameObject obj = newGameObject(_name, _parent);
        Image result = obj.AddComponent<Image>();
        result.sprite = ResManager.GetSprite(atlas_name, sprite_name);

        result.SetNativeSize();
        result.raycastTarget = raycast;
        return result;
    }

    public static RawImage newRawImage(string _name, Transform _parent, string ab_name, string texture_name, bool raycast = true)
    {
        GameObject obj = newGameObject(_name, _parent);
        RawImage result = obj.AddComponent<RawImage>();
        result.texture = ResManager.GetTexture(ab_name, texture_name);

        result.SetNativeSize();
        result.raycastTarget = raycast;
        return result;
    }

    public static RawImage newRawImage(string _name, Transform _parent, string texture_name, bool raycast = true)
    {
        GameObject obj = newGameObject(_name, _parent);
        RawImage result = obj.AddComponent<RawImage>();
        result.texture = ResManager.GetTextureInResources(texture_name);

        result.SetNativeSize();
        result.raycastTarget = raycast;
        return result;
    }

    public static Button newButton(string _name, Transform _parent, string atlas_name, string sprite_name)
    {
        Image img = newImage(_name, _parent, atlas_name, sprite_name);
        Button btn = img.gameObject.AddComponent<Button>();

        return btn;
    }

    public static Text newText(string _name, Transform _parent)
    {
        GameObject obj = newGameObject(_name, _parent);
        Text text = obj.gameObject.AddComponent<Text>();
        text.font = Global.instance.mFontHwakang;

        return text;

    }

    public static RectTransform setRTran(RectTransform rtran, UguiAnchorEnum anchor_enum)
    {
        switch (anchor_enum)
        {
            case UguiAnchorEnum.width:
                rtran.anchorMin = new Vector2(0, 0.5f);
                rtran.anchorMax = new Vector2(1, 0.5f);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;
            case UguiAnchorEnum.height:
                rtran.anchorMin = new Vector2(0.5f, 0);
                rtran.anchorMax = new Vector2(0.5f, 1);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;

            case UguiAnchorEnum.width_and_height:
                rtran.anchorMin = new Vector2(0, 0);
                rtran.anchorMax = new Vector2(1, 1);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;

            case UguiAnchorEnum.center:
                rtran.anchorMin = new Vector2(0.5f, 0.5f);
                rtran.anchorMax = new Vector2(0.5f, 0.5f);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;

            case UguiAnchorEnum.up:
                rtran.anchorMin = new Vector2(0.5f, 1);
                rtran.anchorMax = new Vector2(0.5f, 1);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;

            case UguiAnchorEnum.bottom:
                rtran.anchorMin = new Vector2(0.5f, 0);
                rtran.anchorMax = new Vector2(0.5f, 0);
                rtran.pivot = new Vector2(0.5f, 0.5f);
                break;
        }

        return rtran;
    }

    public static RectTransform setOffect(RectTransform rtran, float minx, float miny, float maxx, float maxy)
    {
        rtran.offsetMin = new Vector2(minx, miny);
        rtran.offsetMax = new Vector2(maxx, maxy);
        return rtran;
    }

    public static ParticleSystem newParticle(string _name, Transform _parent, Vector3 _pos, Vector3 _scale, string ab_name, string prefab_name)
    {
        return InitGameObj(ResManager.GetPrefab(ab_name, prefab_name), _parent, _name, _pos, _scale).GetComponent<ParticleSystem>();
    }
    
    public static void newGameObjects(Transform p, params object[] param)
    {
        for(int i = 0; i < param.Length; i++)
        {
            newGameObject(param[i].ToString(), p).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                GlobalParam.screen_width, GlobalParam.screen_height);
        }
    }

}
