using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Explanation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private bool pointerOn;
    private GameObject explanation;

	// Use this for initialization
	void Start () {
        pointerOn = false;
        explanation = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && pointerOn)
        {
            //Debug.Log("RightClicked");
            explanation.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            explanation.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOn = false;
    }
}
