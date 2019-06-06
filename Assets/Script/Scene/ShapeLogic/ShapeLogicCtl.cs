using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 1-小手袋
/// 2-公文包
/// 3-礼品袋
/// 4-小毛巾
/// 5-小窗帘
/// 6-小小五角星
/// 7-电梯小人儿
/// -------分割线，上面是选择，下面没有选择-------
/// 8-小推车
/// 9-快乐的小动物
/// 10-小小火车头
/// 11-小房子
/// 12-小相框
/// </summary>
public class ShapeLogicCtl : BaseScene
{
    public static ShapeLogicCtl instance = null;

    public SceneEnum mCurentGameEnum = SceneEnum.None;
    public List<int> mGuankaList = new List<int>();

    public int mdata_guanka_index = 0;
    public int mdata_guanka { get; set; }

    public SoundManager mSound { get; set; }

    public ShapeLogicGuanka1 mGuankaCtl1 { get; set; }
    public ShapeLogicGuanka2 mGuankaCtl2 { get; set; }
    public ShapeLogicGuanka3 mGuankaCtl3 { get; set; }
    public ShapeLogicGuanka4 mGuankaCtl4 { get; set; }
    public ShapeLogicGuanka5 mGuankaCtl5 { get; set; }
    public ShapeLogicGuanka6 mGuankaCtl6 { get; set; }
    public ShapeLogicGuanka7 mGuankaCtl7 { get; set; }
    public ShapeLogicGuanka8 mGuankaCtl8 { get; set; }
    public ShapeLogicGuanka9 mGuankaCtl9 { get; set; }
    public ShapeLogicGuanka10 mGuankaCtl10 { get; set; }
    public ShapeLogicGuanka11 mGuankaCtl11 { get; set; }
    public ShapeLogicGuanka12 mGuankaCtl12 { get; set; }

    void Awake()
    {
        instance = this;
        mSound = gameObject.AddComponent<SoundManager>();
        mSound.SetAbName("shapelogic_sound");

    }
    void Start ()
    {

        mSceneType = mCurentGameEnum;
        CallLoadFinishEvent();

        mGuankaCtl1 = UguiMaker.newGameObject("mGuankaCtl1", transform).AddComponent<ShapeLogicGuanka1>();
        mGuankaCtl2 = UguiMaker.newGameObject("mGuankaCtl2", transform).AddComponent<ShapeLogicGuanka2>();
        mGuankaCtl3 = UguiMaker.newGameObject("mGuankaCtl3", transform).AddComponent<ShapeLogicGuanka3>();
        mGuankaCtl4 = UguiMaker.newGameObject("mGuankaCtl4", transform).AddComponent<ShapeLogicGuanka4>();
        mGuankaCtl5 = UguiMaker.newGameObject("mGuankaCtl5", transform).AddComponent<ShapeLogicGuanka5>();
        mGuankaCtl6 = UguiMaker.newGameObject("mGuankaCtl6", transform).AddComponent<ShapeLogicGuanka6>();
        mGuankaCtl7 = UguiMaker.newGameObject("mGuankaCtl7", transform).AddComponent<ShapeLogicGuanka7>();
        mGuankaCtl8 = UguiMaker.newGameObject("mGuankaCtl8", transform).AddComponent<ShapeLogicGuanka8>();
        mGuankaCtl9 = UguiMaker.newGameObject("mGuankaCtl9", transform).AddComponent<ShapeLogicGuanka9>();
        mGuankaCtl10 = UguiMaker.newGameObject("mGuankaCtl10", transform).AddComponent<ShapeLogicGuanka10>();
        mGuankaCtl11 = UguiMaker.newGameObject("mGuankaCtl11", transform).AddComponent<ShapeLogicGuanka11>();
        mGuankaCtl12 = UguiMaker.newGameObject("mGuankaCtl12", transform).AddComponent<ShapeLogicGuanka12>();

        mGuankaCtl1.gameObject.SetActive(false);
        mGuankaCtl2.gameObject.SetActive(false);
        mGuankaCtl3.gameObject.SetActive(false);
        mGuankaCtl4.gameObject.SetActive(false);
        mGuankaCtl5.gameObject.SetActive(false);
        mGuankaCtl6.gameObject.SetActive(false);
        mGuankaCtl7.gameObject.SetActive(false);
        mGuankaCtl8.gameObject.SetActive(false);
        mGuankaCtl9.gameObject.SetActive(false);
        mGuankaCtl10.gameObject.SetActive(false);
        mGuankaCtl11.gameObject.SetActive(false);
        mGuankaCtl12.gameObject.SetActive(false);
        
        Reset();

    }

    public void SetGuanka(int guanka)
    {
        mdata_guanka = guanka;

    }
    
    
    public void Reset()
    {
        mdata_guanka_index = 0;
        SetGuanka(mGuankaList[mdata_guanka_index]);
        StartCoroutine(TStart());
        TopTitleCtl.instance.Reset();

    }

    public void GameNext()
    {
        TopTitleCtl.instance.AddStar();
        mdata_guanka_index++;
        if (mdata_guanka_index < mGuankaList.Count)
        {
            mdata_guanka = mGuankaList[mdata_guanka_index];
            SetGuanka(mdata_guanka);
            GameBegin(mdata_guanka);
        }
        else
        {
            Debug.LogError("已经通关所有"); 
        }
    }
    public void GameBegin(int guanka)
    {
        switch (guanka)
        {
            case 1:
                mGuankaCtl1.BeginGame();
                break;
            case 2:
                mGuankaCtl2.BeginGame();
                break;
            case 3:
                mGuankaCtl3.BeginGame();
                break;
            case 4:
                mGuankaCtl4.BeginGame();
                break;
            case 5:
                mGuankaCtl5.BeginGame();
                break;
            case 6:
                mGuankaCtl6.BeginGame();
                break;
            case 7:
                mGuankaCtl7.BeginGame();
                break;
            case 8:
                mGuankaCtl8.BeginGame();
                break;
            case 9:
                mGuankaCtl9.BeginGame();
                break;
            case 10:
                mGuankaCtl10.BeginGame();
                break;
            case 11:
                mGuankaCtl11.BeginGame();
                break;
            case 12:
                mGuankaCtl12.BeginGame();
                break;

        }

    }

    bool isFirstTime = true;
    IEnumerator TStart()
    {
        GameBegin(mdata_guanka);

        yield return new WaitForSeconds(4f);
        if (isFirstTime)
        {
            TopTitleCtl.instance.MoveIn();
            SoundManager.instance.PlayBgAsync("bgmusic_loop3", "bgmusic_loop3", 0.2f);
            isFirstTime = false;
        }
    }

    List<Image> temp_line_image = new List<Image>();
    public void CreateLine(float line_width, Transform line_parent, List<Vector2> poss, bool show_animtion = false)
    {
        temp_line_image.Clear();
        for (int i = 0; i < poss.Count; i++)
        {
            Image img = UguiMaker.newImage("line" + i.ToString(), line_parent, "public", "white", false);
            img.rectTransform.sizeDelta = new Vector2(line_width, 4);
            img.rectTransform.anchoredPosition = poss[i];
            img.color = new Color32(255, 255, 255, 105);
            temp_line_image.Add(img);
        }
        if(show_animtion)
        {
            StartCoroutine("TCreateLine");
        }
    }
    IEnumerator TCreateLine()
    {
        Vector2 size0 = temp_line_image[0].rectTransform.sizeDelta;
        Vector2 size1 = new Vector2(0, 4);
        for (float i = 0; i < 1f; i += 0.08f)
        {
            Vector2 size = Vector2.Lerp( size1, size0, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = 0; j < temp_line_image.Count; j++)
            {
                temp_line_image[j].rectTransform.sizeDelta = size;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < temp_line_image.Count; j++)
        {
            temp_line_image[j].rectTransform.sizeDelta = size0;
        }
    }
    public void DestroyLine()
    {
        if(0 < temp_line_image.Count)
        {
            StartCoroutine("TDestroyLine");
        }
    }
    IEnumerator TDestroyLine()
    {
        Vector2 size0 = temp_line_image[0].rectTransform.sizeDelta;
        Vector2 size1 = new Vector2(0, 4);
        for(float i = 0; i < 1f; i += 0.08f)
        {
            Vector2 size = Vector2.Lerp(size0, size1, Mathf.Sin(Mathf.PI * 0.5f * i));
            for (int j = 0; j < temp_line_image.Count; j++)
            {
                temp_line_image[j].rectTransform.sizeDelta = size;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < temp_line_image.Count; j++)
        {
            Destroy(temp_line_image[j].gameObject);
        }
        temp_line_image.Clear();
    }

}
