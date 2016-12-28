using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 类名 : 事件
/// 作者 : Canyon
/// 日期 : 2016-12-27 09:10
/// 功能 : 
/// </summary>
[System.Serializable]
public class EA_Event<T> : System.Object
{
    // 数据表中的标识
    
    public T id {
        get;
        set;
    }

    System.Action<T> callFunc;

    public EA_Event(T id, System.Action<T> call)
    {
        this.id = id;
        if (call != null)
        {
            this.callFunc = call;
        }
    }

    public void DoClear()
    {
        this.id = default(T);
        DoClearDelegate();
    }

    public void AppendFunc(System.Action<T> call)
    {
        if (call != null)
        {
            if (this.callFunc == null)
            {
                this.callFunc = call;
            }
            else
            {
                this.callFunc += call;
            }
        }
    }

    void DoClearDelegate()
    {
        if (this.callFunc != null)
        {
            System.Delegate[] dels = this.callFunc.GetInvocationList();
            if (dels != null && dels.Length > 0)
            {
                foreach (System.Delegate det in dels)
                {
                    this.callFunc -= (det as System.Action<T>);
                }
            }
            this.callFunc = null;
        }
    }

    public void DoDelegate()
    {
        if(this.callFunc != null)
        {
            this.callFunc(this.id);
        }
    }
}
