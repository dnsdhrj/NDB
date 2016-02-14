using UnityEngine;
using System.Collections;

public class Stage1 : MonoBehaviour {

    // 각 스테이지의 초기조건을 지정해주는 스크립트
    // 기존에 흩어져 있던 것들을 여기로 옮길 예정

    void Start()
    {
        Unlockables.SetBool("UnlockDaram1", true);

        Unlockables.SetInt("UserLimit", 5000);
    }
}
