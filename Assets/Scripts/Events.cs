using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Events : MonoBehaviour {

    public GameObject canvas;

    private GameManager gm;

	void Start ()
    {
        gm = GameManager.gm;

        //gm.EventCheck += UnlockUpBasic;
	}

    void UnlockUpBasic()
    {
        //if (gm.Fame >= 5000)
        {
            LogText.WriteLog("인기에 힘입어 LV.2 다람쥐를 개발했다!");
            print(canvas.GetComponentInChildren<AddBasicDaram>().daram);

            gm.EventCheck -= UnlockUpBasic;
        }
    }
}
