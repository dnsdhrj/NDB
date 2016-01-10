using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 다람쥐 기본 클래스이며 세부 항목은 상속시켜서 사용
/// </summary>
public class Daram : MonoBehaviour {

    public static List<Daram> All = new List<Daram>();
    
    public int Level;
    public int InitialHP;

    public int HP {
        get { return _HP; }
        set { _HP = value; if (value <= 0) Die(); }
    }
    
    private int _HP = 0;

    // *중요*
    // Daram 하위 클래스의 Start()에 "base.Start();" 를 넣어주세요.
    // 생성자 쓰면 버그 생겨서 Start()로 써야함.
    protected void Start()
    {
        HP = InitialHP;

        All.Add(this);
    }

    public void Die()
    {
        All.Remove(this);
        Destroy(gameObject);
    }

    //All.Remove()를 쓰기 위해 비교연산자가 필요함.
    public override bool Equals(object obj)
    {
        if (obj == null)
            return true;
        else if (((Daram)obj).GetInstanceID() == GetInstanceID())
            return true;
        else
            return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
