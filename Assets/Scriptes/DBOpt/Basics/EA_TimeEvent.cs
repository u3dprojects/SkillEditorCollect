using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 类名 : animator 动作状态机 时间轴时间
/// 作者 : Canyon
/// 日期 : 2016-12-21 10:10
/// 功能 : 
/// </summary>
[System.Serializable]
public class EA_TimeEvent : System.Object{
    float _progress = 0.0f;
    public float progress
    {
        get { return _progress; }
    }

    bool is_doed = false;

    public bool isDoed
    {
        get
        {
            return is_doed;
        }
    }

    List<EA_Event<string>> lstCalls = new List<EA_Event<string>>();
    
    public EA_TimeEvent(float pro, EA_Event<string> m_event)
    {
        this._progress = pro;
        this.is_doed = false;
        Add(m_event);
    }

    public void Add(EA_Event<string> m_event)
    {
        if (m_event != null)
        {
            this.lstCalls.Add(m_event);
        }
    }

    public EA_Event<string> GetEvent(string unqid)
    {
        EA_Event<string> retOne = null;
        EA_Event<string> tmp = null;
        int lens = this.lstCalls.Count;
        for (int i = 0; i < lens; i++)
        {
            tmp = this.lstCalls[i];
            if (tmp.id == unqid)
            {
                retOne = tmp;
                break;
            }
        }
        return retOne;
    }

    public EA_Event<string> Remove(string unqid)
    {
        EA_Event<string> rmOne = GetEvent(unqid);
        if (rmOne != null)
        {
            Remove(rmOne);
        }
        return rmOne;
    }

    public EA_Event<string> Remove(EA_Event<string> m_event)
    {
        if (m_event != null && this.lstCalls.Contains(m_event))
        {
            this.lstCalls.Remove(m_event);
        }
        return m_event;
    }

    public void DoClear()
    {
        this._progress = 0.0f;
        this.is_doed = false;
        DoClearEvent();
    }

    public  void DoClearEvent()
    {
        if(lstCalls == null)
        {
            return;
        }

        foreach (EA_Event<string> item in lstCalls)
        {
            item.DoClear();
        }
        lstCalls.Clear();
    }

    public void DoEvent(float progress)
    {
        if (this.is_doed) {
            return;
        }
        if(this._progress > progress)
        {
            return;
        }

        this.is_doed = true;

        DoEvent();
    }

    void DoEvent()
    {
        foreach (EA_Event<string> item in lstCalls)
        {
            item.DoDelegate();
        }
    }

    public bool isEmpty
    {
        get
        {
            return this.lstCalls.Count <= 0;
        }
    }
}

// 按照时间进度排序
public class EA_Sort_TimeEvent : IComparer<EA_TimeEvent>
{
    public int Compare(EA_TimeEvent x, EA_TimeEvent y)
    {
        return x.progress < y.progress ? -1 : 1;
    }
}
