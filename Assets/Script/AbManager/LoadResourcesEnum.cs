using UnityEngine;
using System.Collections;

public enum LoadResourcesEnum
{
    Ab,//打包到StreamingAssets,再解压读取
    ResCopy,//通过ResCopy拷贝到Resources，在里面读取

}
