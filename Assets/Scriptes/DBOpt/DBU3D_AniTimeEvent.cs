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
public class DBU3D_AniTimeEvent : System.Object{
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

    List<DBU3D_AniEvent<string>> lstCalls = new List<DBU3D_AniEvent<string>>();
    
    public DBU3D_AniTimeEvent(float pro, DBU3D_AniEvent<string> m_event)
    {
        this._progress = pro;
        this.is_doed = false;
        Add(m_event);
    }

    public void Add(DBU3D_AniEvent<string> m_event)
    {
        if (m_event != null)
        {
            this.lstCalls.Add(m_event);
        }
    }

    public DBU3D_AniEvent<string> GetEvent(string unqid)
    {
        DBU3D_AniEvent<string> retOne = null;
        DBU3D_AniEvent<string> tmp = null;
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

    public DBU3D_AniEvent<string> Remove(string unqid)
    {
        DBU3D_AniEvent<string> rmOne = GetEvent(unqid);
        if (rmOne != null)
        {
            Remove(rmOne);
        }
        return rmOne;
    }

    public DBU3D_AniEvent<string> Remove(DBU3D_AniEvent<string> m_event)
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
        foreach (DBU3D_AniEvent<string> item in lstCalls)
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
        foreach (DBU3D_AniEvent<string> item in lstCalls)
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
public class SortAniTimeEvent : IComparer<DBU3D_AniTimeEvent>
{
    public int Compare(DBU3D_AniTimeEvent x, DBU3D_AniTimeEvent y)
    {
        return x.progress < y.progress ? -1 : 1;
    }
}
