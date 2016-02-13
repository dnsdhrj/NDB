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
    private int DaramHP;
    private float DeveloperTime = 0;

    void Start() {
        button = GetComponent<Button>();
        DaramCost = daram.GetComponent<Daram>().Cost;
        DaramHP = daram.GetComponent<Daram>().InitialHP;
        daramInfo = transform.GetChild(0).gameObject;
        daramInfoText = daramInfo.GetComponentInChildren<Text>();
        daramInfoText.text = daram.name + "\n\n가격 : " + DaramCost + "\n체력 : " + DaramHP + "\n특성 : " + daram.GetComponent<Daram>().feature;
        pointerOn = false;
    }

    void Update()
    {
        // 해금되었는지 확인
        string key = "UnlockDaram" + daram.GetComponent<Daram>().Level;
        button.interactable = Unlockables.GetBool(key);

        if (GameManager.gm.IsPaused || !button.interactable) return;

        if (Input.GetMouseButtonDown(1) && pointerOn)
        {
            //Debug.Log("RightClicked");
            daramInfo.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            daramInfo.SetActive(false);
        }

        // 개발자 한명당 3초에 한마리씩 뿌림
        if(GameManager.gm.IsInterRound == false && daram.GetComponent<Daram>().Level == 1)
            if (GameManager.gm.Developers != 0 && GameManager.gm.time >= DeveloperTime)
            {
                OnClick();
                DeveloperTime = GameManager.gm.time + 3.0f / GameManager.gm.Developers;
            }

    }

    public void OnClick()
    {
        if (GameManager.gm.IsPaused) return;
        else if (GameManager.gm.Money < DaramCost)
        {
            LogText.WriteLog("돈이 부족합니다.");
            return;
        }
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
