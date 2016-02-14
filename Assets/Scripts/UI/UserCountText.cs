using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserCountText : MonoBehaviour {
    private Text userCountText;
	// Use this for initialization
	void Start () {
        userCountText = GetComponent<Text>();
        
	}
	
	// Update is called once per frame
	void Update () {
        userCountText.text = "접속한 유저 수\n초보: "+GameManager.gm.userCount[User.level1]
            + "\n중수: " + GameManager.gm.userCount[User.level2];
	}
}
