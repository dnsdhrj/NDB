using UnityEngine;
using System.Collections;

public class DeveloperHireButton : MonoBehaviour {

    private Post modifyingDeveloper;
    private DeveloperCheckup devChkup;

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
