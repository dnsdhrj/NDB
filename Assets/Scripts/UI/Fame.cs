using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fame : MonoBehaviour {

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        slider.value = GameManager.gm.Fame;
    }
}
