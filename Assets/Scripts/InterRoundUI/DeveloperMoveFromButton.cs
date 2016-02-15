using UnityEngine;
using System.Collections;

public class DeveloperMoveFromButton : MonoBehaviour {

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

    public void MoveFrom()
    {
        if (modifyingDeveloper.DeveloperInPost() > 0)
        {
            dev.temp = modifyingDeveloper;
            devChkup.RefreshPostButtons1();
        }
    }
}
