using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Events : MonoBehaviour {

    private GameManager gm; //많이 쓸것같아서 만들어둠

    public GameObject UnlockUpBasic_Box;
    public GameObject UserLimitExcess_Box;
    public GameObject RivalGameRelease_Box;
    public GameObject MacroEvent_Box;
    public GameObject TreeOfSavior_Box;
    public GameObject GettingFamous_Box;
    public GameObject DaramUpDownTutorial_Box;
    public GameObject SlimeParty_Box;
    public GameObject SlimeParty_Slime;

    public GameObject NormalMessage_Box;

    void Start ()
    {
        gm = GameManager.gm;

        gm.EventCheck += UnlockUpBasic;
        gm.EventCheck += UserLimitExcess;
        gm.EventCheck += RivalGameRelease;
        gm.EventCheck += MacroEvent;
        gm.EventCheck += TreeOfSavior;
        gm.EventCheck += GettingFamous;
        gm.RoundStartEvent += DaramUpDownTutorial;
        gm.RoundStartEvent += SlimeParty;
    }

    void UnlockUpBasic()
    {
        if (GameManager.gm.fame >= 15000)
        {
            GameManager.gm.EventCheck -= UnlockUpBasic;
 
            Instantiate(UnlockUpBasic_Box);
            LogText.WriteLog("인기에 힘입어 LV.2 다람쥐를 개발했다!");
            UserChat.CreateChat("GM: 고레벨 다람쥐가 새롭게 등장합니다!!", 5);
            
            gm.FameChange += gm.FameDaram2;
            gm.UserChange += gm.UserLevel2;
            gm.EventCheck += UserChat.uc.Daram2Number;
            Unlockables.SetBool("UnlockDaram2", true);
            
        }
    }

    void UserLimitExcess() {
        if (GameManager.gm.UserAllCount() > Unlockables.GetInt("UserLimit")) {

            Instantiate(UserLimitExcess_Box);
            LogText.WriteLog("서버가 충당 가능한 유저 수를 초과했습니다.");
            LogText.WriteLog("유저들이 접속 불량을 호소합니다. (유저 수와 인기도가 감소합니다.)");
            
            
            //유저채팅 추가
            gm.UserChat += UserChat.uc.UserLimitExcess;

            GameManager.gm.userCount[User.level1] -= (int)( GameManager.gm.userCount[User.level1] * Random.Range(0.3f, 0.5f));
            GameManager.gm.userCount[User.level2] -= (int)(GameManager.gm.userCount[User.level2] * Random.Range(0.3f, 0.5f));
            GameManager.gm.fame -= (int)(GameManager.gm.fame * (0.2 - 0.015 * Mathf.Min(10, Developer.dev.developerCount[Developer.dev.FindPostIDByName("Customer")])));

        }
    }

    void RivalGameRelease() {
        if (Random.value < 1f/14401f) {
            Instantiate(RivalGameRelease_Box);
            LogText.WriteLog("경쟁작 '전설의 어둠'이 베타 테스트를 시작했다!");
            LogText.WriteLog("(유저 수가 감소합니다.)");
            UserChat.CreateChat("전설의 어둠하러 갑시다.", 5);
            UserChat.CreateChat("ㄱㄱㄱ", 5);
            UserChat.CreateChat("이 게임 접으려는데 아이디 사실 분?", 5);

            GameManager.gm.userCount[User.level1] -= 1500 + (int)(GameManager.gm.userCount[User.level1] * 0.1f) - 150 * Mathf.Min(10, Developer.dev.developerCount[Developer.dev.FindPostIDByName("Publicity")]);
            GameManager.gm.userCount[User.level2] -= (int)(GameManager.gm.userCount[User.level2] * 0.1f);

            gm.EventCheck -= RivalGameRelease;
        }
    }

    void MacroEvent()
    {
        if (gm.UserAllCount() >= 20000 && Random.value < 1f/7201f)
            Instantiate(MacroEvent_Box);
    }

    void TreeOfSavior()
    {
        if (gm.time >= 200)
        {
            Instantiate(TreeOfSavior_Box);
            gm.EventCheck -= TreeOfSavior;
        }
    }

    void GettingFamous()
    {
        if (gm.fame >= 30000)
        {
            Instantiate(GettingFamous_Box);
            gm.userCount[User.level1] += 1000 + 100 * Developer.dev.developerCount[Developer.dev.FindPostIDByName("Publicity")];
            gm.userCount[User.level2] += 100;
            gm.EventCheck -= GettingFamous;
        }
    }

    void DaramUpDownTutorial()
    {
        if (Unlockables.GetBool("UnlockDaram1_Amount10") || Unlockables.GetBool("UnlockDaram2_Amount10"))
        {
            Instantiate(DaramUpDownTutorial_Box);
            gm.RoundStartEvent -= DaramUpDownTutorial;
        }
    }

    void SlimeParty()
    {
        if (gm.roundCount == 2)  // 일단은 2라운드때 무조건
        {
            // 다람쥐와 슬라임을 스왑함
            GameObject temp = GameObject.Find("AddBasicDaram").GetComponent<AddBasicDaram>().daram;
            GameObject.Find("AddBasicDaram").GetComponent<AddBasicDaram>().daram = SlimeParty_Slime;
            SlimeParty_Slime = temp;

            Instantiate(SlimeParty_Box);
        }
        if (gm.roundCount == 3)  // 3라운드에 해제
        {
            // 다람쥐와 슬라임을 스왑함
            GameObject temp = GameObject.Find("AddBasicDaram").GetComponent<AddBasicDaram>().daram;
            GameObject.Find("AddBasicDaram").GetComponent<AddBasicDaram>().daram = SlimeParty_Slime;
            SlimeParty_Slime = temp;

            gm.RoundStartEvent -= SlimeParty;
        }
            
    }
}
