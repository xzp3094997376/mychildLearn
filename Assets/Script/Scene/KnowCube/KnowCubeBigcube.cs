using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class KnowCubeBigcube : MonoBehaviour
{
    [HideInInspector]
    public Vector3 vstart;

    private List<Image> cubeList = new List<Image>();

    public int nType = 0;
    public int nIndex = 0;

    public RectTransform rectTransform;

    private BoxCollider2D box2d;

    public void InitAwake(int _ntype,int _index)
    {
        nType = _ntype;
        nIndex = _index;

        for (int i = 0; i < 27; i++)
        {
            Image img0 = transform.Find("Image" + i).GetComponent<Image>();
            cubeList.Add(img0);
            img0.sprite = ResManager.GetSprite("knowcube_sprite", "spobj1_4");
            img0.raycastTarget = false;
        }

        vstart = transform.localPosition;
        rectTransform = transform as RectTransform;

        box2d = transform.GetComponent<BoxCollider2D>();
        BoxActive(true);
    }

    public void BoxActive(bool _active)
    {
        box2d.enabled = _active;
    }

    public void MoveToStart()
    {
        transform.DOLocalMove(vstart, 0.2f);
    }


    /// <summary>
    /// 匹配错误弹回
    /// </summary>
    public void FaileMoveBack()
    {
        BoxActive(false);
        transform.DOScale(Vector3.one, 0.6f);
        StartCoroutine(IEToyMoveBack());
    }
    IEnumerator IEToyMoveBack()
    {
        Vector3 vfrom = transform.localPosition;
        //vRemaidLocalPos = vPath[2];
        for (float i = 0; i < 1f; i += 0.04f)
        {
            transform.localPosition = Vector3.Lerp(vfrom, vstart, i) + new Vector3(0f, 200f, 0f) * Mathf.Sin(Mathf.PI * i);
            yield return new WaitForSeconds(0.02f * Mathf.Sin(Mathf.PI * i));
        }
        BoxActive(true);
    }

    private Transform tCenter;
    public Transform GetCenter()
    {
        if (tCenter == null)
        {
            GameObject mgo = UguiMaker.newGameObject("centerpos", transform);
            Vector2 v2 = box2d.offset;
            mgo.transform.localPosition = Vector3.zero + new Vector3(v2.x, v2.y, 0f);
            tCenter = mgo.transform;
        }
        return tCenter;
    }
}

public class KnowCubeBottle : MonoBehaviour
{

    private Image img;
    private Vector3 vStart;

    private Transform uppos;
    public List<KnowCubeBigcube> mdropInList = new List<KnowCubeBigcube>();

    private KnowCubeLv3 ctrllv3;

    public void InitAwake(KnowCubeLv3 _mctrl)
    {
        ctrllv3 = _mctrl;
        img = transform.Find("Image").GetComponent<Image>();
        img.sprite = ResManager.GetSprite("knowcube_sprite", "bottle");
        vStart = transform.localPosition;
        uppos = transform.Find("uppos");
    }

    public void ResetINfos()
    {
        mdropInList.Clear();
        transform.localPosition = vStart;
    }


    public void DropIn(KnowCubeBigcube _cube)
    {
        _cube.transform.SetParent(transform);
        _cube.transform.localPosition = new Vector3(0f, 100f, 0f);
        _cube.transform.DOScale(Vector3.one * 0.2f, 0.3f);

        mdropInList.Add(_cube);
        if (mdropInList.Count >= 2)
        {
            ctrllv3.bCanDrop = false;
            ShakeBottle(1.6f, ShakeFinishCheck);
        }
    }

    private bool CheckIsOK()
    {
        if (mdropInList[0].nIndex + mdropInList[1].nIndex == 3)
        { return true; }
        return false;
    }


    private void ShakeFinishCheck()
    {
        ctrllv3.StartCoroutine(IEPlayEffect());
    }
    IEnumerator IEPlayEffect()
    {
        yield return new WaitForSeconds(1f);
        if (CheckIsOK())
        {
            ctrllv3.CombineOK();
        }
        else
        {
            AudioClip cp0 = ctrllv3.mCtrl.GetClip("zadditional-8");
            ctrllv3.mCtrl.PlaySound(ctrllv3.mCtrl.mKimiAudioSource, cp0, 1f);

            for (int i = 0; i < mdropInList.Count; i++)
            {
                transform.DOScaleY(0.6f, 0.3f);
                transform.DOScaleX(1.2f, 0.3f);
                yield return new WaitForSeconds(0.2f);
                transform.DOScaleY(1f, 0.2f).SetEase(Ease.OutBack);
                transform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(0.2f);
                mdropInList[i].transform.SetParent(transform.parent);
                mdropInList[i].transform.position = uppos.position;
                mdropInList[i].FaileMoveBack();
                ctrllv3.mCtrl.PlayTheSortSound("cubeshowout");
            }
            yield return new WaitForSeconds(1.5f);
            mdropInList.Clear();
            ctrllv3.bCanDrop = true;
        }
    }



    public void PlayBottleAni()
    {
        transform.DOScaleY(0.6f, 0.3f);
        transform.DOScaleX(1.2f, 0.3f);
    }

    public void ShakeBottle(float _time = 1f,System.Action _action = null)
    {
        ctrllv3.mCtrl.PlayTheSortSound("blockhecheng");
        transform.DOShakeScale(_time, 0.25f,10,20f).OnComplete(()=> 
        {
            if (_action != null)
                _action();
        });
    }

    public void BottleHide()
    {
        transform.localScale = Vector3.one * 0.001f;
    }

    private Transform tCenter;
    public Transform GetCenter()
    {
        if (tCenter == null)
        {
            GameObject mgo0 = UguiMaker.newGameObject("centerpos", transform);
            mgo0.transform.localPosition = new Vector3(0f, 100f, 0f);
            tCenter = mgo0.transform;
        }
        Debug.Log(tCenter.transform.localPosition);
        return tCenter;
    }
}


