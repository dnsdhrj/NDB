using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    private bool pointerOn;
    private GameObject Info;

    void Start()
    {
        Info = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && pointerOn)
        {
            Info.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Info.SetActive(false);
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
