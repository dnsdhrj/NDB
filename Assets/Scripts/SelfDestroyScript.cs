using UnityEngine;
using System.Collections;

/// <summary>
/// 일시정지되면 시간이 같이 멈춥니다
/// </summary>
public class SelfDestroyScript : MonoBehaviour {

    public float LifeTime;

    private float DoomsDay;

    void Start()
    {
        DoomsDay = GameManager.gm.time + LifeTime;
    }

    public void ChangeFate(float Lifetime)
    {
        LifeTime = Lifetime;
        DoomsDay = GameManager.gm.time + LifeTime;
    }

    void Update()
    {
        if (GameManager.gm.time >= DoomsDay)
            Destroy(gameObject);
    }
}
