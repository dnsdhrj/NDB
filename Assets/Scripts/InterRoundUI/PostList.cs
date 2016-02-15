using UnityEngine;
using System.Collections;

public class PostList : MonoBehaviour {

    public GameObject hireButton;
    public GameObject moveFromButton;
    public GameObject fireButton;
    public GameObject moveToButton;

    // 부서 이동 버튼을 눌렀을 때
    public void ActiveMoveTo()
    {
        hireButton.SetActive(false);
        moveFromButton.SetActive(false);
        fireButton.SetActive(false);
        moveToButton.SetActive(true);
    }

    // 기본 상태(여기로 이동 또는 취소 버튼을 눌렀을 때)
    public void ActiveMoveFrom()
    {
        hireButton.SetActive(true);
        moveFromButton.SetActive(true);
        fireButton.SetActive(true);
        moveToButton.SetActive(false);
    }
}
