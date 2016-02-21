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
        userCountText.text = "전체 유저 수 : " + GameManager.gm.UserAllCount();
	}
}
