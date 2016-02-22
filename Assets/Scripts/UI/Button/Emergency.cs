using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Emergency : MonoBehaviour {

    private GameManager gm;
    void Start()
    {
        gm = GameManager.gm;
    }

    public void OnClick()
    {
        gm.Pause(true);
        gm.isEmergency = true;
        gm.fame = Mathf.Max(gm.fame - 10000, 0);
        gm.earnedMoneyModifier *= 0.7f;

        gm.currentStageScene = SceneManager.GetActiveScene().name;
        StopAllCoroutines();
        SceneManager.LoadScene("InterRound");
    }

    void Update()
    {
        if (Unlockables.GetBool("Emergency") == true)
            transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        else
            transform.localScale = Vector3.zero;
    }
}
