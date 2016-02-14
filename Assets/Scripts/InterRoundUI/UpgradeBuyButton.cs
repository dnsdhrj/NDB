using UnityEngine;
using System.Collections;

public class UpgradeBuyButton : MonoBehaviour
{
    private Upgrade buyingUpgrade;
    private UpgradeDatabase database;

    void Start()
    {
        database = GameManager.gm.GetComponentInChildren<UpgradeDatabase>();
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        buyingUpgrade = upgrade;
    }

    public void OnClick()
    {
        database.AddUpgrade(buyingUpgrade);
        UpgradeCheckup._this.RefreshTooltip();
    }
}
