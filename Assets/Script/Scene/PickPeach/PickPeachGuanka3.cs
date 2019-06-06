using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickPeachGuanka3 : MonoBehaviour {

    Button mBtnOK { get; set; }

    Vector3[] mStandyPos = new Vector3[3];
    Dictionary<PickPeachLanzi, Vector3> mDicPos = new Dictionary<PickPeachLanzi, Vector3>();

    bool mdata_can_control { get; set; }
    int mdata_sort_type = 0;//0从少到多，1从多到少

    // Use this for initialization
    void Start () {
	
	}

    //彩虹半径是1833
    void Update ()
    {
        if (3 != PickPeachCtl.instance.mdata_guanka || !mdata_can_control)
            return;

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                PickPeachLanzi.gSelect = hit.collider.gameObject.GetComponent<PickPeachLanzi>();
                PickPeachLanzi.gSelect.transform.SetAsLastSibling();
                PickPeachLanzi.gSelect.SetBoxEnable(false);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (null != PickPeachLanzi.gSelect)
            {
                //选项
                Vector3 pos = GetCaihongLanziPos(transform.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x);
                PickPeachLanzi.gSelect.mRtran.anchoredPosition3D = pos;

                PickPeachLanzi lanzi = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    lanzi = hit.collider.gameObject.GetComponent<PickPeachLanzi>();
                }
              
                if(null != lanzi)
                {
                    //Debug.Log("交换位置");
                    PickPeachCtl.instance.mSound.PlayShortDefaultAb("09-连上对调位置");
                    StartCoroutine(TMoveLanzi(lanzi, mDicPos[lanzi], mDicPos[PickPeachLanzi.gSelect]));
                    Vector3 temp_pos = mDicPos[lanzi];
                    mDicPos[lanzi] = mDicPos[PickPeachLanzi.gSelect];
                    mDicPos[PickPeachLanzi.gSelect] = temp_pos;
                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(null != PickPeachLanzi.gSelect)
            {
                StartCoroutine(TMoveLanzi(PickPeachLanzi.gSelect, PickPeachLanzi.gSelect.mRtran.anchoredPosition3D, mDicPos[PickPeachLanzi.gSelect]));
                PickPeachLanzi.gSelect.SetBoxEnable(true);
                PickPeachLanzi.gSelect = null;

            }

        }

    }

    public Vector3 GetCaihongLanziPos(float posx)
    {
        return new Vector3(posx, Mathf.Sqrt(1833 * 1833 - posx * posx) + 70, 0);
    }
    


    public void EndGame()
    {
        mdata_can_control = false;
    }
    public void BeginGame()
    {
        mStandyPos[0] = PickPeachCtl.instance.mLanzi[0].mRtran.anchoredPosition3D;
        mStandyPos[1] = PickPeachCtl.instance.mLanzi[1].mRtran.anchoredPosition3D;
        mStandyPos[2] = PickPeachCtl.instance.mLanzi[2].mRtran.anchoredPosition3D;

        mDicPos.Clear();
        mDicPos.Add(PickPeachCtl.instance.mLanzi[0], mStandyPos[0]);
        mDicPos.Add(PickPeachCtl.instance.mLanzi[1], mStandyPos[1]);
        mDicPos.Add(PickPeachCtl.instance.mLanzi[2], mStandyPos[2]);

        mdata_sort_type = Random.Range(0, 1000) % 2;
        if(mdata_sort_type == 0)
        {
            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip从少到多", 1, true);
        }
        else
        {
            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip从多到少", 1, true);
        }
        StartCoroutine(TBeginGame());

    }
    IEnumerator TBeginGame()
    {
        mBtnOK = PickPeachCtl.instance.mGuanka1.GetBtnOK();
        mBtnOK.gameObject.SetActive(true);
        mBtnOK.transform.SetAsLastSibling();
        mBtnOK.onClick.RemoveAllListeners();
        mBtnOK.onClick.AddListener(OnClkBtnOK);
        mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_up");

        Vector2 pos0 = new Vector2(561, -500);
        Vector2 pos1 = new Vector2(561, -348);
        for (float i = 0; i < 1f; i += 0.07f)
        {
            mBtnOK.image.rectTransform.anchoredPosition = Vector2.Lerp(pos0, pos1, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }

        mdata_can_control = true;
    }


    public void OnClkBtnOK()
    {

        if (!mdata_can_control)
            return;

        mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_down");
        CancelInvoke();
        Invoke("InvokeOnClkBtn", 0.3f);
        PickPeachCtl.instance.mSound.PlayShort("button_up");

    }
    void InvokeOnClkBtn()
    {
        bool correct = true;
        PickPeachLanzi[] lanzi = PickPeachCtl.instance.mLanzi;

        PickPeachLanzi left = lanzi[0];
        PickPeachLanzi right = lanzi[0];
        for (int i = 1; i < 3; i++)
        {
            if (lanzi[i].mRtran.anchoredPosition3D.x < left.mRtran.anchoredPosition3D.x)
            {
                left = lanzi[i];
            }
            if (lanzi[i].mRtran.anchoredPosition3D.x > right.mRtran.anchoredPosition3D.x)
            {
                right = lanzi[i];
            }
        }


        //从少到多排序
        if (0 == mdata_sort_type)
        {
            if (left.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumSmall() &&
                 right.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumBig())
            {

            }
            else
            {
                correct = false;
            }

        }
        else//从多到少排序
        {
            if(left.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumBig() && 
                right.mdata_flower_number == PickPeachCtl.instance.GetFlowerNumSmall())
            {

            }
            else
            {
                correct = false;
            }

        }

        if (correct)
        {
            mdata_can_control = false;
            PickPeachCtl.instance.mGuanka1.mEffectOK.Play();
            PickPeachCtl.instance.callbackGuanka3_over();
            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip你真是一个排序小能手");

        }
        else
        {
            lanzi[0].Shake();
            lanzi[1].Shake();
            lanzi[2].Shake();
            PickPeachCtl.instance.mSound.PlayTipDefaultAb("tip不对不对你再仔细数数");

            PickPeachCtl.instance.mSound.PlayShort("button_down");
            mBtnOK.image.sprite = ResManager.GetSprite("pickpeach_sprite", "btn_up");

        }

    }


    IEnumerator TMoveLanzi(PickPeachLanzi lanzi, Vector3 begin_pos, Vector3 end_pos)
    {
        lanzi.SetBoxEnable(false);
        for(float i = 0; i < 1f; i += 0.08f)
        {
            Vector3 pos = Vector3.Lerp(begin_pos, end_pos, i);
            lanzi.mRtran.anchoredPosition3D = GetCaihongLanziPos(pos.x);
            yield return new WaitForSeconds(0.01f);
        }
        lanzi.mRtran.anchoredPosition3D = end_pos;
        lanzi.SetBoxEnable(true);

    }

}
