using UnityEngine;
using System.Collections;

public class Macro : MonoBehaviour {

    public void KillMacro()
    {
        if (GameManager.gm.Money >= 3000)
        {
            GameManager.gm.Money -= 3000;
            GameManager.gm.Fame += 1000;
            GetComponentInParent<EventBox>().OnClick();
            LogText.WriteLog("GM을 시켜 열심히 매크로를 잡았다.");
        }
        else
            LogText.WriteLog("돈이 부족합니다.");
    }

    public void KeepMacro()
    {
        GameManager.gm.Fame -= 1000;
        ActivityEnd = GameManager.gm.time + 100;
        GameManager.gm.DaramDeath += MacroActivity;
        LogText.WriteLog("매크로가 게임에 판을 치고 있다.");
    }

    private float ActivityEnd;
    private float NextActivity = 0;
    void MacroActivity()
    {
        if (GameManager.gm.time >= ActivityEnd)
            GameManager.gm.DaramDeath -= MacroActivity;

        if (GameManager.gm.time >= NextActivity)
        {
            Daram.All[Random.Range(0, Daram.All.Count)].HP -= 1000;
            NextActivity = GameManager.gm.time + 10.0f / (float)Daram.All.Count;    // 다람쥐가 초당 10% 감소
        }
    }
}
