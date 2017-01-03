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
    float _curSize = -1;
    float _curScale = 1;

    // 当前速度的倍数值
    float _cur_speed_rate = 1;

    // 1:play,2:pause,other:stop
    int _curState = 0;

    // 该粒子的最长时间
    float _maxTime = 0.0f;

    // 当前操作对象
    GameObject gobjEdtity = null;
    Transform trsfEntity = null;
    Vector3 v3DefScale = Vector3.one;

    // 用于计算的

    // 有效时间(<0则需要自己管理,=0就用最长时间判断，>0则用该时间进行判断)
    float m_time_out = 0.0f;

    // 运行时间
    float m_run_time = 0.0f;

    // 循环次数
    int m_loop_count = 0;

    // 播放完毕
    bool m_isRunOver = false;

    // 可控制的循环次数
    public int loopTimes { get;set; }

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
        this.gobjEdtity = gobj;
        this.trsfEntity = gobj.transform;
        this.v3DefScale = this.trsfEntity.localScale;

        ParticleSystem[] arr = gobjEdtity.GetComponentsInChildren<ParticleSystem>(true);
        Renderer[] arrRenders = gobjEdtity.GetComponentsInChildren<Renderer>(true);
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
        gobjEdtity.SetActive(false);
#if UNITY_EDITOR
        GameObject.DestroyImmediate(gobjEdtity);
#else
        GameObject.Destroy(gobj);
#endif
        DoClear();
        
    }

    public void DoClear() {
        DoClearParticle(true);

        this.gobjEdtity = null;
        this.trsfEntity = null;

        lens = 0;
        _cur_speed_rate = 1;
        _maxTime = 0.0f;

        listAll.Clear();
        listAllRenders.Clear();
        dicDefaultScale.Clear();

        OnInitReckonAttrs();
    }

    void OnInitReckonAttrs()
    {
        m_time_out = 0.0f;
        m_run_time = 0.0f;
        m_loop_count = 0;
        loopTimes = 0;
        m_isRunOver = false;
    }

    void DoClearParticle(bool isRestart = false)
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
                if (isRestart)
                {
                    ps.time = 0;
                }
            }
        }
    }

    public void SetSize(float size)
    {
        if (lens <= 0 || size < 0 || size == _curSize)
        {
            return;
        }
        _curSize = size;
        ParticleSystem ps;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            ps.startSize = _curSize;
        }
    }


    public void SetScale(float _scale)
    {
        bool isOkey = SetScalePs(_scale);
        if (isOkey)
        {
            this.trsfEntity.localScale = this.v3DefScale * _scale;
        }
    }

    bool SetScalePs(float _scale)
    {
        if (lens <= 0 || _scale < 0 || _scale == _curScale)
        {
            return false;
        }
        _curScale = _scale;

        DoClearParticle();

        ParticleSystem ps;
        List<float> vList;
        for (int i = 0; i < lens; i++)
        {
            ps = listAll[i];
            vList = dicDefaultScale[ps.GetInstanceID()];
            ps.startSize = _curScale * (vList[0]);
            ps.gravityModifier = _curScale * (vList[1]);
            ps.startSpeed = _curScale * (vList[2]);
        }
        return true;
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

    public float maxTime
    {
        get
        {
            return _maxTime + 0.005f;
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

    public bool isEnd
    {
        get
        {
            if (lens <= 0)
            {
                return true;
            }
            return this.m_isRunOver;
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
            ps.startSpeed = _cur_speed_rate * (vList[2]);
            ps.playbackSpeed = _cur_speed_rate * (vList[3]);
        }
    }

    public void Play()
    {
        ChangeState(1);
    }

    public void DoStart(float timeOut = 0.0f)
    {
        OnInitReckonAttrs();
        m_time_out = timeOut;

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

    public void DoUpdate(float deltatime)
    {
        if(lens <= 0)
        {
            return;
        }

        OnUpdateTime(deltatime);

        if (!Application.isPlaying)
        {
            Simulate(deltatime, false, false);
        }
    }

    void OnUpdateTime(float deltatime)
    {
        if(this.loopTimes <= 0) {
            if (this.m_time_out > 0)
            {
                this.m_isRunOver = this.m_time_out <= this.m_run_time;
            }
            else if (this.m_time_out == 0)
            {
                this.m_isRunOver = this.maxTime <= this.m_run_time;
            }
        }else
        {
            this.m_isRunOver = this.m_loop_count >= this.loopTimes;
        }

        this.m_run_time += deltatime;

        this.m_loop_count = (int)(this.m_run_time / this.maxTime);
    }
}
