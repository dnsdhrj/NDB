using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddBasicDaram : MonoBehaviour {

    public GameObject daram;

    void Start()
    {
        if (daram.GetComponent<Daram>().Level == 2) // 한번만 실행되게 하기 위함
            GameManager.gm.EventCheck += UnlockUpBasic;
    }

    private Vector2 RandomPosition()
    {
        Vector2 randompos;
        randompos.x = Random.Range(GameManager.gm.FieldCenterX - GameManager.gm.FieldWidth / 2f, GameManager.gm.FieldCenterX + GameManager.gm.FieldWidth / 2f);
        randompos.y = Random.Range(GameManager.gm.FieldCenterY - GameManager.gm.FieldHeight / 2f, GameManager.gm.FieldCenterY + GameManager.gm.FieldHeight / 2f);
        return randompos;
    }

    public void OnClick()
    {
        Vector2 pos = RandomPosition();
        Instantiate(daram, pos, Quaternion.identity);
    }

    void UnlockUpBasic()
    {
        if (GameManager.gm.Fame >= 5000)
        {
            LogText.WriteLog("인기에 힘입어 LV.2 다람쥐를 개발했다!");
            GetComponent<Button>().interactable = true;

            GameManager.gm.EventCheck -= UnlockUpBasic;
        }
    }
}
