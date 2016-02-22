using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeDatabase : MonoBehaviour {

    public List<Upgrade> upgradeDatabase = new List<Upgrade>();

    private GameManager gm;

    //업그레이드 데이터베이스는 여기에 추가
    void Start()
    {
        gm = GameManager.gm;

        upgradeDatabase.Add(new Upgrade("서버 증설", 0, 2500, 1, "이 설명은 예외처리함", "Server", "UserLimit", "현재 최대 유저 수", 1, 2.0f));
    }


    //itemDatabase와는 달리 다른 script(e.g. inventory)가 없어 이곳에 함수를 넣음 나중에 다른 적절한 script가 생기면 옴기자.
    public void AddUpgrade(Upgrade upgrade)
    {
        if (gm.Money() < upgrade.upgradePrice)
        {
            //나중에 창으로 나오게 고치자
            Debug.Log("돈이 부족합니다.");
            return;
        }

        else
        {
            gm.ChangeMoneyInterRound(-upgrade.upgradePrice);
            if (upgrade.upgradeQuantity == 0)
                Unlockables.SetBool(upgrade.upgradeInternalName, !Unlockables.GetBool(upgrade.upgradeInternalName));   // bool 값을 반전시킴
            else
                Unlockables.SetInt(upgrade.upgradeInternalName, Unlockables.GetInt(upgrade.upgradeInternalName) + upgrade.upgradeQuantity);
            upgrade.upgradePrice = (int) (upgrade.upgradePrice * upgrade.upgradeModifier);
        }
    }

    public Upgrade Find(string name)
    {
        foreach (Upgrade up in upgradeDatabase)
            if (up.upgradeName == name)
                return up;
        return null;
    }

    void Update()
    {
        // 딱히 어디 두어야 할지 모르겠음
        Unlockables.SetInt("UserLimit", Unlockables.GetInt("Server") * Unlockables.GetInt("ServerEff"));
    }
}
