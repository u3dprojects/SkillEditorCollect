using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 类名 : U3D - 粒子系统数据
/// 作者 : Canyon
/// 日期 : 2016-12-26 10:53
/// 功能 :  
/// </summary>
[System.Serializable]
public class DBU3D_Particle : System.Object {
    List<ParticleSystem> listAll = new List<ParticleSystem>();
    List<Renderer> listAllRenders = new List<Renderer>();

    Dictionary<int, List<float>> dicDefaultScale = new Dictionary<int, List<float>>();

    int lens = 0;
    float curSize = -1;
    float _curScale = 1;

    // 当前速度的倍数值
    float _cur_speed_rate = 1;

    // 1:play,2:pause,other:stop
    int _curState = 0;

    // 该粒子的最长时间
    float _maxTime = 0.0f;

    // 当前操作对象
    GameObject gobj;

    public DBU3D_Particle() { }

    public DBU3D_Particle(GameObject gobj)
    {
        DoReInit(gobj);
    }
    
    public void DoReInit(GameObject gobj) {
        DoClear();
        DoInit(gobj);
    }

    public void DoReInit(Transform trsf)
    {
        DoReInit(trsf.gameObject);
    }

    void DoInit(GameObject gobj)
    {
        this.gobj = gobj;
        ParticleSystem[] arr = gobj.GetComponentsInChildren<ParticleSystem>(true);
        Renderer[] arrRenders = gobj.GetComponentsInChildren<Renderer>(true);
        if (arrRenders != null && arrRenders.Length > 0)
        {
            listAllRenders.AddRange(arrRenders);
        }

        if (arr != null && arr.Length > 0)
        {
            listAll.AddRange(arr);
        }

        lens = listAll.Count;
        if (lens <= 0)
        {
            return;
        }

        ParticleSystem ps;
        float curTime = 0f;
        int key;
        List<float> vList;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            curTime = ps.startDelay + ps.duration;
            if (curTime > _maxTime)
            {
                _maxTime = curTime;
            }

            key = ps.GetInstanceID();
            vList = new List<float>();
            vList.Add(ps.startSize);
            vList.Add(ps.gravityModifier);
            vList.Add(ps.startSpeed);
            vList.Add(ps.playbackSpeed);
            dicDefaultScale.Add(key, vList);
        }
    }

    public void DoDestory()
    {
        GameObject gobj = this.gobj;

        DoClear();

        if (gobj)
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(gobj);
#else
            GameObject.Destroy(gobj);
#endif
        }
    }

    public void DoClear() {
        DoClearParticle();

        this.gobj = null;
        lens = 0;
        _cur_speed_rate = 1;
        _maxTime = 0.0f;

        listAll.Clear();
        listAllRenders.Clear();
        dicDefaultScale.Clear();
    }

    void DoClearParticle()
    {
        if (lens <= 0)
        {
            return;
        }
        ParticleSystem ps;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            if (ps)
            {
                ps.Clear();
                ps.time = 0;
            }
        }
    }

    public void SetSize(float size)
    {
        if (lens <= 0 || size < 0 || size == curSize)
        {
            return;
        }
        curSize = size;
        ParticleSystem ps;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            ps.startSize = size;
        }
    }

    public void SetScale(float _scale)
    {
        if (lens <= 0 || _scale < 0 || _scale == _curScale)
        {
            return;
        }
        _curScale = _scale;

        DoClearParticle();

        ParticleSystem ps;
        List<float> vList;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            vList = dicDefaultScale[ps.GetInstanceID()];
            ps.startSize = _scale * (vList[0]);
            ps.gravityModifier = _scale * (vList[1]);
            // ps.startSpeed = _scale * (vList[2]);
        }
    }

    public void ChangeState(int state)
    {
        if (lens <= 0)
        {
            return;
        }
        ParticleSystem ps;
        _curState = state;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            switch (_curState)
            {
                case 1:
                    ps.Play(false);
                    break;
                case 2:
                    ps.Pause(false);
                    break;
                default:
                    ps.Stop(false);
                    ps.Clear(false);
                    ps.time = 0;
                    break;
            }
        }
    }

    public float curScale
    {
        get
        {
            return _curScale;
        }
    }

    public float lifeTime
    {
        get
        {
            return _maxTime - 0.01f;
        }
    }

    public float maxTime
    {
        get
        {
            return _maxTime + 0.01f;
        }
    }

    public void SetAlpha(float alpha)
    {
        int lensRender = listAllRenders.Count;
        if (lensRender <= 0 || alpha < 0)
        {
            return;
        }
        Renderer curRender;
        Material mat;
        Color col;
        alpha = alpha > 1 ? alpha / 255f : alpha;
        for (int i = 0; i < lensRender; i++)
        {
            curRender = listAllRenders[i];
            mat = GetMaterial(curRender);
            if (mat != null)
            {
                if (mat.HasProperty("_Color"))
                {
                    col = mat.GetColor("_Color");
                    col.a = alpha;
                    mat.SetColor("_Color", col);
                }

                if (mat.HasProperty("_TintColor"))
                {
                    col = mat.GetColor("_TintColor");
                    col.a = alpha;
                    mat.SetColor("_TintColor", col);
                }
            }
        }
    }

    Material GetMaterial(Renderer render)
    {
        if (render.material == null)
        {
            return render.sharedMaterial;
        }
        return render.material;
    }
    
    public int curState
    {
        get { return _curState; }
    }

    // 模拟快进 -在给定一段时间内通过模拟粒子快进粒子系统，然后暂停它
    public void Simulate(float timeProgress,bool withChildren = false,bool resStart = false)
    {
        if (lens <= 0)
        {
            return;
        }
        ParticleSystem ps;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            ps.Simulate(timeProgress, withChildren, resStart);
        }
    }

    public bool isEndMax
    {
        get
        {
            if (lens <= 0)
            {
                return true;
            }
            ParticleSystem ps;
            for (int i = 0; i < lens; i++)
            {
                ps = listAll[i];
                if (ps.time > maxTime)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool isEndLife
    {
        get
        {
            if (lens <= 0)
            {
                return true;
            }
            ParticleSystem ps;
            for (int i = 0; i < lens; i++)
            {
                ps = listAll[i];
                if(ps.time > lifeTime)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // 设置播放速度 1 为正常
    public void SetSpeed(float speed)
    {
        if (lens <= 0)
        {
            return;
        }
        ParticleSystem ps;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            ps.startSpeed = speed;
            ps.playbackSpeed = speed;
        }
    }

    public void SetSpeedRate(float speedRate)
    {
        if (lens <= 0 && _cur_speed_rate != speedRate)
        {
            return;
        }
        _cur_speed_rate = speedRate;

        DoClearParticle();

        ParticleSystem ps;
        List<float> vList;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            vList = dicDefaultScale[ps.GetInstanceID()];
            ps.startSpeed = speedRate * (vList[2]);
            ps.playbackSpeed = speedRate * (vList[3]);
        }
    }

    public void Play()
    {
        ChangeState(1);
    }

    public void DoStart()
    {
        if (Application.isPlaying) {
            ChangeState(1);
        }else
        {
            Simulate(0,false,true);
        }
    }

    public void Pause()
    {
        ChangeState(2);
    }

    public void Stop()
    {
        ChangeState(0);
    }
}
