using UnityEngine;
using System.Collections;

public class UnlockUpBasic : MonoBehaviour {

    public GameObject SecondTutorial_Box;
    public void OnClick()
    {
        Instantiate(SecondTutorial_Box);
    }
}
