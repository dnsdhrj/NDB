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

    // MoveFrom()에서 선택한 부서로 개발자를 옮겨주는 함수. 인자로 선택한 부서의 ID를 넘겨주세요. 
    public void MoveTo(int destPostID)
    {
        Post destination = Developer.dev.FindPostByPostID(destPostID);
        if (destination == null) return;
        Developer.dev.MoveDeveloper(modifyingDeveloper, destination);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
