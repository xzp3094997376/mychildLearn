using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineUp_level1 : MonoBehaviour
{
    private LineUpCtrl mCtrl;

    private List<LineUp_Sprite> mSpriteObjList = new List<LineUp_Sprite>();
    public List<LineUp_Sprite> mTargetSprite = new List<LineUp_Sprite>();

    public LineUp_AnimalObj targetAnimal;
    /// <summary>
    /// 目标id
    /// </summary>
    public int targetID = 0;
    //左右区分
    public bool bIsStartRight = false;

    public int nAnswer1 = 0;
    public int nAnswer2 = 0;
    public int nAnswer3 = 0;

    public int nToCount = 4;
    public int nCount = 0;

    private bool bQuestionOK = false;

    public void InitAwake(LineUpCtrl _mCtrl)
    {
        mCtrl = _mCtrl;
    }

    public void ResetInfo()
    {
        mTargetSprite.Clear();
        Common.DestroyChilds(transform);
        mSpriteObjList.Clear();
        nCount = 0;
    }

    public void SetData()
    {
        ResetInfo();

        //target id
        targetID = Random.Range(2, mCtrl.nAnimalCount);
        targetAnimal = mCtrl.mAnimalObjList[targetID - 1];

        //随机左右
        if (Random.value > 0.5f)
        {
            bIsStartRight = true;
        }
        else
        {
            bIsStartRight = false;
        }

        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_numbg", true, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_add", false, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_1", false, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_add", false, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_numbg", true, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_qua", false, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_numbg", true, transform));
        mSpriteObjList.Add(mCtrl.CreateSpriteObj("xs_zhi", false, transform));
        //pos index
        List<float> findexpos = Common.GetOrderList(8, 80f);
        for (int i = 0; i < findexpos.Count; i++)
        {
            mSpriteObjList[i].transform.localPosition = new Vector3(findexpos[i], 0f, 0f);
        }

        //set quas
        mTargetSprite.Add(mSpriteObjList[0]);
        mTargetSprite.Add(mSpriteObjList[4]);
        mTargetSprite.Add(mSpriteObjList[6]);

        //右
        if (bIsStartRight)
        {           
            nAnswer1 = mCtrl.nAnimalCount - targetID;
            nAnswer2 = targetID - 1;
            nAnswer3 = mCtrl.nAnimalCount;
        }
        else
        {
            nAnswer1 = targetID - 1;
            nAnswer2 = mCtrl.nAnimalCount - targetID;
            nAnswer3 = mCtrl.nAnimalCount;
        }

        if (mTargetSprite.Count > 0)
            mTargetSprite[0].PlayTip();

        //提示音播放
        mCtrl.PlayTipSound();
    }

    LineUp_Sprite mSelect;
    LineUp_AnimalObj mSelectAnimal;
    private void Update()
    {
        if (mCtrl.bLvPass)
            return;

        if (mCtrl.mInputNumObj.gameObject.activeSelf)
        {
            return;
        }

        //if (bQuestionOK)
        //    return;

        if (Input.GetMouseButtonUp(0))
        {
            mSelect = null;
            mSelectAnimal = null;

            #region//射线检测
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (nCount < 3)
                {
                    LineUp_Sprite com = hit.collider.gameObject.GetComponent<LineUp_Sprite>();
                    if (com != null)
                    {
                        if (mTargetSprite.Count > 0)
                        {
                            if (com == mTargetSprite[0])
                            {
                                mSelect = com;
                            }
                        }
                    }

                    LineUp_AnimalObj comAnimal = hit.collider.gameObject.GetComponent<LineUp_AnimalObj>();
                    if (comAnimal != null)
                    {
                        comAnimal.PlayAnimation("face_walk", false);
                        AudioClip mcp = ResManager.GetClip("aa_animal_sound", comAnimal.strAnimalNameCN + "0");
                        if (mcp != null)
                        {
                            mCtrl.mSoundCtrl.PlaySortSound(mcp);
                        }
                    }
                }
                else
                {
                    LineUp_AnimalObj comAnimal = hit.collider.gameObject.GetComponent<LineUp_AnimalObj>();
                    if (comAnimal != null)
                    {
                        mSelectAnimal = comAnimal;
                        if (mSelectAnimal == targetAnimal)
                        {
                            mCtrl.mSoundCtrl.PlaySortSound("lineupctrl_sound", "inputnum_ok");
                            nCount++;
                            CheckPass();
                        }
                        else
                        {
                            //Debug.Log("选错，动物沮丧动作");
                            comAnimal.PlayAnimation("face_sayno", false);
                            //播放错误语音
                            AudioClip faileCp = ResManager.GetClip("lineupctrl_sound", "question_faile_2");
                            mCtrl.StopTipSound();
                            mCtrl.mSoundCtrl.PlaySound(faileCp, 1f);
                        }
                    }
                }
            }
            #endregion

            if (mSelect != null)
            {
                mCtrl.mInputNumObj.ShowEffect();
                mCtrl.mInputNumObj.transform.position = mSelect.transform.position;
                mCtrl.mInputNumObj.transform.localPosition = mCtrl.mInputNumObj.transform.localPosition + new Vector3(150f, 30f);
                mCtrl.mInputNumObj.SetInputNumberCallBack(InputNumberCallback);
            }        
        }
    }

    //数字输入回调
    private void InputNumberCallback()
    {
        int num = 0;
        string strnum = mCtrl.mInputNumObj.strInputNum;
        int.TryParse(strnum, out num);
        mSelect.SetNumber(num);
        if (nCount == 0)
        {
            CheckAnswer(num, nAnswer1);
        }
        else if (nCount == 1)
        {
            CheckAnswer(num, nAnswer2);
        }
        else if (nCount == 2)
        {
            CheckAnswer(num, nAnswer3);
        }
        mCtrl.mInputNumObj.HideEffect();
        mSelect = null;
    }
    //Q:123 答案check 
    private void CheckAnswer(int _num, int _answer)
    {
        if (_num == _answer)
        {
            mSelect.BoxActive(false);

            mTargetSprite[0].StopTip();
            mTargetSprite.Remove(mSelect);
            if (mTargetSprite.Count > 0)
                mTargetSprite[0].PlayTip();

            nCount++;
            mCtrl.mSoundCtrl.PlaySortSound("lineupctrl_sound", "inputnum_ok");

            if (ieNextQueSound != null)
                mCtrl.StopCoroutine(ieNextQueSound);
            if (nCount < 3)
            {
                ieNextQueSound = IENextQuestuin();
            }
            else
            {
                ieNextQueSound = IEToQue4();
            }
            //下一个问题语音
            mCtrl.StartCoroutine(ieNextQueSound);
        }
        else
        {
            mSelect.ShakeObj();
            //播放错误语音
            AudioClip faileCp = ResManager.GetClip("lineupctrl_sound", "question_faile_" + Random.Range(0, 3));
            mCtrl.StopTipSound();
            mCtrl.mSoundCtrl.PlaySound(faileCp, 1f);
        }
    }

    IEnumerator ieNextQueSound = null;
    //下一个问题语音
    IEnumerator IENextQuestuin()
    {
        bQuestionOK = true;
        AudioClip goodClicp = ResManager.GetClip("aa_good_sound", "goodsound" + Random.Range(0, 5));
        mCtrl.StopTipSound();
        mCtrl.mSoundCtrl.PlaySound(goodClicp, 1f);
        yield return new WaitForSeconds(goodClicp.length + 0.1f);   
        bQuestionOK = false;
        //提示音播放
        mCtrl.PlayTipSound();       
    }

    IEnumerator IEToQue4()
    {
        bQuestionOK = true;
        AudioClip goodClicp = ResManager.GetClip("aa_good_sound", "goodsound" + Random.Range(0, 5));
        mCtrl.StopTipSound();
        mCtrl.mSoundCtrl.PlaySound(goodClicp, 1f);

        mCtrl.PlayAllAnimalActions();
        float ftime = goodClicp.length;
        if (ftime < 2.8f)
            ftime = 2.8f;
        yield return new WaitForSeconds(2.8f);
        mCtrl.PlayAllAnimalIdle();
        bQuestionOK = false;
        //提示音播放
        mCtrl.PlayTipSound();
    }




    //关卡pass check
    private void CheckPass()
    {
        targetAnimal.PlayAnimation("face_sayyes", true);
        if (nCount >= nToCount)
        {
            mCtrl.bLvPass = true;
            mCtrl.StopTipSound();
            mCtrl.StartCoroutine(IEToNextLevel());
        }
    }
    IEnumerator IEToNextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("嗯嗯，就是" + targetAnimal.strAnimalName + ",因为你没有数过他呢");
        //end suc sound
        List<AudioClip> cpList = new List<AudioClip>();
        cpList.Add(ResManager.GetClip("lineupctrl_sound", "嗯嗯就是"));
        cpList.Add(ResManager.GetClip("aa_animal_name", targetAnimal.strAnimalNameCN));
        cpList.Add(ResManager.GetClip("lineupctrl_sound", "因为你没数过它"));
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }

        yield return new WaitForSeconds(0.1f);
        mCtrl.LevelCheckNext();
    }

    /// <summary>
    /// 提示音效
    /// </summary>
    /// <returns></returns>
    public IEnumerator iePlaySoundTip()
    {
        //yield return new WaitForSeconds(0.1f);
        List<AudioClip> cpList = GetTipAudioClipList();
        for (int i = 0; i < cpList.Count; i++)
        {
            mCtrl.mSoundCtrl.PlaySound(cpList[i], 1f);
            yield return new WaitForSeconds(cpList[i].length);
        }
    }



    private List<AudioClip> GetTipAudioClipList()
    {
        List<AudioClip> cpList = new List<AudioClip>();
        if (nCount == 0)
        {
            //string strLR = "";
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "排在"));
            cpList.Add(ResManager.GetClip("aa_animal_name", targetAnimal.strAnimalNameCN));
            if (bIsStartRight)
            {
                cpList.Add(ResManager.GetClip("lineupctrl_sound", "右边"));
                //strLR = "右边";
            }
            else
            {
                cpList.Add(ResManager.GetClip("lineupctrl_sound", "左边"));
                //strLR = "左边";
            }
            //cpList.Add(ResManager.GetClip("lineupctrl_sound", "的"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "有几只动物"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "把你数的结果填在"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "第一个格子里"));
            //Debug.Log("Q1:排在" + targetAnimal.strAnimalNameCN + strLR + "的有几只动物，把你数的结果用数字填写在第一个格子里");
        }
        else if (nCount == 1)
        {
            //string strLR = "";
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "排在"));
            cpList.Add(ResManager.GetClip("aa_animal_name", targetAnimal.strAnimalNameCN));
            if (bIsStartRight)
            {
                cpList.Add(ResManager.GetClip("lineupctrl_sound", "左边"));
                //strLR = "左边";
            }
            else
            {
                cpList.Add(ResManager.GetClip("lineupctrl_sound", "右边"));
                //strLR = "右边";
            }
            //cpList.Add(ResManager.GetClip("lineupctrl_sound", "的"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "有几只动物"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "把你数的结果填在"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "第二个格子里"));
            //Debug.Log("Q2:排在" + targetAnimal.strAnimalNameCN + strLR + "的有几只动物，把你数的结果用数字填写在第二个格子里");
        }
        else if (nCount == 2)
        {
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "排队做操的有几只动物"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "把你数的结果填在"));
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "第三个格子里"));
            //Debug.Log("Q3:做操的一共有几只动物，把你数的结果用数字填写在第三个格子里");
        }
        else
        {
            cpList.Add(ResManager.GetClip("lineupctrl_sound", "观察算式加一加的是"));
            //Debug.Log("Q4:观察算式，加1,加的是哪只动物？");
        }
        return cpList;
    }



}
