using UnityEngine;
using System.Collections;

public class RoundStartScene : MonoBehaviour {

    public void OnClick()
    {
        GameManager.gm.DoRoundStartEvent();
    }
}
