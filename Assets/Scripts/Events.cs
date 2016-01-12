using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Events : MonoBehaviour {

    private GameManager gm; //많이 쓸것같아서 만들어둠

    public GameObject UnlockUpBasic_Button;

    void Start ()
    {
        gm = GameManager.gm;

        gm.EventCheck += UnlockUpBasic;
    }

    void UnlockUpBasic()
    {
        if (GameManager.gm.Fame >= 5000)
        {
            LogText.WriteLog("인기에 힘입어 LV.2 다람쥐를 개발했다!");
            UnlockUpBasic_Button.GetComponent<Button>().interactable = true;

            GameManager.gm.EventCheck -= UnlockUpBasic;
        }
    }
}
