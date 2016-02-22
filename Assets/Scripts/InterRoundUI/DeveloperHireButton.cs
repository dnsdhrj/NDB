using UnityEngine;
using System.Collections;

public class DeveloperHireButton : MonoBehaviour {

    private Post modifyingDeveloper;

    public void SetPost(Post post)
    {
        modifyingDeveloper = post;
    }

    public void Hire()
    {
        Developer.dev.HireDeveloper(modifyingDeveloper);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
