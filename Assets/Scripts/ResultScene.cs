using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour {

    private Text resultText;
    private int Events = 0; //실제로는 GameManager에 값이 있고 가져와야 할 듯

	void Awake () {
        resultText = GetComponentInChildren<Text>();
	}

    void OnEnable() {
        StartCoroutine("ShowResult");
    }

    IEnumerator ShowResult() {

        GetComponent<Image>().enabled = true;

        resultText.text = " ";
        yield return new WaitForSeconds(0.5f);
        resultText.text = "남은 돈 : " + GameManager.gm.Money;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n번 돈 :  " + GameManager.gm.EarnedMoney;
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n합계 : " + (GameManager.gm.EarnedMoney + GameManager.gm.Money);
        yield return new WaitForSeconds(0.5f);
        resultText.text = resultText.text + "\n이벤트";

        for (int i = 0; i < Events; i++) {
            StageEndEvent();
            yield return new WaitForSeconds(0.5f);
        }


    }

    void StageEndEvent() {
            //뭔가 스테이지가 끝날 때 발생하는 이벤트
    }
}
