using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddBasicDaram : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject daram;


    private GameObject daramInfo;
    private Text daramInfoText;
    private Button button;

    private bool pointerOn;
    private int DaramCost;

    void Start() {
        button = GetComponent<Button>();
        DaramCost = daram.GetComponent<Daram>().Cost;
        daramInfo = transform.GetChild(0).gameObject;
        daramInfoText = daramInfo.GetComponentInChildren<Text>();
        daramInfoText.text = daram.name + "\n\n가격 : " + DaramCost + "\n특성 : " + daram.GetComponent<Daram>().feature;
        pointerOn = false;
    }

    void Update()
    {
        if (GameManager.gm.IsPaused || !button.interactable) return;

        if (Input.GetMouseButtonDown(1) && pointerOn)
        {
            Debug.Log("RightClicked");
            daramInfo.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            daramInfo.SetActive(false);
        }
    }

    public void OnClick()
    {
        if (GameManager.gm.IsPaused) return;
        Vector2 pos = GameManager.gm.RandomPosition();
        Instantiate(daram, pos, Quaternion.identity);
        GameManager.gm.Money -= DaramCost;
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
