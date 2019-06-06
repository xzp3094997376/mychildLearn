using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneSceneDrawCtl : MonoBehaviour
{
    RawImage mRawImg;
    Texture2D mDrawTex { get; set; }
    Color mBrushColor = Color.red;
    int mBrushHalf = 10;

    void Start () {
        mRawImg = UguiMaker.newGameObject("raw_img", transform).AddComponent<RawImage>();
        mRawImg.rectTransform.sizeDelta = new Vector2(1423, 800);
        

        mDrawTex = new Texture2D(1423, 800);
        Color32[] all_color = mDrawTex.GetPixels32();
        for (int i = 0; i < all_color.Length; i++)
            all_color[i] = new Color32(255, 255, 255, 255);
        mDrawTex.SetPixels32(all_color);
        mDrawTex.Apply();
         
        mRawImg.texture = mDrawTex;

    }


    Vector3 temp_end_offset = Vector3.zero;
    void LateUpdate()
    {
        Vector3 offset = Common.getMouseLocalPos(transform) + new Vector3(711.5f, 400, 0);
        if(Input.GetMouseButtonDown(0))
        {
            temp_end_offset = offset;
        }
        else if (Input.GetMouseButton(0))
        {
            int temp_x = -1;
            int temp_y = -1;
            int x = -1;
            int y = -1;
            for(float i = 0; i < 1f; i += 0.01f)
            {
                Vector3 pos = Vector3.Lerp(temp_end_offset, offset, i);
                x = Mathf.FloorToInt(pos.x);
                y = Mathf.FloorToInt(pos.y);
                if(x != temp_x || y != temp_y)
                {
                    DrawPoint(x, y);
                    temp_x = x;
                    temp_y = y;
                }
            }
            mDrawTex.Apply();
            temp_end_offset = offset;
        }


    }

    void DrawPoint(Vector3 texture_offset)
    {
        int x = Mathf.FloorToInt(texture_offset.x);
        int y = Mathf.FloorToInt(texture_offset.y);
        int broder = mBrushHalf * mBrushHalf;
        for (int i = -mBrushHalf; i < 2 * mBrushHalf; i++)
        {
            for (int j = -mBrushHalf; j < 2 * mBrushHalf; j++)
            {
                if(i * i + j * j < broder)
                {
                    mDrawTex.SetPixel( x + i, y + j, mBrushColor);
                }
            }
        }
    }
    void DrawPoint(int x, int y)
    {
        int broder = mBrushHalf * mBrushHalf;
        for (int i = -mBrushHalf; i < 2 * mBrushHalf; i++)
        {
            for (int j = -mBrushHalf; j < 2 * mBrushHalf; j++)
            {
                if (i * i + j * j < broder)
                {
                    mDrawTex.SetPixel(x + i, y + j, mBrushColor);
                }
            }
        }
    }

}
