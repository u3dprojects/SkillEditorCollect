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
    GameObject _gobjFab;
    public GameObject gobjFab { get {
            return _gobjFab;
        }
        set {
            if(value != _gobjFab)
            {
                _gobjFab = value;

                Reset(_gobjFab);
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

    // 挂节点
    public Transform trsfParent { get; set; }

    // 偏移
    public Vector3 v3LocPos { get; set; }

    // 旋转
    public Vector3 v3LocEulerAngle { get; set; }

    // 缩放
    public float scale { get; set; }

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

    void Reset(GameObject gobj)
    {
        DoInit(gobj);
    }

    public void DoInit(GameObject gobj)
    {
        if (gobj == null)
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

        UnityEngine.Object parentObject = this.gobjFab;
        UnityEditor.PrefabType p_type = UnityEditor.PrefabUtility.GetPrefabType(this.gobjFab);
        switch (p_type)
        {
            case UnityEditor.PrefabType.PrefabInstance:
                parentObject = UnityEditor.PrefabUtility.GetPrefabParent(this.gobjFab);
                break;
            case UnityEditor.PrefabType.Prefab:
                break;
        }

        this.gobjFab = parentObject as GameObject;
        path = GetPath(parentObject);
        // Debug.Log(path);

        this.fab_path = path;
        this.name = Path.GetFileName(this.fab_path);
    }


    string GetPath(UnityEngine.Object parentObj)
    {
        return UnityEditor.AssetDatabase.GetAssetPath(parentObj);
    }
    
    public void DoClear()
    {
        gobjFab = null;
        fab_path = "";
        
        time = 0;
        bind_bones_type = 0;
        trsfParent = null;
        isChanged = false;
        v3LocPos = Vector3.zero;
        v3LocEulerAngle = Vector3.zero;
        scale = 1;
    }
}
