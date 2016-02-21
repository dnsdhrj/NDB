using UnityEngine;
using System.Collections;

public class Slime : Daram
{

    private int past_move_stat = 0; // 현재 움직임 상태
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
        if ((now_move_stat%2) != (past_move_stat%2))
        {
            switch (now_move_stat%2)
            {
                case 0:
                    Anim.SetTrigger("UL");
                    break;
                case 1:
                    Anim.SetTrigger("DR");
                    break;
            }
            past_move_stat = now_move_stat;
        }
    }
}
