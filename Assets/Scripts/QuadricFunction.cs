using UnityEngine;
using System.Collections;

// Quadric == 2차함수라고 합니다
// 함수에 대한 정보가 필요한 경우가 있어서 따로 만들어둠


/// <summary>
/// y = -k(x - a)^2 + max   (y >= min)
/// </summary>
public class Quadric {  

    public float k;     //계수
    public float x;     //실제 다람쥐 수
    public float a;     //적정 다람쥐 수
    public float max;   //인기도 최대 증가량
    public float min;   //인기도 최소 감소량

    /// <summary>
    /// 2차함수의 계산값
    /// </summary>
    public float value
    {
        get
        {
            return Mathf.Max((-1) * k * (x - a) * (x - a) + max, min);
        }
    }

    /// <summary>
    /// (실제 - 적정) 다람쥐 수
    /// </summary>
    public float diff
    {
        get
        {
            return x - a;
        }
    }

    /// <summary>
    /// 함수의 두 근의 차
    /// </summary>
    public float solution
    {
        get
        {
            return 2.0f * Mathf.Sqrt(max / k);
        }
    }
}


public class QuadricFunction : MonoBehaviour { }    // 이거 없으면 스크립트 부착이 안됨