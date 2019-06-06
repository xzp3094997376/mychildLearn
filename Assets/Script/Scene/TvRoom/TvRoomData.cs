using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TvRoomData
{
    //固定数据
    //类型-类型的种类
    public Dictionary<int, List<int>> mTypeData = new Dictionary<int, List<int>>();
    //关卡-相同的属性：1-数量，2-队形，3-大小，4-颜色，5-种类
    //Dictionary<int, List<int>> mGuankaTypes = new Dictionary<int, List<int>>();




    //变动数据
    public int guanka { get; set; }
    //当前关卡的相同属性有哪些：1-数量，2-队形，3-大小，4-颜色，5-种类(注意：按下按钮时候不能用这个作为唯一标准因为其他随机生成的数据可能会一样)
    public List<int> types = new List<int>();
    //当前关卡的相同属性，各是那种(注意：按下按钮时候不能用这个作为唯一标准因为其他随机生成的数据可能会一样)
    public Dictionary<int, int> typedata = new Dictionary<int, int>();

    public Vector3 angle = Vector3.zero;//当队形属性一样时，角度也一样

    public TvRoomData()
    {
        //1-数量：0-6
        mTypeData.Add(1, new List<int>() { 0 });
        //2-队形：0-圆，1-正方形，2-人，3-三角形，4-一
        mTypeData.Add(2, new List<int>() { 0, 1, 2, 3, 4 });
        //3-大小：0-大，1-小
        mTypeData.Add(3, new List<int>() { 0, 1 });
        //4-颜色：0-熊，1-鸽子，2-羊，3-马，4-豹，5-鸭，6-猪，7-猫头鹰，8-鸡，9-燕子，10-牛，11-老虎，12-鹅，13-兔
        mTypeData.Add(4, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13});
        //5-种类：0-熊，1-鸽子，2-羊，3-马，4-豹，5-鸭，6-猪，7-猫头鹰，8-鸡，9-燕子，10-牛，11-老虎，12-鹅，13-兔
        mTypeData.Add(5, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });

        //for(int i = 1; i <= 5; i++)
        //{
        //    //多少个相同的属性,2, 3, 4
        //    int num = Random.Range(0, 1000) % 3 + 2;
        //    //选择那几个相同属性 
        //    List<int> types = Common.GetMutexValue(2, 5, num - 1);
        //    types.Insert(0, 1);
        //    mGuankaTypes.Add(i, types);
        //}

        typedata.Add(1, 0);
        typedata.Add(2, 0);
        typedata.Add(3, 0);
        typedata.Add(4, 0);
        typedata.Add(5, 0);

    }
    public void Set(int _guanka)
    {
        guanka = _guanka;

        int num = Random.Range(0, 1000) % 2 + 1;//多少个相同的属性,2, 3
        types = Common.GetMutexValue(2, 4, num);//选择那几个相同属性 
        types.Insert(0, 1);
        //types = new List<int>() { 1, 2};

        for (int i = 0; i < types.Count; i++)
        {
            int index = Random.Range(0, 1000) % mTypeData[types[i]].Count;
            int value = mTypeData[types[i]][index];
            typedata[types[i]] = value;
            
        }
        typedata[5] = typedata[4];

        switch (typedata[2])
        {
            case 0:
                angle =  new Vector3(0, 0, Random.Range(0, 1000));
                break;
            case 1:
                angle = new Vector3(0, 0, Random.Range(0, 1000));
                break;
            case 2:
                angle = new Vector3(0, 0, (Random.Range(0, 1000) % 2 == 0 ? 0 : 180));
                break;
            case 3:
                angle = new Vector3(0, 0, ((Random.Range(0, 1000) % 8) * 180));
                break;
            case 4:
                angle = new Vector3(0, 0, Random.Range(-35, 35));
                break;
            default:
                angle = Vector3.zero;
                break;
        }



    }
    
    //获取随机变动数据
    public List<int> GetRandomData()
    {
        List<int> result = new List<int>();
        for(int type = 1; type <= 5; type++)
        {
            result.Add(mTypeData[type][Random.Range(0, 1000) % mTypeData[type].Count]);
        }
        return result;
    }
    //获取按原计划设定好的数据
    public List<int> GetSetData()
    {
        List<int> result = new List<int>() { 0, 0, 0, 0, 0};
        for (int i = 1; i <= 5; i++)
        {
            if (types.Contains(i))
            {
                //相同属性
                result[i - 1] = typedata[i];
            }
            else
            {
                //不同属性
                int index = Random.Range(0, 1000) % mTypeData[i].Count;
                int value = mTypeData[i][index];
                result[i - 1] = value;
            }
        }
        result[4] = result[3];//种类和颜色一样
        return result;
    }

}
