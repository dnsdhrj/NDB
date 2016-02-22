using UnityEngine;
using System.Collections;

public class DeveloperFireButton : MonoBehaviour {

    private Post modifyingDeveloper;

    public void SetPost(Post post)
    {
        modifyingDeveloper = post;
    }

    public void Fire()
    {
        Developer.dev.FireDeveloper(modifyingDeveloper);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
