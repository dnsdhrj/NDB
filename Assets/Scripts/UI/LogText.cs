using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogText : MonoBehaviour {
    
    private Text logText;
    public string initialText = "The Game Start";
	
    // Use this for initialization
	void Start () 
    {
        logText = GetComponent<Text>();
        logText.text = initialText;
    }

    public void WriteLog(string log)
    {
        logText.text += "\n" + log;
    }
}
