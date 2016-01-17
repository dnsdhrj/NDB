using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddBasicDaram : MonoBehaviour {

    public GameObject daram;

    private int DaramCost;

    public void OnClick()
    {
        Vector2 pos = GameManager.gm.RandomPosition();
        Instantiate(daram, pos, Quaternion.identity);
        DaramCost = daram.GetComponent<Daram>().Cost;
        GameManager.gm.Money -= DaramCost;
    }


}
