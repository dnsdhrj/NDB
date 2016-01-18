using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UserBar : MonoBehaviour {

    public GameObject BarPrefab;

    private Slider[] sliders;

    void Start()
    {
        sliders = new Slider[User.Count];

        //숫자가 큰것부터 배치
        sliders[1] = CreateBar(Color.green);
        sliders[0] = CreateBar(Color.yellow);
    }

    void Update()
    {
        int all = GameManager.gm.UserAllCount();
        int sum = 0;
        for (int i = 0; i < User.Count; i++)
        {
            sum += GameManager.gm.UserCount[i];
            sliders[i].value = sum / (float)all;
        }
    }

    Slider CreateBar(Color color)
    {
        GameObject obj = (GameObject)Instantiate(BarPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.GetComponentInChildren<Image>().color = color;

        return obj.GetComponent<Slider>();
    }
}
