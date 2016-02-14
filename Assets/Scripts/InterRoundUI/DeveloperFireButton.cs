using UnityEngine;
using System.Collections;

public class DeveloperFireButton : MonoBehaviour {

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

    public void Fire()
    {
        Developer.dev.FireDeveloper(modifyingDeveloper);
        DeveloperCheckup.devChkup.RefreshPostTooltip();
    }
}
