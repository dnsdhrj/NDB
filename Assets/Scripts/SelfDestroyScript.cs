using UnityEngine;
using System.Collections;

public class SelfDestroyScript : MonoBehaviour {

    public float LifeTime;

    private float DoomsDay;

    void Start()
    {
        DoomsDay = Time.time + LifeTime;
    }

    public void ChangeFate(float Lifetime)
    {
        LifeTime = Lifetime;
        DoomsDay = Time.time + LifeTime;
    }

    void Update()
    {
        if (Time.time >= DoomsDay)
            Destroy(gameObject);
    }
}
