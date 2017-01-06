using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : R-Runnin,D-Demo，T-Test 运行模式下的代码测试类
/// 作者 : Canyon
/// 日期 : 2017-01-06 17:10
/// 功能 : 
/// </summary>
public class RD_DT1 : MonoBehaviour {

    public bool isOpen = true;
    float timeOut = 3;
    float timeCur = 0;

	// Use this for initialization
	void Start () {
        StartCoroutine(Test());
        timeOut = 3;
        timeCur = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeCur >= timeOut)
        {
            // 当隐藏时，该脚本开启的协程会主动关闭
            // gameObject.SetActive(false);
        }

        timeCur += Time.deltaTime;
    }

    IEnumerator Test()
    {
        while (isOpen)
        {
            Debug.Log("RD_DT1 Test");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
