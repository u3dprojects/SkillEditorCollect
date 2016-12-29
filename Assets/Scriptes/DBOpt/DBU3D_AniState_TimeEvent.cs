using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 /// <summary>
 /// 类名 : 某天动作的时间轴事件
 /// 作者 : Canyon
 /// 日期 : 2016-12-28 14:10
 /// 功能 : 
 /// </summary>
[System.Serializable]
public class DBU3D_AniState_TimeEvent : System.Object {

    // 排序对象
    static EA_Sort_TimeEvent _sort_event = new EA_Sort_TimeEvent();

    // 动作时间轴-事件
    public List<EA_TimeEvent> lst_events = new List<EA_TimeEvent>();
    
    // 当前动作的特效事件
    public List<EA_Effect> lst_effects = new List<EA_Effect>();

    public DBU3D_AniState_TimeEvent() {
        DoClear();
    }
    
    public void DoClear()
    {
        OnClearAttEvent();
        OnClearAttEffect();
    }

    void OnClearAttEvent()
    {
        int lens = lst_events.Count;
        for (int i = 0; i < lens; i++)
        {
            (lst_events[i]).DoClear();
        }

        lst_events.Clear();
    }

    void OnClearAttEffect()
    {
        int lens = lst_effects.Count;
        for(int i = 0;i < lens; i++)
        {
            (lst_effects[i]).DoClear();
        }

        lst_effects.Clear();
    }
    
    public void AddEffect()
    {
        EA_Effect one_effect = new EA_Effect();
        lst_effects.Add(one_effect);
    }

    public void RemoveEffect(EA_Effect one)
    {
        lst_effects.Remove(one);
        RemoveEvent(one);
    }

    public void ResetEvents()
    {
        int lens = lst_effects.Count;
        EA_Effect one_effect;
        for (int i = 0; i < lens; i++)
        {
            one_effect = lst_effects[i];
            ResetOneEvent(one_effect);
        }
    }
    
    public void ResetOneEvent(EA_Effect one_effect)
    {
        one_effect.isChanged = false;

        EA_Event<string> m_event = RemoveEvent(one_effect);
        if (m_event == null) {
            m_event = new EA_Event<string>(one_effect.unq_id, delegate (string id)
            {
                EDM_Particle.m_instance.DoReady(one_effect.gobj);
            });
        }
        AddEvent(one_effect.time, m_event);
    }

    void AddEvent(float time, EA_Event<string> m_event)
    {
        EA_TimeEvent time_event = new EA_TimeEvent(time, m_event);
        lst_events.Add(time_event);
    }

    void DoClearEmptyEvent()
    {
        int lens = lst_events.Count;
        EA_TimeEvent one_time_event;
        List<EA_TimeEvent> empty = new List<EA_TimeEvent>();
        for (int i = 0; i < lens; i++)
        {
            one_time_event = lst_events[i];
            if (one_time_event.isEmpty)
            {
                empty.Add(one_time_event);
            }
        }

        lens = empty.Count;
        for (int i = 0; i < lens; i++)
        {
            one_time_event = empty[i];
            lst_events.Remove(one_time_event);
        }
    }

    EA_Event<string> RemoveEvent(string oneid)
    {
        if (string.IsNullOrEmpty(oneid))
            return null;

        EA_Event<string> ret = null;
        EA_Event<string> tmp = null;
        int lens = lst_events.Count;
        for (int i = 0; i < lens; i++)
        {
            tmp = (lst_events[i]).Remove(oneid);
            if (ret == null)
            {
                ret = tmp;
            }
        }
        DoClearEmptyEvent();
        return tmp;
    }

    EA_Event<string> RemoveEvent(EA_Effect one)
    {
        if(one != null)
            return RemoveEvent(one.unq_id);

        return null;
    }

    public void OnUpdate(float normalizedTime)
    {
        DoEvent(normalizedTime);
    }

    void DoEvent(float normalizedTime)
    {
        int lens = lst_events.Count;
        if (lens <= 0)
            return;
        lst_events.Sort(_sort_event);

        for (int i = 0; i < lens; i++)
        {
            (lst_events[i]).DoEvent(normalizedTime);
        }
    }
}
