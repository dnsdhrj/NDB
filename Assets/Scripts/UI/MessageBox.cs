using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour
{
    private float startTime;
    private float duration = 1f;

    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        GameManager.gm.Pause(true);
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > duration)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnClick()
    {
        Destroy(this.gameObject);      
    }
}
