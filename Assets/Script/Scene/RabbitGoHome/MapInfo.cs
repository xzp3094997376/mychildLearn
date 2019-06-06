using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapInfo  {

    public  MapInfo()
    {

    }
    //获取十字路坐标字典
    public Dictionary<string, Vector3> GetShiZiLuPosDic(int map)
    {
        Dictionary<string, Vector3> shiziluPosList = new Dictionary<string, Vector3>();
        switch (map)
        {
            case 1:
                shiziluPosList.Add("1_2_3_4_5", new Vector3(-558f, 160f, 0f));
                shiziluPosList.Add("1_2_4", new Vector3(-316f, 161f, 0f));
                shiziluPosList.Add("2_4_5", new Vector3(-31f, 50f, 0f));
                shiziluPosList.Add("3_4_5", new Vector3(-167f, -232f, 0f));
                shiziluPosList.Add("1_0", new Vector3(174f, 255f, 0f));
                shiziluPosList.Add("1_0_0", new Vector3(525f, 180f, 0f));
                shiziluPosList.Add("1_2_3_4_5_end", new Vector3(225f, -212f, 0f));
                shiziluPosList.Add("3_5", new Vector3(-480f, -21f, 0f));
                break;
            case 2:
                shiziluPosList.Add("1_2_3", new Vector3(-558f, 160f, 0f));
                shiziluPosList.Add("1_0", new Vector3(128f, 223f, 0f));
                //shiziluPosList.Add("1_0_0", new Vector3(454f, -52f, 0f));
                shiziluPosList.Add("2_0", new Vector3(-46f, -5f, 0f));
                //shiziluPosList.Add("2_3", new Vector3(150f, -128f, 0f));
                shiziluPosList.Add("3_0", new Vector3(-472f, -247f, 0f));
                shiziluPosList.Add("1_2_3_end", new Vector3(292f, -264f, 0f));
                break;
            case 3:
                shiziluPosList.Add("1_2_3", new Vector3(-558f, 160f, 0f));
                shiziluPosList.Add("1_0", new Vector3(343f, 237f, 0f));
                shiziluPosList.Add("2_3", new Vector3(-430f, -40f, 0f));
                shiziluPosList.Add("1_2", new Vector3(-118f, -67f, 0f));
                shiziluPosList.Add("1_2_3_end", new Vector3(-150f, -269f, 0f));
                break;
        }

        return shiziluPosList;
    }
    //获取十字路缩放比例字典
    public Dictionary<string, float> GetShiZiLuScaleDic(int map)
    {
        Dictionary<string, float> shiziluScaleList = new Dictionary<string, float>();
        switch (map)
        {
            case 1:
                shiziluScaleList.Add("1_2_3_4_5", 0.8f);
                shiziluScaleList.Add("1_2_4", 0.8f);
                shiziluScaleList.Add("2_4_5", 0.8f);
                shiziluScaleList.Add("3_4_5", 0.8f);
                shiziluScaleList.Add("1_0", 0.8f);
                shiziluScaleList.Add("1_0_0", 0.6f);
                shiziluScaleList.Add("1_2_3_4_5_end", 0.8f);
                shiziluScaleList.Add("3_5", 0.6f);
                break;
            case 2:
                shiziluScaleList.Add("1_2_3", 0.8f);
                shiziluScaleList.Add("1_0", 0.8f);
                shiziluScaleList.Add("1_0_0", 0.8f);
                shiziluScaleList.Add("2_0", 0.8f);
                shiziluScaleList.Add("2_3", 0.8f);
                shiziluScaleList.Add("3_0", 0.8f);
                shiziluScaleList.Add("1_2_3_end", 0.8f);
                break;
            case 3:
                shiziluScaleList.Add("1_2_3", 0.8f);
                shiziluScaleList.Add("1_0", 0.8f);
                shiziluScaleList.Add("2_3", 0.8f);
                shiziluScaleList.Add("1_2", 0.8f);
                shiziluScaleList.Add("1_2_3_end", 0.8f);
                break;
        }

        return shiziluScaleList;
    }
    //获取路劲轨迹坐标
    public List<Vector3> GetRoadPosList(int map,int road)
    {
        List<Vector3> roadPosList = null ;
        switch (map)
        {
            case 1:
                switch (road)
                {
                    case 1:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -463f, 171f, 0f),
                    new Vector3( -391f, 166f, 0f),
                    new Vector3( -316f, 161f, 0f),
                    new Vector3( -265f, 217f, 0f),
                    new Vector3( -187f, 247f, 0f),
                    new Vector3( -112f, 259f, 0f),
                    new Vector3( -43f, 268f, 0f),
                    new Vector3( 27f, 268f, 0f),
                    new Vector3( 99f, 268f, 0f),
                    new Vector3( 174f, 255f, 0f),
                    new Vector3( 252f, 260f, 0f),
                    new Vector3( 335f, 276f, 0f),
                    new Vector3( 418f, 284f, 0f),
                    new Vector3( 499f, 260f, 0f),
                    new Vector3( 525f, 180f, 0f),
                    new Vector3( 504f, 91f, 0f),
                    new Vector3( 426f, 24f, 0f),
                    new Vector3( 348f, -19f, 0f),
                    new Vector3( 276f, -70f, 0f),
                    new Vector3( 223f, -144f, 0f),
                    new Vector3( 225f, -212f, 0f)
                };
                        break;
                    case 2:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -463f, 171f, 0f),
                    new Vector3( -391f, 166f, 0f),
                    new Vector3( -316f, 161f, 0f),
                    new Vector3( -225f, 150f, 0f),
                    new Vector3( -161f, 118f, 0f),
                    new Vector3( -97f, 91f, 0f),
                    new Vector3( -31f, 50f, 0f),
                    new Vector3( 67f, 40f, 0f),
                    new Vector3( 137f, 13f, 0f),
                    new Vector3( 193f, -48f, 0f),
                    new Vector3( 223f, -144f, 0f),
                    new Vector3( 225f, -212f, 0f)
                };
                        break;
                    case 3:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -493f, 64f, 0f),
                    new Vector3( -480f, -21f, 0f),
                    new Vector3( -472f, -104f, 0f),
                    new Vector3( -458f, -182f, 0f),
                    new Vector3( -391f, -209f, 0f),
                    new Vector3( -311f, -220f, 0f),
                    new Vector3( -241f, -230f, 0f),
                    new Vector3( -167f, -232f, 0f),
                    new Vector3( -99f, -292f, 0f),
                    new Vector3( -32f, -330f, 0f),
                    new Vector3( 43f, -335f, 0f),
                    new Vector3( 110f, -303f, 0f),
                    new Vector3( 174f, -255f, 0f),
                    new Vector3( 225f, -212f, 0f)
                };
                        break;
                    case 4:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -463f, 171f, 0f),
                    new Vector3( -391f, 166f, 0f),
                    new Vector3( -316f, 161f, 0f),
                    new Vector3( -225f, 150f, 0f),
                    new Vector3( -161f, 118f, 0f),
                    new Vector3( -97f, 91f, 0f),
                    new Vector3( -31f, 50f, 0f),
                    new Vector3( -54f, -99f, 0f),
                    new Vector3( -115f, -161f, 0f),
                    new Vector3( -167f, -232f, 0f),
                    new Vector3( -99f, -292f, 0f),
                    new Vector3( -32f, -330f, 0f),
                    new Vector3( 43f, -335f, 0f),
                    new Vector3( 110f, -303f, 0f),
                    new Vector3( 174f, -255f, 0f),
                    new Vector3( 225f, -212f, 0f)
                };
                        break;
                    case 5:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -493f, 64f, 0f),
                    new Vector3( -480f, -21f, 0f),
                    new Vector3( -472f, -104f, 0f),
                    new Vector3( -458f, -182f, 0f),
                    new Vector3( -391f, -209f, 0f),
                    new Vector3( -311f, -220f, 0f),
                    new Vector3( -241f, -230f, 0f),
                    new Vector3( -167f, -232f, 0f),
                    new Vector3( -115f, -161f, 0f),
                    new Vector3( -54f, -99f, 0f),
                    new Vector3( -48f, -16f, 0f),
                    new Vector3( -31f, 50f, 0f),
                    new Vector3( 67f, 40f, 0f),
                    new Vector3( 137f, 13f, 0f),
                    new Vector3( 193f, -48f, 0f),
                    new Vector3( 223f, -144f, 0f),
                    new Vector3( 225f, -212f, 0f)
                };
                        break;
                }
                break;

            case 2:
                switch (road)
                {
                    case 1:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3(-558f, 160f, 0f),
                    new Vector3( -502f, 223f, 0f),
                    new Vector3( -437f, 259f, 0f),
                    new Vector3( -360f, 253f, 0f),
                    new Vector3( -283f, 244f, 0f),
                    new Vector3( -202f, 237f, 0f),
                    new Vector3( -121f, 237f, 0f),
                    new Vector3( -39f, 234f, 0f),
                    new Vector3( 45f, 234f, 0f),
                    new Vector3( 128f, 223f, 0f),
                    new Vector3( 205f, 218f, 0f),
                    new Vector3( 288f, 200f, 0f),
                    new Vector3( 366f, 177f, 0f),
                    new Vector3( 438f, 140f, 0f),
                    new Vector3( 494f, 90f, 0f),
                    new Vector3( 478f, 13f, 0f),
                    new Vector3( 454f, -52f, 0f),
                    new Vector3( 395f, -106f, 0f),
                    new Vector3( 340f, -165f, 0f),
                    new Vector3( 292f, -264f, 0f)
                };
                        break;
                    case 2:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3(-558f, 160f, 0f),
                    new Vector3( -462f, 155f, 0f),
                    new Vector3( -404f, 115f, 0f),
                    new Vector3( -337f, 73f, 0f),
                    new Vector3( -265f, 49f, 0f),
                    new Vector3( -192f, 31f, 0f),
                    new Vector3( -116f, 14f, 0f),
                    new Vector3( -46f, -5f, 0f),
                    new Vector3( 30f, -26f, 0f),
                    new Vector3( 92f, -71f, 0f),
                    new Vector3( 150f, -128f, 0f),
                    new Vector3( 211f, -187f, 0f),
                    new Vector3( 292f, -264f, 0f),
                };
                        break;
                    case 3:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3(-558f, 160f, 0f),
                    new Vector3( -587f, 92f, 0f),
                    new Vector3( -586f, 11f, 0f),
                    new Vector3( -582f, -72f, 0f),
                    new Vector3( -579f, -157f, 0f),
                    new Vector3( -544f, -210f, 0f),
                    new Vector3( -472f, -247f, 0f),
                    new Vector3( -392f, -253f, 0f),
                    new Vector3( -312f, -258f, 0f),
                    new Vector3( -230f, -245f, 0f),
                    new Vector3( -152f, -219f, 0f),
                    new Vector3( -83f, -178f, 0f),
                    new Vector3( -5f, -143f, 0f),
                    new Vector3( 67f, -134f, 0f),
                    new Vector3( 150f, -128f, 0f),
                    new Vector3( 211f, -187f, 0f),
                    new Vector3( 292f, -264f, 0f),
                };
                        break;
                    case 4:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3(-558f, 160f, 0f),
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -457f, 112f, 0f),
                    new Vector3( -380f, 96f, 0f),
                    new Vector3( -318f, 150f, 0f),
                    new Vector3( -225f, 150f, 0f),
                    new Vector3( -161f, 118f, 0f),
                    new Vector3( -97f, 91f, 0f),
                    new Vector3( -31f, 50f, 0f),
                    new Vector3( -54f, -99f, 0f),
                    new Vector3( -115f, -161f, 0f),
                    new Vector3( -167f, -232f, 0f),
                    new Vector3( -99f, -292f, 0f),
                    new Vector3( -32f, -330f, 0f),
                    new Vector3( 43f, -335f, 0f),
                    new Vector3( 110f, -303f, 0f),
                    new Vector3( 174f, -255f, 0f),
                    new Vector3( 225f, -212f, 0f),
                    new Vector3( 292f, -284f, 0f),
                    new Vector3( 367f, -335f, 0f)
                };
                        break;
                    case 5:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3(-558f, 160f, 0f),
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -493f, 64f, 0f),
                    new Vector3( -480f, -21f, 0f),
                    new Vector3( -472f, -104f, 0f),
                    new Vector3( -458f, -182f, 0f),
                    new Vector3( -391f, -209f, 0f),
                    new Vector3( -311f, -220f, 0f),
                    new Vector3( -241f, -230f, 0f),
                    new Vector3( -167f, -232f, 0f),
                    new Vector3( -115f, -161f, 0f),
                    new Vector3( -54f, -99f, 0f),
                    new Vector3( -48f, -16f, 0f),
                    new Vector3( -31f, 50f, 0f),
                    new Vector3( 67f, 40f, 0f),
                    new Vector3( 137f, 13f, 0f),
                    new Vector3( 193f, -48f, 0f),
                    new Vector3( 223f, -144f, 0f),
                    new Vector3( 225f, -212f, 0f),
                    new Vector3( 292f, -284f, 0f),
                    new Vector3( 367f, -335f, 0f)
                };
                        break;
                }
                break;
            case 3:
                switch (road)
                {
                    case 1:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -470f, 218f, 0f),
                    new Vector3( -390f, 243f, 0f),
                    new Vector3( -304f, 248f, 0f),
                    new Vector3( -220f, 235f, 0f),
                    new Vector3( -135f, 234f, 0f),
                    new Vector3( -59f, 230f, 0f),
                    new Vector3( 24f, 230f, 0f),
                    new Vector3( 104f, 224f, 0f),
                    new Vector3( 185f, 216f, 0f),
                    new Vector3( 265f, 220f, 0f),
                    new Vector3( 343f, 237f, 0f),
                    new Vector3( 421f, 256f, 0f),
                    new Vector3( 509f, 236f, 0f),
                    new Vector3( 562f, 172f, 0f),
                    new Vector3( 573f, 104f, 0f),
                    new Vector3( 519f, 40f, 0f),
                    new Vector3( 436f, 24f, 0f),
                    new Vector3( 361f, 37f, 0f),
                    new Vector3( 279f, 56f, 0f),
                    new Vector3( 185f, 33f, 0f),
                    new Vector3( 99f, 0f, 0f),
                    new Vector3( -34f, -26f, 0f),
                    new Vector3( -118f, -67f, 0f),
                    new Vector3( -166f, -128f, 0f),
                    new Vector3( -172f, -199f, 0f),
                    new Vector3( -150f, -269f, 0f),
                };
                        break;
                    case 2:
                        roadPosList = new List<Vector3>()
                {
                     new Vector3( -558f, 160f, 0f),
                    new Vector3( -576f, 72f, 0f),
                    new Vector3( -518f, 5f, 0f),
                    new Vector3( -430f, -40f, 0f),
                    new Vector3( -377f, 9f, 0f),
                    new Vector3( -320f, 42f, 0f),
                    new Vector3( -246f, 83f, 0f),
                    new Vector3( -157f, 93f, 0f),
                    new Vector3( -102f, 34f, 0f),
                    new Vector3( -118f, -67f, 0f),
                    new Vector3( -166f, -128f, 0f),
                    new Vector3( -172f, -199f, 0f),
                    new Vector3( -150f, -269f, 0f),
                };
                        break;
                    case 3:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -544f, 72f, 0f),
                    new Vector3( -510f, 5f, 0f),
                    new Vector3( -430f, -40f, 0f),
                    new Vector3( -426f, -123f, 0f),
                    new Vector3( -423f, -205f, 0f),
                    new Vector3( -399f, -275f, 0f),
                    new Vector3( -331f, -287f, 0f),
                    new Vector3( -247f, -282f, 0f),
                    new Vector3( -150f, -269f, 0f),
                };
                        break;
                    case 4:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -470f, 218f, 0f),
                    new Vector3( -390f, 243f, 0f),
                    new Vector3( -300f, 231f, 0f),
                    new Vector3( -220f, 260f, 0f),
                    new Vector3( -129f, 295f, 0f),
                    new Vector3( -51f, 304f, 0f),
                    new Vector3( 35f, 308f, 0f),
                    new Vector3( 108f, 283f, 0f),
                    new Vector3( 185f, 248f, 0f),
                    new Vector3( 265f, 220f, 0f),
                    new Vector3( 343f, 237f, 0f),
                    new Vector3( 421f, 256f, 0f),
                    new Vector3( 509f, 236f, 0f),
                    new Vector3( 562f, 172f, 0f),
                    new Vector3( 573f, 104f, 0f),
                    new Vector3( 519f, 40f, 0f),
                    new Vector3( 436f, 24f, 0f),
                    new Vector3( 361f, 37f, 0f),
                    new Vector3( 279f, 56f, 0f),
                    new Vector3( 185f, 33f, 0f),
                    new Vector3( 99f, 0f, 0f),
                    new Vector3( -34f, -26f, 0f),
                    new Vector3( -77f, 55f, 0f),
                    new Vector3( -157f, 93f, 0f),
                    new Vector3( -246f, 83f, 0f),
                    new Vector3( -320f, 42f, 0f),
                    new Vector3( -392f, -4f, 0f),
                    new Vector3( -443f, -129f, 0f),
                    new Vector3( -429f, -222f, 0f),
                    new Vector3( -409f, -296f, 0f),
                    new Vector3( -335f, -329f, 0f),
                    new Vector3( -243f, -318f, 0f),
                    new Vector3( -150f, -269f, 0f),
                };
                        break;
                    case 5:
                        roadPosList = new List<Vector3>()
                {
                    new Vector3( -558f, 160f, 0f),
                    new Vector3( -470f, 218f, 0f),
                    new Vector3( -390f, 243f, 0f),
                    new Vector3( -300f, 231f, 0f),
                    new Vector3( -220f, 260f, 0f),
                    new Vector3( -129f, 295f, 0f),
                    new Vector3( -51f, 304f, 0f),
                    new Vector3( 35f, 308f, 0f),
                    new Vector3( 108f, 283f, 0f),
                    new Vector3( 185f, 248f, 0f),
                    new Vector3( 265f, 220f, 0f),
                    new Vector3( 343f, 237f, 0f),
                    new Vector3( 421f, 256f, 0f),
                    new Vector3( 509f, 236f, 0f),
                    new Vector3( 562f, 172f, 0f),
                    new Vector3( 573f, 104f, 0f),
                    new Vector3( 519f, 40f, 0f),
                    new Vector3( 436f, 24f, 0f),
                    new Vector3( 361f, 37f, 0f),
                    new Vector3( 279f, 56f, 0f),
                    new Vector3( 185f, 33f, 0f),
                    new Vector3( 99f, 0f, 0f),
                    new Vector3( -34f, -26f, 0f),
                    new Vector3( -118f, -67f, 0f),
                    new Vector3( -166f, -128f, 0f),
                    new Vector3( -172f, -199f, 0f),
                    new Vector3( -150f, -269f, 0f),
                };
                        break;
                }
                break;
        }
        
        return roadPosList;
}
    //获取路劲萝卜坐标
    public List<Vector3> GetLuoboPosList(int map,string part)
    {
        List<Vector3> partPosList = null;
        if (map == 1)
        {
            ///*
            switch (part)
            {
                case "11":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -197f, 236f, 0f),
                            new Vector3( -144f, 252f, 0f),
                            new Vector3( -88f, 262f, 0f),
                            new Vector3( -39f, 269f, 0f),
                            new Vector3( 19f, 273f, 0f),
                            new Vector3( 79f, 269f, 0f),
                        };
                    
                    break;
                case "12":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 270f, 261f, 0f),
                            new Vector3( 326f, 279f, 0f),
                            new Vector3( 386f, 285f, 0f),
                            new Vector3( 447f, 287f, 0f),
                            new Vector3( 499f, 271f, 0f),
                            new Vector3( 522f, 231f, 0f),
                        };
                    break;
                case "13":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 519f, 104f, 0f),
                            new Vector3( 478f, 51f, 0f),
                            new Vector3( 425f, 7f, 0f),
                            new Vector3( 370f, -42f, 0f),
                            new Vector3( 319f, -100f, 0f),
                        };
                    break;
                case "21":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -233f, 125f, 0f),
                            new Vector3( -182f, 117f, 0f),
                            new Vector3( -131f, 107f, 0f),
                            new Vector3( -97f, 77f, 0f),
                        };
                    
                    break;
                case "22":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 57f, 44f, 0f),
                            new Vector3( 109f, 23f, 0f),
                            new Vector3( 149f, -1f, 0f),
                            new Vector3( 175f, -40f, 0f),
                        };
                    
                    break;
                case "31":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -482f, -132f, 0f),
                            new Vector3( -473f, -182f, 0f),
                            new Vector3( -447f, -215f, 0f),
                            new Vector3( -391f, -217f, 0f),
                            new Vector3( -337f, -226f, 0f),
                            new Vector3( -281f, -232f, 0f),
                        };
                    break;
                case "32":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -83f, -304f, 0f),
                            new Vector3( -38f, -326f, 0f),
                            new Vector3( 14f, -335f, 0f),
                            new Vector3( 69f, -322f, 0f),
                            new Vector3( 116f, -300f, 0f),
                            new Vector3( 156f, -276f, 0f),
                        };
                    break;
                case "42":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -41f, -29f, 0f),
                            new Vector3( -50f, -84f, 0f),
                            new Vector3( -78f, -133f, 0f),
                            new Vector3( -116f, -167f, 0f),
                        };
                    break;

                default:break;
            }
            //*/
        }else if(map == 3)
        {
            switch (part)
            {
                case "11":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -228f, 222f, 0f),
                            new Vector3( -144f, 223f, 0f),
                            new Vector3( -65f, 222f, 0f),
                            new Vector3( 13f, 215f, 0f),
                            new Vector3( 85f, 207f, 0f),
                            new Vector3( 170f, 206f, 0f),
                        };
                    break;
                case "12":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 556f, 150f, 0f),
                            new Vector3( 553f, 84f, 0f),
                            new Vector3( 501f, 36f, 0f),
                            new Vector3( 408f, 27f, 0f),
                            new Vector3( 330f, 34f, 0f),
                            new Vector3( 248f, 41f, 0f),
                        };
                    
                    break;
                case "13":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -144f, -97f, 0f),
                            new Vector3( -167f, -160f, 0f),
                        };
                    
                    break;
                case "21":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -556f, 71f, 0f),
                            new Vector3( -547f, 21f, 0f),
                        };
                    break;
                case "22":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -355f, 15f, 0f),
                            new Vector3( -301f, 47f, 0f),
                            new Vector3( -246f, 70f, 0f),
                            new Vector3( -186f, 81f, 0f),
                            new Vector3( -124f, 81f, 0f),
                            new Vector3( -80f, 45f, 0f),
                        };
                    break;
                case "31":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -556f, 71f, 0f),
                            new Vector3( -547f, 21f, 0f),
                        };
                   
                    break;
                case "32":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -448f, -149f, 0f),
                            new Vector3( -447f, -207f, 0f),
                            new Vector3( -434f, -250f, 0f),
                            new Vector3( -393f, -278f, 0f),
                            new Vector3( -336f, -288f, 0f),
                            new Vector3( -275f, -291f, 0f),
                        };
                    break;
                case "42":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -41f, -29f, 0f),
                            new Vector3( -50f, -84f, 0f),
                            new Vector3( -78f, -133f, 0f),
                            new Vector3( -116f, -167f, 0f),
                        };
                    break;

                default: break;
            }
        }else if(map == 2)
        {
            switch (part)
            {
                case "11":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -311f, 240f, 0f),
                            new Vector3( -242f, 240f, 0f),
                            new Vector3( -174f, 240f, 0f),
                            new Vector3( -108f, 234f, 0f),
                            new Vector3( -44f, 234f, 0f),
                            new Vector3( 21f, 232f, 0f),
                        };

                    break;
                case "12":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 278f, 198f, 0f),
                            new Vector3( 338f, 179f, 0f),
                            new Vector3( 388f, 154f, 0f),
                            new Vector3( 445f, 127f, 0f),
                            new Vector3( 485f, 86f, 0f),
                            new Vector3( 480f, 29f, 0f),
                        };
                    break;
                case "13":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 403f, -112f, 0f),
                            new Vector3( 354f, -164f, 0f),
                        };
                    break;
                case "21":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( 29f, -31f, 0f),
                            new Vector3( 84f, -61f, 0f),
                        };

                    break;
                case "22":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -419f, 117f, 0f),
                            new Vector3( -365f, 90f, 0f),
                            new Vector3( -313f, 64f, 0f),
                            new Vector3( -255f, 48f, 0f),
                            new Vector3( -202f, 27f, 0f),
                            new Vector3( -142f, 12f, 0f),
                        };

                    break;
                
                case "31":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -589f, 44f, 0f),
                            new Vector3( -596f, -21f, 0f),
                            new Vector3( -605f, -72f, 0f),
                            new Vector3( -603f, -126f, 0f),
                            new Vector3( -587f, -172f, 0f),
                            new Vector3( -555f, -210f, 0f),
                            
                        };
                    break;
                case "32":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -363f, -269f, 0f),
                            new Vector3( -298f, -269f, 0f),
                            new Vector3( -235f, -259f, 0f),
                            new Vector3( -173f, -241f, 0f),
                            new Vector3( -121f, -215f, 0f),
                            new Vector3( -68f, -170f, 0f),
                        };
                    break;
                case "42":
                    partPosList = new List<Vector3>()
                        {
                            new Vector3( -41f, -29f, 0f),
                            new Vector3( -50f, -84f, 0f),
                            new Vector3( -78f, -133f, 0f),
                            new Vector3( -116f, -167f, 0f),
                        };
                    break;

                default: break;
            }
        }
        return partPosList;
    }
    public bool isInDemo(int map,string shiziluName)
    {
        bool boo = false;
        switch (map)
        {
            case 2:
                if(shiziluName == "1_0" || shiziluName == "2_0" || shiziluName == "3_0")
                {
                    boo = true;
                }
                break;
            case 3:
                if (shiziluName == "1_0" || shiziluName == "2_3")
                {
                    boo = true;
                }
                break;
            case 1:
                if (shiziluName == "1_2_4" || shiziluName == "3_5")
                {
                    boo = true;
                }
                break;
        }
        return boo;
    }
    //获取最后一个十字路
    public string getLastShiZiLuName(int map)
    {
        string str = null;
        switch (map)
        {
            case 1:
                str = "shizilu_1_2_3_4_5_end";
                break;
            case 2:
                str = "shizilu_1_2_3_end";
                break;
            case 3:
                str = "shizilu_1_2_3_end";
                break;
        }
        return str;
    }
    //获取第一个十字路
    public string getFirstShiZiLuName(int map)
    {
        string str = null;
        switch (map)
        {
            case 1:
                str = "shizilu_1_2_3_4_5";
                break;
            case 2:
                str = "shizilu_1_2_3";
                break;
            case 3:
                str = "shizilu_1_2_3";
                break;
        }
        return str;
    }
    //获取坑图片名称
    public string getkengName(int map)
    {
        string str = null;
        switch (map)
        {
            case 1:
                str = "keng_2";
                break;
            case 2:
                str = "keng_1";
                break;
            case 3:
                str = "keng_1";
                break;
        }
        return str;
    }
    //获取出口坐标
    public Vector3 getHomePos(int map)
    {
        Vector3 pos = Vector3.zero;
        switch (map)
        {
            case 1:
                pos =  new Vector3(404, -391, 0); ;
                break;
            case 2:
                pos = new Vector3(404, -391, 0); ;
                break;
            case 3:
                pos = new Vector3(404, -391, 0); ;
                break;
        }
        return pos;
    }
    //获取兔子开始坐标
    public Vector3 getRabbitPos(int map)
    {
        Vector3 pos = Vector3.zero;
        switch (map)
        {
            case 1:
                pos = new Vector3(-558f, 160f, 0f);
                break;
            case 2:
                pos = new Vector3(-558f, 137f, 0f);
                break;
            case 3:
                pos = new Vector3(-558f, 137f, 0f);
                break;
        }
        return pos;
    }
    //获取地图有几条路
    public int GetMapRoadNum(int map)
    {
        int roadNum = 0;
        switch (map)
        {
            case 1:
                roadNum = 5;
                break;
            case 2:
                roadNum = 3;
                break;
            case 3:
                roadNum = 3;
                break;
        }
        return roadNum;
    }
    //获取正确路段的候选列表
    public List<int> getOkRoadList(int map,int guanka)
    {
        List<int> list = null;
        switch (map)
        {
            case 1:
                if(guanka == 1)
                {
                    list = Common.BreakRank(new List<int>() { 1, 2, 3 });
                }
                else
                {
                    list = Common.BreakRank(new List<int>() { 1, 4, 5 });
                }
                break;

            case 2:
                if (guanka == 1)
                {
                    list = Common.BreakRank(new List<int>() { 1, 2, 3 });
                }
                else
                {
                    list = Common.BreakRank(new List<int>() { 1,1 });
                }
                break;
            case 3:
                if (guanka == 1)
                {
                    list = Common.BreakRank(new List<int>() { 1, 2, 3 });
                }
                else
                {
                    list = Common.BreakRank(new List<int>() { 1, 2 });
                }
                break;
        }
        return list;
    }
    //获取所有萝卜块
    public List<string> getPartName(int map)
    {
        List<string> list = null;
        switch (map)
        {
            case 1:
                list = new List<string>() { "11", "12", "13", "21", "22", "31", "32", "42" };
                break;
            case 2:
                list = new List<string>() { "11", "12", "13", "21", "22", "31", "32" };
                break;
            case 3:
                list = new List<string>() { "11", "12", "13", "21", "22", "32"};
                break;
        }
        return list;
    }
    //获取公共部分
    public List<string> getComPartName(int map)
    {
        List<string> list = null;
        switch (map)
        {
            case 1:
                list = new List<string>() { "21", "22", "31", "32", "42" };
                break;
            case 2:
                list = new List<string>();
                break;
            case 3:
                list = new List<string>() { "13","21"};
                break;
        }
        return list;
    }
    //获取出口箭头坐标
    public Vector3 getExitPos(int map)
    {
        Vector3 pos = Vector3.zero;
        switch (map)
        {
            case 1:
                pos = new Vector3(330, -318, 0);
                break;
            case 2:
                pos = new Vector3(368, -361, 0);
                break;
            case 3:
                pos = new Vector3(326, -333, 0);
                break;
        }
        return pos;
    }

    //获取获取进房子的路劲
    public List<Vector3> getIntoHomePos(int map)
    {
        List<Vector3> list = null;
        switch (map)
        {
            case 1:
                list = new List<Vector3>() { new Vector3(335, -328, 0), new Vector3(414, -375, 0), new Vector3(511, -362, 0), new Vector3(700, -350, 0) };
                break;
            case 2:
                list = new List<Vector3>() { new Vector3(335, -328, 0), new Vector3(414, -375, 0), new Vector3(511, -362, 0), new Vector3(700, -350, 0) };
                break;
            case 3:
                list = new List<Vector3>() { new Vector3(-38, -292, 0), new Vector3(73, -293, 0), new Vector3(176, -287, 0),new Vector3(266, -310, 0), new Vector3(388, -375, 0), new Vector3(511, -362, 0), new Vector3(700, -350, 0) };
                break;
        }
        return list;
    }

    //获取获取地图
    public int getMap(string guankaStr)
    {
        int map = 1;
        switch (guankaStr)
        {
            case "2_0":
            case "2_1":
                map = 1;
                break;
            case "1_0":
                map = 2;
                break;
            case "1_1":
                map = 3;
                break;
        }
        return map;
    }
}
