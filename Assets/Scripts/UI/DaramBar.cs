using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DaramBar : MonoBehaviour {

    public GameObject PositiveArea;
    public int UserLevel;   // User 클래스 참조
    public float MaxDiff;     // 게이지 Top에서 Bottom까지의 다람쥐 수

    private const int BarHeight = 140;
    private Slider slider;
    private RectTransform rect;
    private Quadric DaramFunc;

    void Start()
    {
        slider = GetComponent<Slider>();
        rect = PositiveArea.GetComponent<RectTransform>();
        DaramFunc = GameManager.gm.DaramFunction[UserLevel];

        // NaN exception이 뜨지 않게 대충 초기화
        DaramFunc.k = 1;
        DaramFunc.x = 0;
        DaramFunc.a = 0;
        DaramFunc.max = 1;
        DaramFunc.min = -1;
    }
    

    void Update()
    {
        Vector3 v = rect.localScale;
        v.y = DaramFunc.solution / MaxDiff;
        rect.localScale = v;

        slider.value = 2.0f * DaramFunc.diff / MaxDiff;
    }
}
