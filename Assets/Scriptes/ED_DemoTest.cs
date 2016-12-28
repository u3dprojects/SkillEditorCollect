using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ED_DemoTest : MonoBehaviour {

    EA_Effect effect = new EA_Effect();

	// Use this for initialization
	void Start () {
        effect.DoReInit(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
