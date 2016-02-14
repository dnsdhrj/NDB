using UnityEngine;
using System.Collections;

public class Carcass : MonoBehaviour {

    public int Duration;
    private int hp;
    SpriteRenderer fade;

	// Use this for initialization
	void Start () {
        hp = Duration;
        fade = GetComponent("SpriteRenderer") as SpriteRenderer;
	}
	
    void Die()
    {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (GameManager.gm.isPaused) return;
        hp--;
        fade.color = Color.Lerp(fade.color, Color.clear, (1f / (float)Duration) * 200f * Time.deltaTime);
        if (hp < 0) Die();
	
	}
}
