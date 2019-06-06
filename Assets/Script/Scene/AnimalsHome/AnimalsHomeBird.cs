using UnityEngine;
using System.Collections;
using DG.Tweening;
using Spine.Unity;

public class AnimalsHomeBird : MonoBehaviour
{

    private SkeletonGraphic spine;

    private AudioSource maudioSource;
    AnimalsHomeCtrl mctrl;
    public void InitAwake()
    {
        if (mctrl == null)
        {
            mctrl = SceneMgr.Instance.GetNowScene() as AnimalsHomeCtrl;
        }
        spine = gameObject.GetComponent<SkeletonGraphic>();
        PlayAction("stop");

        maudioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// 播放动画 fly/stop
    /// </summary>
    /// <param name="_name"></param>
    public void PlayAction(string _name)
    {
        spine.AnimationState.SetAnimation(1, _name, true);
    }


    public void MoveToPosWorld(Vector3 _vpos, float _time)
    {
        PlayBirdFlySound();
        PlayAction("fly");
        transform.DOMove(_vpos, _time).OnComplete(()=> 
        {
            StopBirdSound();
            PlayAction("stop");
            transform.SetSiblingIndex(3);
        });
    }
    public void MoveToPosLocal(Vector3 _vpos, float _time)
    {
        PlayBirdFlySound();
        transform.SetSiblingIndex(30);
        PlayAction("fly");
        transform.DOLocalMove(_vpos, _time).OnComplete(() =>
        {
            StopBirdSound();
            PlayAction("stop");
        });
    }

    AudioClip birdfly;
    AudioClip birdjijiji;   
    public void PlayBirdFlySound()
    {      
        if (birdfly == null)
        {
            birdfly = mctrl.MSoundCtrl.GetClip("animalshome_sound", "birdfly");
        }
        maudioSource.clip = birdfly;
        maudioSource.loop = true;
        maudioSource.volume = 0.7f;
        maudioSource.Play();
        PlayBirdJiJiJiSound();
    }

    public void StopBirdSound()
    {
        maudioSource.Stop();
    }

    public void PlayBirdJiJiJiSound()
    {
        if (birdjijiji == null)
        {
            birdjijiji = mctrl.MSoundCtrl.GetClip("animalshome_sound", "birdjijiji");
        }
        mctrl.MSoundCtrl.PlaySortSound(birdjijiji,0.3f);
    }
}
