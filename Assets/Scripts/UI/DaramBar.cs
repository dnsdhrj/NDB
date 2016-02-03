using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DaramBar : MonoBehaviour {

    public GameObject PositiveArea;
    public GameObject Arrow;
    public int UserLevel;   // User 클래스 참조
    public float MaxDiff;     // 게이지 Top에서 Bottom까지의 다람쥐 수

    private const int BarHeight = 140;
    private Slider slider;
    private RectTransform rect;
    private RectTransform arrowrect;
    private Quadric DaramFunc;
    private string UnlockKey;

    void Start()
    {
        slider = GetComponent<Slider>();
        rect = PositiveArea.GetComponent<RectTransform>();
        arrowrect = Arrow.GetComponent<RectTransform>();
        DaramFunc = GameManager.gm.DaramFunction[UserLevel];

        switch (UserLevel)
        {
            case User.level1:
                UnlockKey = "UnlockDaram1";
                break;
            case User.level2:
                UnlockKey = "UnlockDaram2";
                break;
        }

        // NaN exception이 뜨지 않게 대충 초기화
        DaramFunc.k = 1;
        DaramFunc.x = 0;
        DaramFunc.a = 0;
        DaramFunc.max = 1;
        DaramFunc.min = -1;
    }


    void Update()
    {
        if (Unlockables.GetBool(UnlockKey) == false)
            transform.localScale = Vector3.zero;
        else
            transform.localScale = new Vector3(1, 1, 1);

        Vector3 v = rect.localScale;
        v.y = DaramFunc.solution / MaxDiff;
        rect.localScale = v;

        if (DaramFunc.diff >= 0) // 다람쥐 수가 적정 수준 이상이면 화살표가 아래를 향함
        {
            slider.value = DaramFunc.solution / MaxDiff + 0.1f * Mathf.Sin(3.0f * GameManager.gm.time);
            arrowrect.localScale = new Vector3(1, -1, 1);
        }
        else // 다람쥐 수가 적정 수준보다 적으면 화살표가 위를 향함
        {
            slider.value = (-1) * DaramFunc.solution / MaxDiff - 0.1f * Mathf.Sin(3.0f * GameManager.gm.time);
            arrowrect.localScale = new Vector3(1, 1, 1);
        }
    }
}
