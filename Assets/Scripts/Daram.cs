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
    public int Speed;

    public int HP {
        get { return _HP; }
        set { _HP = value; if (value <= 0) Die(); }
    }
    
    private int _HP = 0;
    private Vector2 dir;

    // *중요*
    // Daram 하위 클래스의 Start()에 "base.Start();" 를 넣어주세요.
    // 생성자 쓰면 버그 생겨서 Start()로 써야함.
    protected void Start()
    {
        HP = InitialHP;

        All.Add(this);
    }

    // *중요*
    // Update()도 위와 동일
    protected void Update()
    {
        Move();
    }

    public void Die()
    {
        All.Remove(this);
        Destroy(gameObject);
    }

    private float NextMove = 0;
    void Move()
    {
        if (NextMove <= Time.time)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    dir = Vector2.up;
                    break;
                case 1:
                    dir = Vector2.down;
                    break;
                case 2:
                    dir = Vector2.left;
                    break;
                case 3:
                    dir = Vector2.right;
                    break;
            }

            NextMove = Time.time + Random.Range(1.0f, 3.0f);    //방향전환 시간
        }

        transform.position += (Vector3) dir * (Speed * 0.001f);
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
