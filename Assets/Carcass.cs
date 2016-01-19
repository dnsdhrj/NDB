using UnityEngine;
using System.Collections;

public class Carcass : MonoBehaviour {

    public int Duration;
    private int hp;

	// Use this for initialization
	void Start () {
        hp = Duration;
	}
	
    void Die()
    {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void FixedUpdate () {
        hp--;
        if (hp < 0) Die();
	
	}
}
