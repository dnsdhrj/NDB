using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpgradeCheckup : MonoBehaviour {

    public GameObject UpgradeListTemplate;
    static public UpgradeCheckup _this;

    private GameObject upgradePanel;
    private GameObject upgradeScrollPanel;
    private UpgradeDatabase database;
    private RectTransform upgradeScrollPanelrect;
    private int imageIconSize = 256;

    private List<GameObject> upgradeList = new List<GameObject>();

    void Start()
    {
        _this = this;
        database = GameManager.gm.GetComponentInChildren<UpgradeDatabase>();
        upgradePanel = GameObject.Find("UpgradePanel");
        upgradeScrollPanel = GameObject.Find("UpgradeScrollPanel");
        upgradeScrollPanelrect = upgradeScrollPanel.GetComponent<RectTransform>();
        upgradePanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        upgradePanel.SetActive(false);
        MakeUpgradeList();
    }

    void MakeUpgradeList()
    {
        SetListSize(upgradeScrollPanelrect);
        foreach (Upgrade Upgrade in database.upgradeDatabase)
        {
            GameObject newupgrade = Instantiate(UpgradeListTemplate, new Vector3(0f, (upgradeScrollPanelrect.rect.height / 2) - 120f * Upgrade.upgradeID - 20f, 0f), Quaternion.identity) as GameObject;
            newupgrade.name = Upgrade.upgradeName;

            /* 다른 children이 추가되면 아래 코드에서 에러가 발생할 수도? */
            newupgrade.GetComponentInChildren<Image>().sprite = Sprite.Create(Upgrade.upgradeImage, new Rect(0, 0, imageIconSize, imageIconSize), new Vector2(0f, 0f));
            newupgrade.transform.SetParent(upgradeScrollPanel.transform, false);

            newupgrade.GetComponentInChildren<UpgradeBuyButton>().SetUpgrade(Upgrade);

            upgradeList.Add(newupgrade);

        }
        RefreshTooltip();
    }

    public void ShowUpgrades()
    {
        //이유는 모르겠지만 처음에 위치 조정을 안해주면 스크롤바랑 이미지 표시가 이상해짐
        upgradePanel.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
        upgradeScrollPanelrect.localPosition = new Vector2(upgradeScrollPanelrect.localPosition.x, -upgradeScrollPanelrect.rect.height / 2);
        upgradePanel.SetActive(true);
        RefreshTooltip();
    }

    void SetListSize(RectTransform rect)
    {
        rect.sizeDelta = new Vector2(rect.rect.width, database.upgradeDatabase.Count * 120f + 20f);
    }

    // 새로고침 할 수 있게 변경함
    public void RefreshTooltip()
    {
        foreach (GameObject go in upgradeList)
        {
            Upgrade Upgrade = database.Find(go.name);
            if (Upgrade != null)
            {
                string tooltip = "<color=#ffffff>" + Upgrade.upgradeName + "</color>\n\n";
                tooltip += "<color=#029919>" + Upgrade.upgradeDescription + "</color>\n\n";
                tooltip += "<color=#990282>" + "가격 : " + Upgrade.upgradePrice + "</color>\t\t";
                tooltip += "<color=#990282>" + "필요 개발자 수 : " + Upgrade.upgradeRequiredDev + "</color>\t\t";
                if (Upgrade.upgradeQuantity != 0)
                    tooltip += "<color=#990282>" + Upgrade.upgradeQuantityName + " : "+ Unlockables.GetInt(Upgrade.upgradeInternalName) + "</color>";
                go.GetComponentInChildren<Text>().text = tooltip;
            }
        }
    }
}
