using UnityEngine;
using System.Collections;

public class Basic : Daram {

    private int past_move_stat = -1; // 현재 움직임 상태
    private int now_move_stat; //이전 움직임 상태

    

    void Start()
    {
        Anim = GetComponent<Animator>();
        Type = "Basic";
        base.Start();
    }

    void FixedUpdate()
    {
        now_move_stat = base.move_stat;
        if (now_move_stat != past_move_stat)
        {
            switch (now_move_stat)
            {
                case 0:
                    Anim.SetTrigger("Up");
                    Anim.SetTrigger("UL");
                    break;
                case 1:
                    Anim.SetTrigger("Down");
                    Anim.SetTrigger("DR");
                    break;
                case 2:
                    Anim.SetTrigger("Left");
                    Anim.SetTrigger("UL");
                    break;
                case 3:
                    Anim.SetTrigger("Right");
                    Anim.SetTrigger("DR");
                    break;
            }
            past_move_stat = now_move_stat;
        }
    }


}
