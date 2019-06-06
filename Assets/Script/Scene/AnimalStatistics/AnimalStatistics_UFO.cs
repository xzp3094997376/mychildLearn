using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class AnimalStatistics_UFO : MonoBehaviour
{

    private Image ufo;
    private Image block;

    private float fHeight = 0f;

    private Vector3 vStart;

    private AnimalStatisticsCtrl mctrl;
    private bool bInit = false;

    private AudioSource mAudioSource;

    public void InitAwake()
    {
        gameObject.SetActive(true);
        mctrl = SceneMgr.Instance.GetNowScene() as AnimalStatisticsCtrl;

        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.playOnAwake = false;
        mAudioSource.loop = false;
        mAudioSource.Stop();

        vStart = transform.localPosition;
        transform.localPosition = vStart + new Vector3(-1000f, 0f, 0f);

        ufo = transform.Find("ufo").GetComponent<Image>();
        ufo.sprite = ResManager.GetSprite("animalstatistics_sprite", "ufo");

        block = transform.Find("block").GetComponent<Image>();
        block.sprite = ResManager.GetSprite("animalstatistics_sprite", "block_yellow");
        fHeight = transform.localPosition.y;

        SetBlockState(false);

        mClickBallTip = GuideHandCtl.Create(transform);
        bInit = true;
    }

    public Vector3 GetCreatePos()
    {
        return block.transform.position;
    }

    public void SetBlockState(bool _active)
    {
        if (block != null)
            block.enabled = _active;
    }

    public void BlockShowOutEffect()
    {
        mctrl.PlaySortSound("sound_ufoblockshowout");
        block.transform.localScale = Vector3.one * 0.001f;
        block.transform.DOScale(Vector3.one, 0.2f);
    }

    public void PlayUFOMoveSound(bool _play)
    {
        if (_play)
        {
            if (mAudioSource.clip == null)
            {
                AudioClip cp = mctrl.GetClip("sound_ufomove");
                mAudioSource.clip = cp;
            }
            mAudioSource.Play();
        }
        else
        {
            mAudioSource.Stop();
        }
    }

    public void UFOMoveTo(bool _local, Vector3 _worldPos, float _time, System.Action _action = null)
    {
        PlayUFOMoveSound(true);
        if (_local)
        {
            transform.DOLocalMove(_worldPos, _time).SetEase(Ease.Linear).OnComplete(() =>
            {
                PlayUFOMoveSound(false);
                if (_action != null)
                    _action();
            });
        }
        else
        {
            transform.DOMove(_worldPos, _time).SetEase(Ease.Linear).OnComplete(() =>
            {
                PlayUFOMoveSound(false);
                if (_action != null)
                    _action();
            });
        }
    }


    public void MoveToStartHeight(System.Action _action = null)
    {
        PlayUFOMoveSound(true);
        transform.DOLocalMoveY(fHeight + 300f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetBlockState(false);
            PlayUFOMoveSound(true);
            transform.DOLocalMoveY(fHeight, 0.5f).OnComplete(() =>
            {
                PlayUFOMoveSound(false);
                if (_action != null)
                    _action();
            });
        });
    }

    public void SceneMove(bool _in)
    {
        if (_in)
        {
            transform.localPosition = vStart + new Vector3(-1000f, 0f, 0f);
            transform.DOLocalMove(vStart, 1f).OnComplete(()=> 
            {
                BlockShowOutEffect();
                SetBlockSprite("block_yellow");
                SetBlockState(true);
            });
        }
        else
        {
            transform.DOLocalMove(vStart + new Vector3(-1000f, 0f, 0f), 1f);
        }
    }


    public void SetBlockSprite(string _sp)
    {
        block.sprite = ResManager.GetSprite("animalstatistics_sprite", _sp);
    }

    public void ShowFlower(bool _active)
    {
    }

    /// <summary>
    /// click 指引
    /// </summary>
    private GuideHandCtl mClickBallTip;
    public void ShowClickTip(Vector3 _worldPos)
    {
        mClickBallTip.GuideTipClick(1.3f, 0.7f, true, true, "hand1");
        mClickBallTip.SetClickTipPos(_worldPos);
        mClickBallTip.SetClickTipOffsetPos(new Vector3(5f, -30f, 0f), 1f);
    }
    public void StopClickTip()
    {
        mClickBallTip.StopClick();
    }

}
