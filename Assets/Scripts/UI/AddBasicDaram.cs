using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddBasicDaram : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private GameManager gm;
    
    public GameObject daram;


    private GameObject daramInfo;
    private Text daramInfoText;
    private Button button;

    private bool pointerOn;
    private int DaramCost;
    private int DaramHP;
    private float DeveloperTime = 0;

    void Start() {
        gm = GameManager.gm;
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

        if (GameManager.gm.isPaused || !button.interactable) return;

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
        if(GameManager.gm.isInterRound == false && daram.GetComponent<Daram>().Level == 1
            && Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv1")] != 0 && gm.time >= DeveloperTime)
        {
            Create(); // 개발자가 뿌리는 다람쥐는 돈이 들지 않음 (대신 개발자에게 따로 월급을 줌)
            DeveloperTime = gm.time + 3.0f / Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv1")];
        }

        if (GameManager.gm.isInterRound == false && daram.GetComponent<Daram>().Level == 2
            && Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv2")] != 0 && gm.time >= DeveloperTime)
        {
            Create(); // 개발자가 뿌리는 다람쥐는 돈이 들지 않음 (대신 개발자에게 따로 월급을 줌)
            DeveloperTime = gm.time + 3.0f / Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv2")];
        }
    }

    public void OnClick()
    {
        if (gm.isPaused) return;
        else if (gm.money < DaramCost)
        {
            LogText.WriteLog("돈이 부족합니다.");
            return;
        }
        GameManager.gm.money -= DaramCost;
        Create();
    }

    private void Create()
    {
        if (gm.isPaused) return;
        Vector2 pos = GameManager.gm.RandomPosition();
        Instantiate(daram, pos, Quaternion.identity);
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
