using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventBox : MonoBehaviour {

    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GameManager.gm.Pause();
    }

    public void OnClick()
    {
        GameManager.gm.Pause();
        Destroy(gameObject);
    }
}
