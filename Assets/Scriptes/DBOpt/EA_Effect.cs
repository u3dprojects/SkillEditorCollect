using UnityEngine;
using System.IO;
using System.Collections;

/// <summary>
/// 类名 : U3D - 动作时间轴上的特效数据
/// 作者 : Canyon
/// 日期 : 2016-12-26 10:53
/// 功能 :  
/// </summary>
[System.Serializable]
public class EA_Effect : System.Object
{
    static public int INDEX_ID_ANIEFFECT = 0;

    public string unq_id { get; private set; }

    // 文件对象
    GameObject pre_gobj;

    GameObject _gobj;
    public GameObject gobj { get {
            return _gobj;
        }
        set {
            if(value != _gobj)
            {
                _gobj = value;
                this.isChanged = true;
            }
        }
    }

    public string fab_path { get; set; }

    // 文件名
    public string name { get; set; }

    // 触发时间
    float _trigger_time;
    public float time { get {
            return _trigger_time;
        }
        set {
            if(_trigger_time != value)
            {
                _trigger_time = value;
                this.isChanged = true;
            }
        }
    }

    // 绑定点模式
    public int bind_bones_type { get; set;}

    // 是否有改变
    public bool isChanged { get; set; }
    
    public EA_Effect(){
        DoClear();
        this.unq_id = string.Format("Atc_Effect_{0:d}", (INDEX_ID_ANIEFFECT++));
    }

    public void DoReInit(string path)
    {
        bool isExists = File.Exists(path);
        if (!isExists)
        {
            Debug.LogWarning(path + "不存在！！！");
            return;
        }
        UnityEngine.GameObject gobjFBX = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject)) as GameObject;
        DoInit(gobjFBX);
    }

    public void DoReInit(GameObject gobj)
    {
        DoClear();

        DoInit(gobj);
    }

    public void Reset(GameObject gobj)
    {
        if(this.pre_gobj != gobj)
        {
            DoReInit(gobj);
        }
    }

    public void DoInit(GameObject gobj)
    {
        this.gobj = gobj;
        this.pre_gobj = gobj;
        if (this.gobj == null)
        {
            DoClear();
            return;
        }

        string path = "";
        //string path2 = "";

        //path = UnityEditor.AssetDatabase.GetAssetPath(gobj);
        //path2 = UnityEditor.AssetDatabase.GetAssetOrScenePath(gobj);
        //Debug.Log(path);
        //Debug.Log(path2);

        //Object obj = UnityEditor.PrefabUtility.GetPrefabObject(gobj);
        //path = UnityEditor.AssetDatabase.GetAssetPath(obj);
        //path2 = UnityEditor.AssetDatabase.GetAssetOrScenePath(obj);
        //UnityEditor.AssetDatabase.GUIDToAssetPath();
        //Debug.Log(path);
        //Debug.Log(path2);

        UnityEngine.Object parentObject = this.gobj;
        UnityEditor.PrefabType p_type = UnityEditor.PrefabUtility.GetPrefabType(this.gobj);
        switch (p_type)
        {
            case UnityEditor.PrefabType.PrefabInstance:
                parentObject = UnityEditor.PrefabUtility.GetPrefabParent(this.gobj);
                break;
            case UnityEditor.PrefabType.Prefab:
                break;
        }

        path = GetPath(parentObject);
        Debug.Log(path);

        this.fab_path = path;
        this.name = Path.GetFileName(this.fab_path);
    }


    string GetPath(UnityEngine.Object parentObj)
    {
        return UnityEditor.AssetDatabase.GetAssetPath(parentObj);
    }
    
    public void DoClear()
    {
        gobj = null;
        pre_gobj = null;
        fab_path = "";
        
        time = 0;
        bind_bones_type = -1;
        isChanged = false;
    }

    public void Play()
    {
        UnityEngine.GameObject gobjFab = GameObject.Instantiate(this.gobj,Vector3.zero,Quaternion.identity) as GameObject;
        EDM_Particle.m_instance.DoActive(gobjFab);
    }
}
