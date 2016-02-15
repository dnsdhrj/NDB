using UnityEngine;
using System.Collections;

public class DeveloperMoveToButton : MonoBehaviour {

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

    // MoveFrom()에서 선택한 부서로 개발자를 옮겨주는 함수.
    public void MoveTo()
    {
        Post moveFrom = database.temp;
        Post destination = modifyingDeveloper;
        if (moveFrom == null || destination == null) return;
        Developer.dev.MoveDeveloper(moveFrom, destination);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
