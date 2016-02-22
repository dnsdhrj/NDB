using UnityEngine;
using System.Collections;

public class DeveloperMoveToButton : MonoBehaviour {

    private Post modifyingDeveloper;
    private Developer dev;
    private DeveloperCheckup devChkup;

    void Start()
    {
        devChkup = DeveloperCheckup.devChkup;
        dev = Developer.dev;
    }

    public void SetPost(Post post)
    {
        modifyingDeveloper = post;
    }

    // MoveFrom()에서 선택한 부서로 개발자를 옮겨주는 함수.
    public void MoveTo()
    {
        if (dev.temp == null || modifyingDeveloper == null) return;
        dev.MoveDeveloper(dev.temp, modifyingDeveloper);
        devChkup.RefreshPostTooltip();
        devChkup.RefreshPostButtons2();
    }
}
