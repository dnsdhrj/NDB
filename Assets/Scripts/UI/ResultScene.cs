using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour {

    private Text resultText;
    private int Events = 0; //실제로는 GameManager에 값이 있고 가져와야 할 듯
    public bool isEnabled = false;

	void Awake () {
        resultText = GetComponentInChildren<Text>();
	}

    void Update() {
        if (isEnabled)
        {
            print("Aa");
            StartCoroutine("ShowResult");
            isEnabled = false;
        }
    }

    IEnumerator ShowResult() {

        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        resultText.text = " ";
        yield return new WaitForSeconds(0.5f);
        resultText.text = "남은 돈 : " + GameManager.gm.Money;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n번 돈 :  " + GameManager.gm.EarnedMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n합계 : " + (GameManager.gm.EarnedMoney + GameManager.gm.Money);
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n이벤트";

        MoneyUpdate();

        for (int i = 0; i < Events; i++) {
            StageEndEvent();
            yield return new WaitForSeconds(0.5f);
        }


    }

    public void OnButtonClick()
    {
        GameManager.gm.Pause(true);
        GameManager.gm.CurrentStageScene = SceneManager.GetActiveScene().name;
        StopAllCoroutines();
        SceneManager.LoadScene("InterRound");
    }

    void StageEndEvent() {
            //뭔가 스테이지가 끝날 때 발생하는 이벤트
    }

    //스테이지가 끝날 때 GameManager에 결과 저장
    void MoneyUpdate()
    {
        GameManager.gm.Money = (GameManager.gm.Money + GameManager.gm.EarnedMoney);
        GameManager.gm.EarnedMoney = 0;
    }
}
