using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class RM_KeDou : MonoBehaviour
{

    private Image imgRang;
    private Image imgKedou;
    private SkeletonGraphic mspine;

    private Vector3 vstart = new Vector3(-280f, 280f, 0f);
    private RegularMazeCtrl mCtrl;

    AudioSource mAudioSource;
    AudioClip moveClip;

    public void InitAwake()
    {
        mCtrl = SceneMgr.Instance.GetNowScene() as RegularMazeCtrl;

        imgRang = UguiMaker.newImage("rang", transform, "regularmaze_sprite", "rang", false);
        imgKedou = UguiMaker.newImage("kedou", transform, "regularmaze_sprite", "kedou", false);
        imgKedou.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
        imgKedou.enabled = false;

        transform.localPosition = vstart;

        mspine = ResManager.GetPrefab("regularmaze_prefab", "kedou", transform).GetComponent<SkeletonGraphic>();
        mspine.transform.localPosition = new Vector3(-45f, -34f, 0f);
        mspine.transform.localEulerAngles = new Vector3(0f, 0f, -45f);
        mspine.transform.localScale = new Vector3(-1f, 1f, 1f);
        imgRang.transform.SetSiblingIndex(3);

        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.playOnAwake = false;
        mAudioSource.loop = true;
        mAudioSource.Stop();
        mAudioSource.volume = 0.4f;
        moveClip = ResManager.GetClip("regularmaze_sound", "kedoumove");
        mAudioSource.clip = moveClip;
    }


    public void PlayMoveSound(bool _play)
    {
        if (_play)
            mAudioSource.Play();
        else
            mAudioSource.Stop();
    }

    public void ResetInfos()
    {
        transform.localPosition = vstart;
        transform.localEulerAngles = new Vector3(0f, 0f, -45f);
        PlayAnimation("Idle", true);
        imgRang.enabled = true;
        PlayMoveSound(false);
    }


    /// <summary>
    /// 移动到青蛙
    /// </summary>
    public void MoveToQingwa(System.Action _callback = null)
    {
        mKedouMoveCallback = _callback;

        List<RM_Block> mroadList = mCtrl.roadList;
        vpaths.Clear();
        nindex = 0;

        for (int i = 0; i < mroadList.Count; i++)
        {
            vpaths.Add(mroadList[i].transform.localPosition);
        }

        mCtrl.SoundCtrl.PlaySortSound(ResManager.GetClip("regularmaze_sound", "paopaobreak"));
        Movemove();
    }

    int nindex = 0;
    List<Vector3> vpaths= new List<Vector3>();
    private System.Action mKedouMoveCallback = null;

    private void Movemove()
    {
        RM_Block target = mCtrl.roadList[nindex];
        Vector3 vdir = (target.transform.localPosition - transform.localPosition).normalized;
        transform.right = vdir * 1f;

        PlayAnimation("Click", true);
        imgRang.enabled = false;

        PlayMoveSound(true);
        transform.DOLocalMove(vpaths[nindex], 0.5f).SetEase(Ease.Linear).OnComplete(() => 
        {
            nindex++;
            if (nindex < vpaths.Count)
            {
                Movemove();
            }
            else
            {
                //Debug.Log("move over");
                if (mKedouMoveCallback != null)
                {
                    mKedouMoveCallback();
                    PlayAnimation("Idle", true);
                    PlayMoveSound(false);
                }
            }
        });
    }

    /// <summary>
    /// 播放动画 Idle/Click
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name, bool _loop)
    {
        mspine.AnimationState.SetAnimation(1, _name, _loop);
    }

}

public class RM_QingWa : MonoBehaviour
{

    private SkeletonGraphic mspine;
    private Vector3 vstart = new Vector3(280f, -275f, 0f);
    private Image imgheye;

    AudioSource mAudioSource;
    AudioClip jumpClip;

    public void InitAwake()
    {
        transform.localPosition = vstart;

        imgheye = UguiMaker.newImage("heye", transform, "regularmaze_sprite", "heye3", false);
        imgheye.transform.localPosition = new Vector3(0f, -55f, 0f);

        mspine = ResManager.GetPrefab("regularmaze_prefab", "qingwa", transform).GetComponent<SkeletonGraphic>();
        mspine.transform.localPosition = new Vector3(0f, -80f, 0f);
        mspine.transform.localScale = Vector3.one * 1.2f;

        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.playOnAwake = false;
        mAudioSource.loop = true;
        mAudioSource.Stop();
        mAudioSource.volume = 0.4f;
        jumpClip = ResManager.GetClip("regularmaze_sound", "qingwajump");
        mAudioSource.clip = jumpClip;
    }

    public void ResetInfos()
    {
        PlayAnimation("Idle", true);
    }


    /// <summary>
    /// 播放动画 Idle/Click
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_loop"></param>
    public void PlayAnimation(string _name, bool _loop)
    {
        mspine.AnimationState.SetAnimation(1, _name, _loop);
    }

    IEnumerator iePlayJump = null;
    public void PlayJumpSound(bool _play)
    {
        if (_play)
        {
            iePlayJump = iePlayJumpSound();
            StartCoroutine(iePlayJump);
        }
        else
        {
            if (iePlayJump != null)
                StopCoroutine(iePlayJump);
            mAudioSource.Stop();
        }
    }
    IEnumerator iePlayJumpSound()
    {
        mAudioSource.loop = false;
        mAudioSource.clip = jumpClip;
        mAudioSource.Play();
        yield return new WaitForSeconds(0.53f);
        PlayJumpSound(true);
    }
    
}

