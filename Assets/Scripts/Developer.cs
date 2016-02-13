using UnityEngine;
using System.Collections;
using System.Linq;

public class Developer : MonoBehaviour {

    public static Developer dev;
    private GameManager gm;

    // UI 개편이 필요합니다. 일단 이 스크립트를 "InterRound 씬 -> Test 씬의 GameManager의 컴포넌트"로 옮겨놓았습니다.
    
    private int[] DeveloperSalary;

    [HideInInspector] public int hireCost = 1000;
    [HideInInspector] public int fireCost = -700;
    [HideInInspector] public int salaryCost = 0;

    // 개발자들이 속할 부서의 개수
    public const int PostCount = 8;

    // 부서명과 그 int값. gm.DeveloperCount[]의 array index로 사용하면 됩니다.
    public const int Server = 0;        // 서버 관리팀 (유저수 제한과 관련)
    public const int Debugging = 1;     // 디버깅 팀   (매크로, 버그 발생 이벤트와 관련)
    public const int Publicity = 2;     // 홍보팀      (신규 유저수 증감과 관련)
    public const int Customer = 3;      // 고객 지원팀 (인기도 하락 이벤트와 관련)
    public const int DaramLv1 = 4;      // 레벨1 다람쥐 개발팀
    public const int DaramLv2 = 5;      // 레벨2 다람쥐 개발팀
    public const int Rabbit = 6;        // 토끼 개발팀   (미구현)
    public const int Slime = 7;         // 슬라임 개발팀 (미구현)

    

	void Start () {
        dev = this;
        gm = GameManager.gm;

        DeveloperSalary = Enumerable.Repeat(0, Developer.PostCount).ToArray();

        // 각 부서별 월급 가중치
        DeveloperSalary[Server] = 300;
        DeveloperSalary[Debugging] = 250;
        DeveloperSalary[Publicity] = 220;
        DeveloperSalary[Customer] = 200;
        DeveloperSalary[DaramLv1] = 180;   // 개발자를 고용하면 다람쥐를 직접 뿌리는 것보다 10% 저렴합니다.
        DeveloperSalary[DaramLv2] = 360;
        DeveloperSalary[Rabbit] = 500;     // 토끼랑 슬라임이 아직 얼마일지 몰라 아무 값이나 넣었습니다.
        DeveloperSalary[Slime] = 600;

        CalculateCost();
        ExpandUserLimit();

	}

    void ExpandUserLimit()
    {
        // 서버 관리팀의 개발자 한 명당 유저수 제한을 2500명씩 늘려줍니다.
        gm.UserLimit = gm.BasicUserLimit + 2500 * gm.DeveloperCount[Server]; 
    }

    public void CalculateCost()
    {
        //누군가 적절한 함수를 생각해주길
        hireCost = 1000;
        fireCost = -700; // 해고하면 돈 700을 받습니다.

        salaryCost = 0;
        for (int i = 0; i < PostCount; i++)
        {
            salaryCost += gm.DeveloperCount[i] * DeveloperSalary[i];
        }
    }

    public int DeveloperAllCount()
    {
        int sum = 0;
        foreach (int dev in gm.DeveloperCount)
        {
            sum += dev;
        }
        return sum;
    }

    // 개발자 고용하는 함수. 인자로 부서명을 넘겨주면 해당 부서에 개발자가 1명 투입됩니다.
    // InterRound에서는 Developers 오브젝트에 달려있는 InterRoundDeveloper 컴포넌트의 Hire(post)로 사용 바랍니다.
    public void HireDeveloper(int post) {
        //if (post == Debugging && gm.DeveloperCount[Debugging] >= 10) return; // 나중에 개발자 인원 제한이 필요할 수 있으므로
        if (gm.Money >= hireCost) {
            gm.Money -= hireCost;
            gm.DeveloperCount[post]++;
            CalculateCost();
            if (post == Server) ExpandUserLimit();
        }
    }

    // 개발자 해고하는 함수. 인자로 부서명을 넘겨주면 해당 부서에서 개발자가 1명 빠집니다.
    // InterRound에서는 Developers 오브젝트에 달려있는 InterRoundDeveloper 컴포넌트의 Fire(post)로 사용 바랍니다.
    public void FireDeveloper(int post) {
        if (gm.Money >= fireCost && gm.DeveloperCount[post] > 0) {
            gm.Money -= fireCost;
            gm.DeveloperCount[post]--;
            CalculateCost();
            if (post == Server) ExpandUserLimit();
        }
    }

    // 개발자의 부서를 이전하는 함수. from은 원래 있던 부서명, to는 새로 이동할 부서명입니다.
    // InterRound에서는 Developers 오브젝트에 달려있는 InterRoundDeveloper 컴포넌트의 Move(from, to)로 사용 바랍니다.
    public void MoveDeveloper(int from, int to)
    {
        //if (to == Debugging && gm.DeveloperCount[Debugging] >= 10) return; // 나중에 개발자 인원 제한이 필요할 수 있으므로
        gm.DeveloperCount[from]--;
        gm.DeveloperCount[to]++;
        CalculateCost();
        if (from == Server || to == Server) ExpandUserLimit();
    }
}
