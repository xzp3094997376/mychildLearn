using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalNumOnlySelectPlane : MonoBehaviour
{
    Dictionary<int, AnimalNumOnlyBtn> mBtns = new Dictionary<int, AnimalNumOnlyBtn>();

    RectTransform mRtran { get; set; }
    bool isShow = false;
    public int mControlType { get; set; }

    void Awake()
    {
        mRtran = gameObject.GetComponent<RectTransform>();
        mRtran.anchoredPosition3D = new Vector3(9999, 9999, 0);
        Init();

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 6;
        gameObject.layer = LayerMask.NameToLayer("UI");

    }
    void Update()
    {
        if (AnimalNumOnlyCtl.instance.mPlace.mLock)
            return;

        //Debug.LogError(mControlType);
        if (0 == mControlType)
            Update0();
        else if (1 == mControlType)
            Update1();

    }
    void Update0()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AnimalNumOnlyBtn.gSelect = null;
            AnimalNumOnlyCell cell = null;
            AnimalNumOnlyBtn btn = null;
            foreach (RaycastHit hit in Common.getMouseRayHits())
            {
                if (null == cell)
                    cell = hit.collider.gameObject.GetComponent<AnimalNumOnlyCell>();
                if (null == btn)
                    btn = hit.collider.gameObject.transform.parent.gameObject.GetComponent<AnimalNumOnlyBtn>();
            }
            if (null == cell && btn == null)
            {
                Hide();
            }
            else if (null != btn)
            {
                //选中
            }
            else if (null != cell)
            {
                //展开
                mRtran.anchoredPosition3D = AnimalNumOnlyCtl.instance.mPlace.mRtran.anchoredPosition3D + cell.mRtran.anchoredPosition3D;// + new Vector3(150, 0, 0);
                transform.SetAsLastSibling();
                isShow = true;
                Show();

                AnimalNumOnlyCell.gSelect = cell;
                AnimalNumOnlyCell.gSelect.SetAnimal(0);

                SoundManager.instance.PlayShort("button_down");
            }
            //Debug.Log("Input.GetMouseButtonDown");

        }

        if (Input.GetMouseButton(0))
        {
            AnimalNumOnlyBtn btn = null;
            foreach (RaycastHit hit in Common.getMouseRayHits())
            {
                btn = hit.collider.gameObject.transform.parent.gameObject.GetComponent<AnimalNumOnlyBtn>();
                if (null != btn)
                {
                    break;
                }
            }
            if (null == btn)
            {
                if (null != AnimalNumOnlyBtn.gSelect)
                {
                    AnimalNumOnlyBtn.gSelect.SetUnSelect();
                }
            }
            else
            {
                if (null == AnimalNumOnlyBtn.gSelect)
                {

                }
                else
                {
                    AnimalNumOnlyBtn.gSelect.SetUnSelect();
                }
                btn.SetSelect();
            }
            AnimalNumOnlyBtn.gSelect = btn;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Input.GetMouseButtonUp");
            Hide();
            isShow = false;
            if (null != AnimalNumOnlyBtn.gSelect && null != AnimalNumOnlyCell.gSelect)
            {
                Select();
                SoundManager.instance.PlayShort("button_up");
            }
        }

    }
    void Update1()
    {
        if(!isShow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (RaycastHit h in Common.getMouseRayHits())
                {
                    AnimalNumOnlyCell.gSelect = h.collider.gameObject.GetComponent<AnimalNumOnlyCell>();
                    if (null != AnimalNumOnlyCell.gSelect)
                        break;
                }
            }

            if (Input.GetMouseButton(0))
            {
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (null != AnimalNumOnlyCell.gSelect)
                {
                    AnimalNumOnlyCell cell = null;
                    foreach (RaycastHit h in Common.getMouseRayHits())
                    {
                        cell = h.collider.gameObject.GetComponent<AnimalNumOnlyCell>();
                        if (null != cell)
                            break;
                    }
                    if (cell == AnimalNumOnlyCell.gSelect)
                    {
                        isShow = true;
                        AnimalNumOnlyCtl.instance.mUI.EnableUI(false);

                        mRtran.anchoredPosition3D = AnimalNumOnlyCtl.instance.mPlace.mRtran.anchoredPosition3D + cell.mRtran.anchoredPosition3D;
                        transform.SetAsLastSibling();

                        List<AnimalNumOnlyBtn> btns = new List<AnimalNumOnlyBtn>();
                        for(int id = 1; id <= 14; id++)
                        {
                            if(AnimalNumOnlyCtl.instance.data_animal_types.Contains(id))
                            {
                                btns.Add(mBtns[id]);
                                mBtns[id].mRtran.localScale = Vector3.one;
                                mBtns[id].gameObject.SetActive(true);
                                mBtns[id].SetColor(new Color(1, 1, 1, 1));
                            }
                            else
                            {
                                mBtns[id].gameObject.SetActive(false);
                            }
                        }
                        switch(AnimalNumOnlyCtl.instance.data_animal_types.Count)
                        {
                            case 2:
                                btns[0].mRtran.anchoredPosition3D = new Vector3(85, 0, 0);
                                btns[1].mRtran.anchoredPosition3D = new Vector3(170, 0, 0);
                                break;
                            case 4:
                                btns[0].mRtran.anchoredPosition3D = new Vector3(85, 0, 0);
                                btns[1].mRtran.anchoredPosition3D = new Vector3(170, 0, 0);
                                btns[2].mRtran.anchoredPosition3D = new Vector3(85, -86, 0);
                                btns[3].mRtran.anchoredPosition3D = new Vector3(170, -86, 0);
                                break;
                            case 6:
                                btns[0].mRtran.anchoredPosition3D = new Vector3(85, 0, 0);
                                btns[1].mRtran.anchoredPosition3D = new Vector3(170, 0, 0);
                                btns[2].mRtran.anchoredPosition3D = new Vector3(256, 0, 0);
                                btns[3].mRtran.anchoredPosition3D = new Vector3(85, -86, 0);
                                btns[4].mRtran.anchoredPosition3D = new Vector3(170, -86, 0);
                                btns[5].mRtran.anchoredPosition3D = new Vector3(256, -86, 0);
                                break;
                        }
                        AnimalNumOnlyCell.gSelect.SetAnimal(0);
                        SoundManager.instance.PlayShort("button_down");

                    }

                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                AnimalNumOnlyCell cell = null;
                foreach (RaycastHit h in Common.getMouseRayHits())
                {
                    AnimalNumOnlyBtn.gSelect = h.collider.gameObject.transform.parent.gameObject.GetComponent<AnimalNumOnlyBtn>();
                    if (null != AnimalNumOnlyBtn.gSelect)
                        break;
                    if(null == cell)
                        cell = h.collider.gameObject.GetComponent<AnimalNumOnlyCell>();
                }
                if(null != AnimalNumOnlyBtn.gSelect)
                {
                }
                else if(null != cell)
                {
                    AnimalNumOnlyCell.gSelect = cell;
                    AnimalNumOnlyCell.gSelect.SetAnimal(0);
                    mRtran.anchoredPosition3D = AnimalNumOnlyCtl.instance.mPlace.mRtran.anchoredPosition3D + cell.mRtran.anchoredPosition3D;
                }
                else
                {
                    Hide1();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                AnimalNumOnlyBtn btn = null;
                foreach (RaycastHit h in Common.getMouseRayHits())
                {
                    btn = h.collider.gameObject.transform.parent.gameObject.GetComponent<AnimalNumOnlyBtn>();
                    if (null != btn)
                        break;
                }
                if(null != btn && btn == AnimalNumOnlyBtn.gSelect)
                {
                    Select();
                    Hide1();
                    SoundManager.instance.PlayShort("button_up");
                }
                else
                {
                    Hide1();
                }


            }

        }

    }

    public void SetType(int type)
    {
        isShow = false;
        foreach (AnimalNumOnlyBtn b in mBtns.Values)
        {
            b.SetType(type);
        }
    }
    public void Select()
    {
        AnimalNumOnlyCell.gSelect.SetAnimal(AnimalNumOnlyBtn.gSelect.m_id);
        if (AnimalNumOnlyCtl.instance.mPlace.Check())
        {
            AnimalNumOnlyCtl.instance.Callback_Correct();
        }
        else
        {
            AnimalNumOnlyCtl.instance.mPlace.Check(AnimalNumOnlyCell.gSelect);
        }


    }

    public void Init()
    {
        if (mBtns.Count > 0)
            return;


        for (int id = 1; id <= 14; id++)
        {
            AnimalNumOnlyBtn btn = UguiMaker.newGameObject(id.ToString(), transform).AddComponent<AnimalNumOnlyBtn>();
            btn.Init(id);
            mBtns.Add(id, btn);
        }
    }

    public void Hide1()
    {
        foreach (AnimalNumOnlyBtn b in mBtns.Values)
        {
            b.gameObject.SetActive(false);
        }
        isShow = false;
        AnimalNumOnlyCtl.instance.mUI.EnableUI(true);
    }
    public void Hide()
    {
        StopAllCoroutines();
        if(isShow)
            StartCoroutine("THide");
        //AnimalNumOnlyCtl.instance.mUI.EnableUI(true);
    }
    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine("TShow");
        //AnimalNumOnlyCtl.instance.mUI.EnableUI(false);
    }
    IEnumerator TShow()
    {
        List<AnimalNumOnlyBtn> btns = new List<AnimalNumOnlyBtn>();
        List<Vector3> angle_end = new List<Vector3>();
        int count = 0;
        float tempf = Mathf.PI * 2f / AnimalNumOnlyCtl.instance.data_animal_types.Count;
        for (int id = 1; id <= 14; id++)
        {
            if (AnimalNumOnlyCtl.instance.data_animal_types.Contains(id))
            {
                mBtns[id].gameObject.SetActive(true);
                btns.Add(mBtns[id]);
                mBtns[id].Reset();
                mBtns[id].mRtran.anchoredPosition3D = new Vector3(Mathf.Sin(count * tempf) * 70, Mathf.Cos(count * tempf) * 70, 0);
                count++;
            }
            else
            {
                mBtns[id].gameObject.SetActive(false);
            }
        }

        for (float i = 0; i < 1f; i += 0.2f)
        {
            for (int j = 0; j < btns.Count; j++)
            {
                btns[j].mRtran.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 1), Vector3.one, i);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int j = 0; j < btns.Count; j++)
        {
            btns[j].mRtran.localScale = Vector3.one;
        }

        
    }
    IEnumerator THide()
    {
        List<AnimalNumOnlyBtn> btns = new List<AnimalNumOnlyBtn>();
        for (int id = 1; id <= 14; id++)
        {
            if (mBtns[id].gameObject.activeSelf)
            {
                btns.Add(mBtns[id]);
            }
        }
        if (btns.Count == 0)
            yield break;
        while (true)
        {
            Color cor = btns[0].GetColor() - new Color(0, 0, 0, 0.1f);

            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].SetColor(cor);
            }

            if (cor.a < 0.05f)
                break;

            yield return new WaitForSeconds(0.01f);

        }
        for (int i = 0; i < btns.Count; i++)
        {
            btns[i].gameObject.SetActive(false);
        }


    }

}
