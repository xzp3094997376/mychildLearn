using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class WFF_SceneObjMove : MonoBehaviour
{

    public int nPanelID = 0;

    private GameObject part0;
    private GameObject part1;
    private List<Image> imgPart0List = new List<Image>();
    private List<Image> imgPart1List = new List<Image>();
    private RawImage bg0;
    private RawImage bg1;

    public float speed = 10f;
    private float baseSpeed = 10f;

    private float minX = 0f;
    private float maxX = 1280f;

    bool bInit = false;

    public bool bMove = false;


	public void InitAwake(int _panelID)
    {
        nPanelID = _panelID;
        part0 = transform.Find("part0").gameObject;
        part1 = transform.Find("part1").gameObject;
        if (nPanelID == 0)
        {
            bg0 = part0.GetComponent<RawImage>();
            bg1 = part1.GetComponent<RawImage>();
            bg0.texture = ResManager.GetTexture("whoflyfast_texture", "beijing");
            bg1.texture = ResManager.GetTexture("whoflyfast_texture", "beijing");
        }
        for (int i = 0; i < 5; i++)
        {
            Transform tran0 = part0.transform.Find("yun" + i);
            if (tran0 != null)
            {
                Image img0 = tran0.GetComponent<Image>();
                if (img0 != null)
                {
                    img0.sprite = ResManager.GetSprite("whoflyfast_sprite", "yuntype" + nPanelID + "_" + i);
                    imgPart0List.Add(img0);
                }
            }
            Transform tran1 = part1.transform.Find("yun" + i);
            if (tran1 != null)
            {
                Image img1 = tran1.GetComponent<Image>();
                if (img1 != null)
                {
                    img1.sprite = ResManager.GetSprite("whoflyfast_sprite", "yuntype" + nPanelID + "_" + i);
                    imgPart1List.Add(img1);
                }
            }
        }

        bInit = true;	
	}
	
	void LateUpdate ()
    {
        if (bInit)
        {
            if (bMove)
            {
                transform.localPosition += new Vector3(Time.deltaTime * speed, 0f, 0f);
                if (transform.localPosition.x >= maxX)
                {
                    transform.localPosition = new Vector3(minX, transform.localPosition.y, 0f);
                }
            }
        }
	}

    public void SetSpeed(float _speed)
    {
        speed = _speed;
        baseSpeed = _speed;
        speed = 0;
        DOTween.To(() => speed, x => speed = x, _speed, 1f);
    }

    public void SetState(bool _fast)
    {
        //if (_fast)
        //{
        //    speed = baseSpeed * 2f;
        //}
        //else
        //{
        //    speed = baseSpeed;
        //}
    }

}
