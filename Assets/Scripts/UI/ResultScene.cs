using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ResultScene : EventBox {

    private Text resultText;
    private int Events = 0; //실제로는 GameManager에 값이 있고 가져와야 할 듯

	void Awake () {
        resultText = GetComponentInChildren<Text>();
	}

    void Start()
    {
        base.Start();   // 생성된 창 위치 맞추고 일시정지
        Developer.dev.CalculateCost();
        StartCoroutine("ShowResult");
    }

    IEnumerator ShowResult() {
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        
        resultText.text = " ";
        yield return new WaitForSeconds(0.5f);
        resultText.text = "라운드 시작 시 돈 : " + GameManager.gm.initialMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n이번 라운드에 번 돈 :  " + GameManager.gm.earnedMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n이번 라운드에 쓴 돈 :  " + GameManager.gm.usedMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n지급한 개발자 월급 : " + GameManager.gm.salaryMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n합계 : " + GameManager.gm.Money();
        yield return new WaitForSeconds(0.5f);
        if (GameManager.gm.roundEventName == "") GameManager.gm.roundEventName = "없음";
        resultText.text = resultText.text + "\n이번 라운드에 적용된 행사 < " + GameManager.gm.roundEventName + " >";

        GameManager.gm.InitiateMoney();

        for (int i = 0; i < Events; i++) {
            StageEndEvent();
            yield return new WaitForSeconds(0.5f);
        }


    }

    public void OnButtonClick()
    {
        GameManager.gm.Pause(true);
        GameManager.gm.currentStageScene = SceneManager.GetActiveScene().name;
        GameManager.gm.roundEventName = "";
        StopAllCoroutines();
        SceneManager.LoadScene("InterRound");
    }

    void StageEndEvent() {
            //뭔가 스테이지가 끝날 때 발생하는 이벤트
    }

}
