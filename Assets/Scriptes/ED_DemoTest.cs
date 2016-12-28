using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ED_DemoTest : MonoBehaviour {

    DBU3D_AniEffect effect = new DBU3D_AniEffect();

	// Use this for initialization
	void Start () {
        effect.DoReInit(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
