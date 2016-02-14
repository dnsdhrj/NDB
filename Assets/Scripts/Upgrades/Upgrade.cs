using UnityEngine;
using System.Collections;

[System.Serializable]
public class Upgrade
{

    public string upgradeName;
    public int upgradeID;
    public int upgradePrice;
    public string upgradeDescription;
    public Texture2D upgradeImage;
    public int upgradeRequiredDev;
    public string upgradeInternalName;  // unlockable에 추가될 이름
    public int upgradeQuantity;         // 이걸 0으로 하면 Unlockable을 bool로 인식하게됨
    public string upgradeQuantityName;  // 현재 업그레이드 값을 표시할 때 그 값의 이름

    //이미지는 업그레이드이름(upgradeName)과 똑같은 파일명을 가진 걸 자동으로 사용하도록 했음
    public Upgrade(string name, int ID, int price, int dev, string desc, string unlockable, string quantityname = "", int quantity = 0)
    {
        upgradeName = name;
        upgradeID = ID;
        upgradePrice = price;
        upgradeDescription = desc;
        upgradeImage = Resources.Load<Texture2D>("Upgrade Icons/" + name);
        upgradeRequiredDev = dev;
        upgradeInternalName = unlockable;
        upgradeQuantityName = quantityname;
        upgradeQuantity = quantity;
    }

    public Upgrade()
    {

    }
}
