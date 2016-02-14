using UnityEngine;
using System.Collections;

public class DeveloperHireButton : MonoBehaviour {

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

    public void Hire()
    {
        Developer.dev.HireDeveloper(modifyingDeveloper);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
