using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 예시
// 기본 다람쥐 해금 : Unlockables.SetBool("UnlockDaram1", true);
// 기본 다람쥐 해금 확인 : Unlockables.GetBool("UnlockDaram1");


/// <summary>
/// 다람쥐 해금이나 업그레이드 여부를 기록하는 클래스
/// </summary>
public static class Unlockables
{
    private static Dictionary<string, bool> bools = new Dictionary<string, bool>();
    private static Dictionary<string, int> ints = new Dictionary<string, int>();

    public static void SetBool(string name, bool value)
    {
        bool b;
        if (bools.TryGetValue(name, out b) == true)
            bools[name] = value;
        else
            bools.Add(name, value);
    }
    public static void SetInt(string name, int value)
    {
        int n;
        if (ints.TryGetValue(name, out n) == true)
            ints[name] = value;
        else
            ints.Add(name, value);
    }

    /// <summary>
    /// 값이 없으면 false 리턴
    /// </summary>
    public static bool GetBool(string name)
    {
        bool b;
        if (bools.TryGetValue(name, out b) == true)
            return b;
        else
            return false;
    }

    /// <summary>
    /// 값이 없으면 0 리턴
    /// </summary>
    public static int GetInt(string name)
    {
        int n;
        if (ints.TryGetValue(name, out n) == true)
            return n;
        else
            return 0;
    }


}

public class Unlockables_ : MonoBehaviour { }
