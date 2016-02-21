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
    private Text DaramAmountText;

    private float LatestClick = 1E8f;
    private bool QuantityControlOn = false;
    private bool pointerOn;
    private int DaramCost;
    private int DaramHP;
    private float DeveloperTime = 0;
    private int DaramAmount = 1;

    void Start() {
        gm = GameManager.gm;
        button = GetComponent<Button>();
        DaramCost = daram.GetComponent<Daram>().Cost;
        DaramHP = daram.GetComponent<Daram>().InitialHP;
        daramInfo = transform.GetChild(0).gameObject;
        daramInfoText = daramInfo.GetComponentInChildren<Text>();
        daramInfoText.text = daram.name + "\n\n가격 : " + DaramCost + "\n체력 : " + DaramHP + "\n특성 : " + daram.GetComponent<Daram>().feature;
        pointerOn = false;
        DaramAmountText = transform.GetChild(2).GetComponentInChildren<Text>();
    }

    void Update()
    {
        // 해금되었는지 확인
        string key = "UnlockDaram" + daram.GetComponent<Daram>().Level;
        button.interactable = Unlockables.GetBool(key);

        DaramAmountText.text = DaramAmount.ToString();

        if (GameManager.gm.isPaused || !button.interactable) return;

        if (Input.GetMouseButtonDown(0))
            LatestClick = gm.time;
        else if (Input.GetMouseButton(0) && pointerOn && !QuantityControlOn && gm.time - LatestClick > 0.15f )
        {
            QuantityControlStart();
        }
        if (Input.GetMouseButtonUp(0) && QuantityControlOn)
        {
            QuantityControlEnd();
        }


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
            Create(1); // 개발자가 뿌리는 다람쥐는 돈이 들지 않음 (대신 개발자에게 따로 월급을 줌)
            DeveloperTime = gm.time + 3.0f / Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv1")];
        }

        if (GameManager.gm.isInterRound == false && daram.GetComponent<Daram>().Level == 2
            && Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv2")] != 0 && gm.time >= DeveloperTime)
        {
            Create(1); // 개발자가 뿌리는 다람쥐는 돈이 들지 않음 (대신 개발자에게 따로 월급을 줌)
            DeveloperTime = gm.time + 3.0f / Developer.dev.developerCount[Developer.dev.FindPostIDByName("DaramLv2")];
        }
    }

    public void OnClick()
    {
        if (gm.isPaused) return;

        bool exception = false;
        if(QuantityControlOn)
            exception = QuantityControlEnd();
        if (exception == true)
            return;

        if (gm.money < DaramCost * DaramAmount)
        {
            LogText.WriteLog("돈이 부족합니다.");
            return;
        }
        GameManager.gm.money -= DaramCost * DaramAmount;
        Create(DaramAmount);
    }

    private void Create(int amount)
    {
        if (gm.isPaused) return;
        for (int i = 0; i < amount; i++)
        {
            Vector2 pos = GameManager.gm.RandomPosition();
            Instantiate(daram, pos, Quaternion.identity);
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

    private int n_up = 0, n_down = 0;
    void QuantityControlStart()
    {
        GameObject up = transform.GetChild(1).GetChild(0).gameObject;
        GameObject down = transform.GetChild(1).GetChild(1).gameObject;

        switch (DaramAmount)
        {
            case 1:
                n_up = 10;
                n_down = 0;
                break;
            case 10:
                n_up = 100;
                n_down = 1;
                break;
            case 100:
                n_up = 0;
                n_down = 10;
                break;
        }
        if (n_up != 0 && Unlockables.GetBool("UnlockDaram" + daram.GetComponent<Daram>().Level + "_Amount" + n_up) == true)
        {
            up.SetActive(true);
            up.GetComponentInChildren<Text>().text = n_up.ToString();
        }
        if(n_down != 0 && Unlockables.GetBool("UnlockDaram" + daram.GetComponent<Daram>().Level + "_Amount" + n_down) == true)
        {
            down.SetActive(true);
            down.GetComponentInChildren<Text>().text = n_down.ToString();
        }

        QuantityControlOn = true;
    }

    bool QuantityControlEnd()
    {
        bool ret = false;
        GameObject up = transform.GetChild(1).GetChild(0).gameObject;
        GameObject down = transform.GetChild(1).GetChild(1).gameObject;

        if (pointerOn)
        {
            float y = Input.mousePosition.y;
            if (y >= transform.position.y + 20)
            {
                DaramAmount *= 10;
                ret = true;
            }
            else if (y <= transform.position.y - 30)
            {
                DaramAmount /= 10;
                ret = true;
            }
        }

        up.SetActive(false);
        down.SetActive(false);
        QuantityControlOn = false;

        return ret;
    }
}
