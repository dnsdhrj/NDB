using UnityEngine;
using System.Collections;

public class AddBasicDaram : MonoBehaviour {

    public GameObject daram;

    public void OnClick()
    {
        Vector2 pos = Vector2.zero; //우선은 원점에만 생성
        Instantiate(daram, pos, Quaternion.identity);
    }
}
