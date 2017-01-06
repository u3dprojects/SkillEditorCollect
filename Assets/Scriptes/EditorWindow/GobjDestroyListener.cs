using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 对象销毁时候调用回调函数
/// 作者 : Canyon
/// 日期 : 2017-01-06 11:10
/// 功能 : 
/// </summary>
public class GobjDestroyListion : MonoBehaviour {

    System.Action Call4Destroy;
    
    protected virtual void OnDestroy()
    {
        Debug.Log("== OnDestroy ==");
        if (Call4Destroy != null)
        {
            Call4Destroy();
            this.Call4Destroy = null;
        }
    }

    public void AddCall4Destroy(System.Action funcCall)
    {
        if(this.Call4Destroy == null)
        {
            this.Call4Destroy = funcCall;
        }else
        {
            this.Call4Destroy += funcCall;
        }
    }
}
