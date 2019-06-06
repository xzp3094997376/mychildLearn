using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScrollRectUpdate : MonoBehaviour
{
    public int m_item_height = 0;//高度
    public int m_item_space = 0;//间距
    public int m_clone_prefab_num = 4;//一页最多展示的个数
    public GameObject m_prefab;

    ScrollRect m_scroll;
    List<RectTransform> mItems = new List<RectTransform>();
    Dictionary<RectTransform, IScrollRectFlush> mFlushs = new Dictionary<RectTransform, IScrollRectFlush>();

    float temp_y_max = 0;
    float temp_y_min = 0;
    void Awake()
    {
        temp_y_max = m_item_height + m_item_space;
        temp_y_min = (m_clone_prefab_num) * -temp_y_max;

        m_prefab.gameObject.SetActive(false);
        m_scroll = gameObject.GetComponent<ScrollRect>();
        for (int i = 0; i < m_clone_prefab_num + 1; i++)
        {
            GameObject g = GameObject.Instantiate(m_prefab) as GameObject;
            g.name = (i).ToString();
            g.SetActive(true);
            UguiMaker.InitGameObj(g, m_scroll.content, (i).ToString(), Vector3.zero, Vector3.one);
            mItems.Add(g.GetComponent<RectTransform>());
            
        }
        foreach(RectTransform r in mItems)
        {
            mFlushs.Add(r, r.GetComponent<IScrollRectFlush>());
        }
        

    }
    
	void Start ()
    {
        Vector2 pos = Vector2.zero;
        for (int i = 0; i < mItems.Count; i++)
        {
            mItems[i].anchoredPosition = pos;
            mFlushs[mItems[i]].Flush(i);
            pos = pos + new Vector2(0, -m_item_height - m_item_space);
        }

    }

    public void RectUpdate(int row_number)
    {
        
        m_scroll.content.sizeDelta = new Vector2(m_scroll.content.sizeDelta.x, row_number * (m_item_height + m_item_space));


    }

	public void ItemUpdateY ()
    {
        //return;
        for(int i = 0; i < mItems.Count; i++)
        {
            Vector2 pos = mItems[i].anchoredPosition + m_scroll.content.anchoredPosition;
            if(pos.y > temp_y_max + 10)
            {
                mItems[i].anchoredPosition -= new Vector2(0, (1 + m_clone_prefab_num) * (m_item_height + m_item_space));
                if(mItems[i].anchoredPosition.y < -m_scroll.content.sizeDelta.y +1)
                {
                    mItems[i].gameObject.SetActive(false);
                }
                else if(!mItems[i].gameObject.activeSelf)
                {
                    mItems[i].gameObject.SetActive(true);
                }
                mFlushs[mItems[i]].Flush(Mathf.RoundToInt(Mathf.Abs(mItems[i].anchoredPosition.y) / (m_item_height + m_item_space)));

                break;
            }
            else if(pos.y < temp_y_min - 10)
            {
                mItems[i].anchoredPosition += new Vector2(0, (1 + m_clone_prefab_num) * (m_item_height + m_item_space));
                if (mItems[i].anchoredPosition.y > 0)
                {
                    mItems[i].gameObject.SetActive(false);
                }
                else if (!mItems[i].gameObject.activeSelf)
                {
                    mItems[i].gameObject.SetActive(true);
                }
                mFlushs[mItems[i]].Flush(Mathf.RoundToInt(Mathf.Abs(mItems[i].anchoredPosition.y) / (m_item_height + m_item_space)));
                break;
            }

        }
	}

}
