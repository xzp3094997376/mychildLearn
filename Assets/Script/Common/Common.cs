using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Common
{
    public static int gHeight = 800;
    public static int gWidth = 0;
    public static void Init()
    {
        gWidth = (int)(gHeight * (Screen.width * 1f / Screen.height));
    }

    /// <summary>
    /// 随机获取值，可以相同
    /// </summary>
    public static List<int> GetValueList(int min, int max, int num)
    {
        List<int> result = new List<int>();
        int len = max - min + 1;
        for (int i = 0; i < num; i++)
        {
            int offset = Random.Range(0, 1000) % len;
            result.Add(min + offset);
        }
        return result;
    }

    /// <summary>
    /// 在最小值和最大值之间随机获取num个互不相同的值
    /// </summary>
    public static List<int> GetMutexValue(int min, int max, int num)
    {
        List<int> result = new List<int>();
        List<int> temp = new List<int>();
        for (int i = min; i <= max; i++) temp.Add(i);

        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, 1000) % temp.Count;
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }
        return result;
    }

    public static List<T> GetMutexValue<T>(List<T> list, int num)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, 1000) % list.Count;
            result.Add(list[index]);
            list.RemoveAt(index);
        }
        return result;
    }

    public static int GetMutexValue(int min, int max)
    {
        int len = max - min + 1;
        int offset = Random.Range(0, 1000) % len;
        return min + offset;
    }

    /// <summary>
    /// 在min与max之间取num个随机数，可以重复
    /// </summary>
    public static List<int> GetRandList(int min, int max, int num)
    {
        List<int> result = new List<int>();
        int len = max - min + 1;
        for (int i = 0; i < num; i++)
        {
            int offset = Random.Range(0, 1000) % len;
            result.Add(min + offset);
        }
        return result;
    }

    /// <summary>
    /// 返回随机值
    /// </summary>
    public static int GetRandValue(int min, int max)
    {
        int len = max - min + 1;
        return Random.Range(0, 10000) % len + min;
    }




    /// <summary>
    /// 返回?x?的数组，每行每列都0-?的id都只出现一次的数组
    /// </summary>
    /// <returns></returns>
    public static int[,] GetArrayMuteId(int array_width, int id_offset = 0, bool debug = false)
    {
        int[,] result = new int[array_width, array_width];

        List<int> temp_histroy = new List<int>();//历史记录
        Dictionary<int, List<int>> temp_ways = new Dictionary<int, List<int>>();//多少种方法
        for(int i = 0; i < array_width; i++)
        {
            for(int j = 0; j < array_width; j++)
            {
                int key = i * array_width + j;
                temp_histroy.Add(key);
                temp_ways.Add(key, new List<int>());
                result[i, j] = -1;
            }
        }

        bool error = true;

        while(error)
        {
            error = false;

            while (true)
            {
                //重新刷新，还没有填空位置，它们各有几种可能可以选择
                for (int i = 0; i < array_width; i++)
                {
                    for (int j = 0; j < array_width; j++)
                    {
                        //复位
                        int key = i * array_width + j;
                        if (!temp_histroy.Contains(key))
                            continue;

                        temp_ways[key].Clear();
                        for (int z = 0; z < array_width; z++)
                            temp_ways[key].Add(z);
                        //剔除
                        for (int z = 0; z < array_width; z++)
                        {
                            if (temp_ways[key].Contains(result[i, z]))
                            {
                                temp_ways[key].Remove(result[i, z]);
                            }
                            if (temp_ways[key].Contains(result[z, j]))
                            {
                                temp_ways[key].Remove(result[z, j]);
                            }
                        }
                    }
                }
                //挑出可能个数最小的随机
                int temp_key = 0;
                for (int i = 0; i < array_width; i++)
                {
                    for (int j = 0; j < array_width; j++)
                    {
                        int key = i * array_width + j;
                        if (!temp_histroy.Contains(key))
                            continue;
                        if (temp_ways.ContainsKey(temp_key))
                        {
                            if (temp_ways[key].Count < temp_ways[temp_key].Count)
                            {
                                temp_key = key;
                            }
                        }
                        else
                        {
                            temp_key = key;
                        }
                    }
                }

                if (temp_ways[temp_key].Count == 0)
                {
                    error = true;
                    temp_histroy.Clear();
                    temp_ways.Clear();
                    for (int i = 0; i < array_width; i++)
                    {
                        for (int j = 0; j < array_width; j++)
                        {
                            int key = i * array_width + j;
                            temp_histroy.Add(key);
                            temp_ways.Add(key, new List<int>());
                            result[i, j] = -1;
                        }
                    }

                    break;
                }

                int x = temp_key / array_width;
                int y = temp_key % array_width;
                result[x, y] = temp_ways[temp_key][Random.Range(0, 1000) % temp_ways[temp_key].Count];
                temp_ways.Remove(temp_key);
                temp_histroy.Remove(temp_key);

                if (temp_histroy.Count == 0)
                {
                    break;
                }
            }

        }

        if(debug)
        {
            string msg = "";
            for (int i = 0; i < array_width; i++)
            {
                for (int j = 0; j < array_width; j++)
                {
                    msg += result[i, j].ToString() + ", ";
                }
                msg += "\n";
            }
            Debug.Log(msg);

        }
        if(0 != id_offset)
        {
            for (int i = 0; i < array_width; i++)
            {
                for (int j = 0; j < array_width; j++)
                {
                    result[i, j] = result[i, j] + id_offset;
                }
            }
        }

        return result;

    }
    

    /*
    /// <summary>
    /// 返回，3x3的数组，每行每列都0，1，2的id都只出现一次的数组
    /// </summary>
    /// <returns></returns>
    public static int[,] Get3x3MuteId()
    {
        int[,] result = new int[3, 3] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };

        //先确定第一行
        List<int> temp = Common.BreakRank<int>(new List<int>() { 0, 1, 2 });
        
        if (0 == Random.Range(0, 1000) % 2)



        return result;
    }
    */

    /// <summary>
    /// 打乱顺序
    /// </summary>
    public static List<T> BreakRank<T>(List<T> list)
    {
        List<T> result = new List<T>();
        while (list.Count > 0)
        {
            int index = Random.Range(0, 1000) % list.Count;
            result.Add(list[index]);
            list.RemoveAt(index);
        }
        return result;
    }

    public static Vector3 getMouseLocalPos(Transform _parent)
    {
        Vector3 pos =  _parent.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos.z = 0;
        return pos;
    }
    public static Vector3 getMouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static RaycastHit[] getMouseRayHits()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(ray, 100);
    }


    //public static Vector3 getMouseLocalPos(Transform _parent, Vector3 world_pos)
    //{
    //    Vector3 pos = _parent.worldToLocalMatrix.MultiplyPoint(Camera.main(Input.mousePosition));
    //    pos.z = 0;
    //    return pos;
    //}

    #region 排序
    public static List<Vector3> PosSortByWidth(float width, int num, float pos_y)
    {
        List<Vector3> result = new List<Vector3>();
        float dis = width * 1f / num;
        float x = (num - 1) * dis * -0.5f;
        for (int i = 0; i < num; i++)
        {
            result.Add(new Vector3(x + i * dis, pos_y, 0));
        }
        return result;
    }
    public static List<Vector3> PosSortByHeight(float height, int num, float pos_x)
    {
        List<Vector3> result = new List<Vector3>();
        float dis = height * 1f / (num + 1);
        float y = (num - 1) * dis * 0.5f;
        for (int i = 0; i < num; i++)
        {
            result.Add(new Vector3(pos_x, y - i * dis, 0));
        }
        return result;
    }

    #endregion
    


    /// <summary>
    /// 顺序整数范围内随机取得不相同的数
    /// </summary>
    /// <param name="_form">起点</param>
    /// <param name="_to">终点</param>
    /// <param name="_count">个数</param>
    /// <param name="_unGet">剔除数</param>
    /// <returns></returns>
    public static List<int> GetIDList(int _form, int _to, int _count, int _unGet)
    {
        List<int> resurtList = new List<int>();

        List<int> baselList = new List<int>();
        for (int i = _form; i <= _to; i++)
        {
            if (_unGet != i)
                baselList.Add(i);
        }

        for (int i = 0; i < _count; i++)
        {
            int getID = baselList[Random.Range(0, baselList.Count)];
            resurtList.Add(getID);
            baselList.Remove(getID);
        }
        return resurtList;
    }

    /// <summary>
    /// 分段
    /// </summary>
    /// <param name="count">个数</param>
    /// <param name="width">间距</param>
    /// <returns></returns>
    public static List<float> GetOrderList(float count, float width)
    {
        List<float> list = new List<float>();
        if (count <= 1f)
        {
            list.Add(0);
            return list;
        }
        float start = width * ((count + 1) / 2f);
        for (float i = 1; i < count + 1; i++)
        {
            float x = width * i - start;
            list.Add(x);
        }
        return list;
    }

    /// <summary>
    /// 在圆上分段
    /// </summary>
    /// <param name="_count">段数</param>
    /// <param name="_radius">半径</param>
    /// <returns></returns>
    public static List<Vector3> GetCircleOrderPosList(int _count, float _radius)
    {
        List<Vector3> vpoaList = new List<Vector3>();
        float fbaseAngle = 360f / _count;
        for (int i = 0; i < 10; i++)
        {
            float fX = _radius * Mathf.Cos((fbaseAngle + i * fbaseAngle) * Mathf.Deg2Rad);
            float fY = _radius * Mathf.Sin((fbaseAngle + i * fbaseAngle) * Mathf.Deg2Rad);
            Vector3 getPos = new Vector3(fX, fY, 0f);
            vpoaList.Add(getPos);
        }
        return vpoaList;
    }

    #region 角度
    public static Vector3 Parse180(Vector3 angle)
    {
        while(true)
        {
            if (angle.z < -180)
            {
                angle.z += 360;
            }
            else if (angle.z > 180)
            {
                angle.z -= 360;
            }
            else
            {
                return angle;
            }
        }
    }
    public static Vector3 Parse360(Vector3 angle)
    {
        while (true)
        {
            if (angle.z < 0)
            {
                angle.z += 360;
            }
            else if (angle.z > 360)
            {
                angle.z -= 360;
            }
            else
            {
                return angle;
            }
        }
    }
    public static void SetChildEulerAngles(Transform root_tran, Vector3 angle)
    {
        foreach(Transform t in root_tran)
        {
            t.localEulerAngles = angle;
        }
    }

    #endregion


    //List复制一个新的List
    public static List<T> CopyList<T>( List<T> list)
    {
        List<T> result = new List<T>();
        for(int i = 0; i < list.Count; i++)
        {
            result.Add(list[i]);
        }
        return result;
    }


    public static void SortDepth(RectTransform root)
    {
        if (root.childCount <= 0)
            return;

        //RectTransform[] child = root.GetComponentsInChildren<RectTransform>();
        //List<RectTransform> temp = new List<RectTransform>();
        //for (int i = 0; i < child.Length; i++)
        //{
        //    if(root != child[i])
        //        temp.Add(child[i]);
        //}
        List<RectTransform> temp = new List<RectTransform>();
        for(int i = 0; i < root.childCount; i++)
        {
            temp.Add(root.GetChild(i).GetComponent<RectTransform>());
        }

        temp.Sort(
            delegate (RectTransform x, RectTransform y)
            {
                if (x.anchoredPosition.y > y.anchoredPosition.y)
                    return -1;
                else
                    return 1;
            }
            );


        for (int i = 0; i < temp.Count; i++)
        {
            temp[i].transform.SetSiblingIndex(i);
        }

    }

    /// <summary>
    /// 删除子物体
    /// </summary>
    /// <param name="_trans"></param>
    public static void DestroyChilds(Transform _trans)
    {
        Transform[] transs = _trans.GetComponentsInChildren<Transform>();
        for (int i =0;i<transs.Length;i++)
        {
            if (transs[i] != _trans)
            {
                GameObject go = transs[i].gameObject;
                if (go != null)
                    GameObject.Destroy(go);
            }
        }
    }

}
