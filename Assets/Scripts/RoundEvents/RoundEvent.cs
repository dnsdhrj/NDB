using UnityEngine;
using System.Collections;

[System.Serializable]
public class RoundEvent{

    public string eventName;
    public string eventFuncName;
    public int eventID;
    public int eventPrice;
    public int eventRequiredDev;
    public string eventDescription;
    public Texture2D eventImage;
    public delegate IEnumerator VoidCoroutine();
    public VoidCoroutine eventStart;

    //이미지는 이벤트이름(eventName)과 똑같은 파일명을 가진 걸 자동으로 사용하도록 했음
    public RoundEvent(string name, string funcname, int ID, int price, int dev, string desc, VoidCoroutine onStart)
    {
        eventName = name;
        eventFuncName = funcname;
        eventID = ID;
        eventPrice = price;
        eventRequiredDev = dev;
        eventDescription = desc;
        eventImage = Resources.Load<Texture2D>("Event Icons/" + name);
        eventStart = onStart;
    }

    public RoundEvent()
    {

    }
}
