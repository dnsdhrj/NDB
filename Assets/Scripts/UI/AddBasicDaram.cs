using UnityEngine;
using System.Collections;

public class AddBasicDaram : MonoBehaviour {

    public GameObject daram;

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
}
