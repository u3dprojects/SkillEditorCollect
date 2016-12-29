using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ED_DemoTest : MonoBehaviour {

    EA_Effect effect = new EA_Effect();

    DBOpt_Time time1 = new DBOpt_Time(false);
    DBOpt_Time time2 = new DBOpt_Time(false);

    // Use this for initialization
    void Start () {
        effect.DoReInit(this.gameObject);
        Debug.Log("= Start ED_DemoTest =");
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("= ED_DemoTest Update 1 =");
        time2.DoUpdateTime();
        time2.DoDebug("Update");
        new GameObject();
        Debug.Log("= ED_DemoTest Update 2 =");
    }

    public void OnUpdate()
    {
        time1.DoUpdateTime();
        time1.DoDebug("OnUpdate");
        // new GameObject();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
        EditorApplication.update += Update;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        EditorApplication.update -= Update;
    }
#endif
}
