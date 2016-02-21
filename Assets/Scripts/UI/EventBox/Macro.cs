using UnityEngine;
using System.Collections;

public class Macro : MonoBehaviour {

    public void KillMacro()
    {
        if (GameManager.gm.Money() >= 3000)
        {
            GameManager.gm.ChangeMoneyInRound(-3000);
            GameManager.gm.fame += 1000;
            GetComponentInParent<EventBox>().OnClick();
            LogText.WriteLog("GM을 시켜 열심히 매크로를 잡았다.");
        }
        else
            LogText.WriteLog("돈이 부족합니다.");
    }

    public void KeepMacro()
    {
        GameManager.gm.fame -= 1000 - 100 * Mathf.Min(10, Developer.dev.developerCount[Developer.dev.FindPostIDByName("Customer")]);
        // 디버깅 팀의 개발자 한 명당 매크로 지속시간이 10초씩 줄어듭니다.
        ActivityEnd = GameManager.gm.time + 100f - 10f * (float)Mathf.Min(10, Developer.dev.developerCount[Developer.dev.FindPostIDByName("Debugging")]);
        GameManager.gm.DaramDeath += MacroActivity;
        LogText.WriteLog("매크로가 게임에 판을 치고 있다. 개발자들이 매크로를 잡을 때까지 기다리자.");
    }

    private float ActivityEnd;
    private float NextActivity = 0;
    void MacroActivity()
    {
        if (GameManager.gm.time >= ActivityEnd)
            GameManager.gm.DaramDeath -= MacroActivity;
            LogText.WriteLog("개발자들이 매크로를 잡았다.");

        if (GameManager.gm.time >= NextActivity)
        {
            Daram.All[Random.Range(0, Daram.All.Count)].HP -= 1000;
            NextActivity = GameManager.gm.time + 10.0f / (float)Daram.All.Count;    // 다람쥐가 초당 10% 감소
        }
    }
}
