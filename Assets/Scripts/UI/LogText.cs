using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogText : MonoBehaviour {
    
    private Text logText;

    public GameObject scrollbar;
    private Scrollbar logScrollbar;
    public string initialText = "The Game Start";
	
    // Use this for initialization
	void Awake () 
    {
        logText = GetComponent<Text>();
        logText.text = initialText;

        logScrollbar = scrollbar.GetComponent<Scrollbar>();
        logScrollbar.size = 0;
    }

    public void WriteLog(string log)
    {
        logText.text += "\n" + log;
        logScrollbar.value = 0;
    }
}
