using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalNumOnlyPlace : MonoBehaviour
{

    public RectTransform mRtran { get; set; }
    Image mBg { get; set; }
    AnimalNumOnlyCell[,] mCells = null;//前面表示行，后面表示列
    Dictionary<int, List<int>> mTempPosIndex = new Dictionary<int, List<int>>();//计算刷新的时候，缓存每个格子还能填什么数字，key十位数是行个位是列，value还能填什么数
    List<int> mTempTipKey = new List<int>();//提示，key十位数是行个位是列
    Button mButtonClick { get; set; }

    int data_place_num = 6;
    int data_bg_width = 560;
    int data_cell_width = 90;
    public bool mLock { get; set; }


    bool CellMatchId(int y, int x, int animal_id)//检测这个空格能否填这个动物id
    {
        if (mCells[y, x].data_id != 0)
            return false;//名花有主

        for(int i = 0; i < data_place_num; i++)
        {
            if (mCells[y, i].data_id == animal_id)
                return false;
            if (mCells[i, x].data_id == animal_id)
                return false;
        }
        return true;
    }
    List<int> GetCells(int animal_id)
    {
        List<int> result = new List<int>();
        List<int> list_countx = new List<int>() { 0, 0, 0, 0, 0, 0 };//列
        List<int> list_county = new List<int>() { 0, 0, 0, 0, 0, 0 };//行

        bool[,] tempbools = new bool[data_place_num, data_place_num];
        for (int i = 0; i < data_place_num; i++)
        {
            for (int j = 0; j < data_place_num; j++)
            {
                if(CellMatchId(i, j, animal_id))
                {
                    tempbools[i, j] = true;
                    list_countx[j]++;
                    list_county[i]++;
                }
                else
                {
                    tempbools[i, j] = false;
                }
            }
        }
        //DebugList(list_countx);
        //DebugList(list_county);

        for(int cell_count = 1; cell_count <= data_place_num; cell_count++)
        {
            //cell_count表示这行或者这列只有cell_count个空格可以填
            //行
            for (int index = 0; index < list_county.Count; index++)
            {
                if(list_county[index] == cell_count)
                {
                    for (int i = 0; i < data_place_num; i++)
                    {
                        if(tempbools[index, i])
                        {
                            int pos_id = index * 10 + i;
                            if(!result.Contains(pos_id))
                            {
                                result.Add(pos_id);
                            }
                        }
                    }
                }
            }

            //列
            for (int index = 0; index < list_countx.Count; index++)
            {
                if (list_countx[index] == cell_count)
                {
                    for (int i = 0; i < data_place_num; i++)
                    {
                        if (tempbools[i, index])
                        {
                            int pos_id = i * 10 + index;
                            if (!result.Contains(pos_id))
                            {
                                result.Add(pos_id);
                            }
                        }
                    }
                }
            }

            if (0 < result.Count)
                return result;

        }


        return result;

    }
    public void Flush()
    {
        if(null != mButtonClick)
            mButtonClick.gameObject.SetActive(false);

        if (null == mRtran)
        {
            mCells = new AnimalNumOnlyCell[data_place_num, data_place_num];

            mRtran = gameObject.GetComponent<RectTransform>();


            mBg = UguiMaker.newImage("bg", transform, "animalnumonly_sprite", "place_frame", false);
            mBg.type = Image.Type.Sliced;
            mBg.rectTransform.sizeDelta = new Vector2(data_bg_width + 14, data_bg_width + 14);

            float subw = (data_bg_width - data_cell_width * data_place_num) / (data_place_num + 1f);
            float begx = (data_cell_width * data_place_num + subw * (data_place_num - 1)) * -0.5f + data_cell_width * 0.5f;
            Vector2 begin_pos = new Vector2(begx, begx);

            for (int i = 0; i < data_place_num; i++)
            {
                for(int j = 0; j < data_place_num; j++)
                {
                    AnimalNumOnlyCell cell = UguiMaker.newGameObject(i.ToString() + "-" + j.ToString(), transform).AddComponent<AnimalNumOnlyCell>();
                    cell.Init();
                    cell.SetPos(begin_pos + new Vector2( j * (data_cell_width + subw), i * (data_cell_width + subw)), j, i);
                    cell.SetSize(data_cell_width, data_cell_width);
                    mCells[i,j] = cell;

                    mTempPosIndex.Add(i * 10 + j, new List<int>());
                }
            }
        }

        for (int i = 0; i < data_place_num; i++)
        {
            for (int j = 0; j < data_place_num; j++)
            {
                mCells[i, j].SetAnimal(0);
                mCells[i, j].SetCorrectId(0);
                mCells[i, j].SetBoxEnable(true);
                mCells[i, j].SetLock(0);
            }
        }

        List<int> temp_pos = new List<int>();

        #region 挑可选项最少法
        foreach(int animal_id in AnimalNumOnlyCtl.instance.data_animal_types)
        {
            for (int i = 0; i < data_place_num; i++)
            {
                List<int> list = GetCells(animal_id);
                if (list.Count == 0)
                    break;
                int pos_id = list[Random.Range(0, 1000) % list.Count];
                int y = pos_id / 10;
                int x = pos_id % 10;
                mCells[y, x].SetAnimal(animal_id);
                mCells[y, x].SetCorrectId(animal_id);
                mCells[y, x].SetBoxEnable(false);
                mCells[y, x].SetLock(1);

                if(!temp_pos.Contains(pos_id))
                    temp_pos.Add(pos_id);
                
            }

        }
        #endregion

        for(int i = 0; i < AnimalNumOnlyCtl.instance.data_answer_num; i++)
        {
            int index = Random.Range(0, 1000) % temp_pos.Count;
            int pos_id = temp_pos[index];
            int y = pos_id / 10;
            int x = pos_id % 10;
            mCells[y, x].SetAnimal(0);
            mCells[y, x].SetBoxEnable(true);
            mCells[y, x].SetLock(2);
            //Debug.Log(pos_id);
            temp_pos.RemoveAt(index);

        }
        

        AnimalNumOnlyCtl.instance.mUI.SetTip(mTempTipKey.Count);
        //AnimalNumOnlyCtl.instance.mUI.SetTime("88:88");
        mLock = false;

    }



    public void SetCellEnable(bool _enable)
    {
        for(int i = 0; i < data_place_num; i++)
        {
            for(int j = 0; j < data_place_num; j++)
            {
                mCells[i, j].SetBoxEnable(_enable);
            }
        }
    }
    public void ClearCells()
    {
        for(int i = 0; i < data_place_num; i++)
        {
            for (int j = 0; j < data_place_num; j++)
            {
                mCells[i, j].SetAnimal(0);
            }
        }
    }
    public void Tip()
    {
        for (int i = 0; i < data_place_num; i++)
        {
            for (int j = 0; j < data_place_num; j++)
            {
                if(0 != mCells[i, j].data_correct_id &&
                    mCells[i, j].data_correct_id != mCells[i, j].data_id)
                {
                    mCells[i, j].SetAnimal(mCells[i, j].data_correct_id);
                    if (Check())
                    {
                        AnimalNumOnlyCtl.instance.Callback_Correct();
                    }
                    else
                    {
                        Check(mCells[i, j]);
                    }
                    Global.instance.PlayBtnClickAnimation(mCells[i, j].transform);
                    return;
                }
            }
        }
    }



    List<int> temp_check_list1 = new List<int>();
    List<int> temp_check_list2 = new List<int>();
    List<AnimalNumOnlyCell> temp_check_error = new List<AnimalNumOnlyCell>();
    public bool Check()
    {
        //temp_check_error.Clear();
        //bool result = true;
        for(int i = 0; i < data_place_num; i++)
        {
            temp_check_list1.Clear();
            temp_check_list2.Clear();
            for (int j = 0; j < data_place_num; j++)
            {
                int id = mCells[i, j].data_id;
                if (temp_check_list1.Contains(id))
                {
                    if(0 != id)
                    {
                        //result = false;
                        //temp_check_error.Add(mCells[i, j]);
                        return false;
                    }
                }
                else
                {
                    if(0 != id)
                        temp_check_list1.Add(id);
                }

                id = mCells[j, i].data_id;
                if (temp_check_list2.Contains(id))
                {
                    if (0 != id)
                    {
                        //result = false;
                        //temp_check_error.Add(mCells[j, i]);
                        return false;
                    }
                }
                else
                {
                    if (0 != id)
                        temp_check_list2.Add(id);
                }


            }

            if (temp_check_list1.Count != AnimalNumOnlyCtl.instance.data_animal_types.Count ||
                temp_check_list2.Count != AnimalNumOnlyCtl.instance.data_animal_types.Count)
                //result = false;
                return false;

        }

        //for(int i = 0; i < temp_check_error.Count; i++)
        //{
        //    temp_check_error[i].ShowError();
        //}
        //return result;
        mLock = true;
        return true;

    }
    public void Check(AnimalNumOnlyCell cell)
    {
        temp_check_error.Clear();
        int id = cell.data_id;
        for(int i = 0; i < data_place_num; i++)
        {
            if(cell != mCells[cell.data_y, i] && mCells[cell.data_y, i].data_id == id)
            {
                temp_check_error.Add(mCells[cell.data_y, i]);
            }
            if(cell != mCells[i, cell.data_x] && mCells[i, cell.data_x].data_id == id)
            {
                temp_check_error.Add(mCells[i, cell.data_x]);
            }
        }

        if(0 < temp_check_error.Count)
        {
            for (int i = 0; i < temp_check_error.Count; i++)
            {
                temp_check_error[i].ShowError();
            }
            cell.ShowError();
            AnimalNumOnlyCtl.instance.mSound.PlayTipList(
                new List<string>() { "animalnumonly_sound", "aa_animal_name" },
                new List<string>() { "这一行已经有", MDefine.GetAnimalNameByID_CH(cell.data_id) },
                new List<float>() { 0.5f, 1f });
                
        }

    }


    public void OnClick()
    {
        //SoundManager.instance.PlayShort("按钮1");
        AnimalNumOnlyCtl.instance.mSound.PlayShortDefaultAb("方块下坠");
        //mEffect.Stop();
        mButtonClick.gameObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine("TPlayGodown");
        StartCoroutine("TResetScene");

        AnimalNumOnlyCtl.instance.mUI.m_can_click_replay = false;

    }



    public void PlayCorrect()
    {
        //Debug.Log("PlayCorrect()");
        StartCoroutine("TPlayCorrect");
    }
    public void StopCorrect()
    {
        StopCoroutine("TPlayCorrect");
    }
    IEnumerator TPlayCorrect()
    {
        if(null == mButtonClick)
        {
            mButtonClick = UguiMaker.newButton("click", transform, "public", "white");
            mButtonClick.image.rectTransform.sizeDelta = new Vector2(600, 600);
            mButtonClick.image.color = new Color(1, 1, 1, 0);
            mButtonClick.onClick.AddListener(OnClick);

        }
        mButtonClick.gameObject.SetActive(true);

        for (float i = 0; i < 1f; i += 0.08f)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.4f, 0.4f, 1), i);
            //transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 20), i);
            AnimalNumOnlyCtl.instance.mRtran.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(529, -11.5f), i);
            AnimalNumOnlyCtl.instance.mRtran.localScale = Vector3.Lerp(Vector2.one, new Vector3(2.2f, 2.2f, 1), i);
            
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                mCells[i, j].PlaySpine();
            }
        }

        Vector3 pos = mRtran.anchoredPosition3D;
        Vector3 scale = transform.localScale;
        float p = 0;
        float p1 = 0;
        while(true)
        {
            mRtran.anchoredPosition3D = pos + new Vector3(0, Mathf.Sin(p) * 15, 0);
            yield return new WaitForSeconds(0.01f);
            p += 0.03f;
            //p1 += 0.1f;
        }
        

    }

    public void PlayGoUp()
    {
        StartCoroutine("TPlayGoUp");
    }
    IEnumerator TPlayGodown()
    {
        Vector3 speed = new Vector3(0, 0, 0);
        while(mRtran.anchoredPosition3D.y > -550)
        {
            mRtran.anchoredPosition3D += speed;
            yield return new WaitForSeconds(0.01f);
            speed.y -= 1;
        }
        AnimalNumOnlyCtl.instance.JumpAnimal();

    }
    IEnumerator TPlayGoUp()
    {
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;

        Vector3 pos0 = AnimalNumOnlyCtl.instance.data_place_pos - new Vector3(0, 800, 0);
        for(float i = 0; i < 1f; i += 0.05f)
        {
            mRtran.anchoredPosition3D = Vector3.Lerp(pos0, AnimalNumOnlyCtl.instance.data_place_pos, Mathf.Sin(Mathf.PI * 0.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
        mRtran.anchoredPosition3D = AnimalNumOnlyCtl.instance.data_place_pos;

    }
    IEnumerator TResetScene()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            AnimalNumOnlyCtl.instance.mRtran.anchoredPosition = Vector2.Lerp(new Vector2(529, -11.5f), Vector2.zero, i);
            AnimalNumOnlyCtl.instance.mRtran.localScale = Vector3.Lerp( new Vector3(2.2f, 2.2f, 1), Vector2.one, i);

            yield return new WaitForSeconds(0.01f);

        }
        AnimalNumOnlyCtl.instance.mRtran.anchoredPosition = Vector2.zero;
        AnimalNumOnlyCtl.instance.mRtran.localScale = Vector3.one;
    }

}
