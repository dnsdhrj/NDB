using UnityEngine;
using System.Collections;

public class DeveloperMoveFromButton : MonoBehaviour {

    private Post modifyingDeveloper;
    private Developer database;
    private DeveloperCheckup devChkup;

    void Start()
    {
        database = GameManager.gm.GetComponent<Developer>();
    }

    public void SetPost(Post post)
    {
        modifyingDeveloper = post;
    }

    public void MoveFrom()
    {
        // 현재 부서의 개발자 1명을 다른 어느 부서로 옮길 것인지 선택하는 UI를 띄워주는 함수
    }
}
