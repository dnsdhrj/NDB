using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fame : MonoBehaviour {

    private Slider slider;
    public Image background;
    public Image fill;

    Color white = new Color(1f, 1f, 1f, 1f);
    Color red = new Color(1f, 0f, 0f, 1f);
    Color orange = new Color(1f, 0.5f, 0f, 1f);
    Color yellow = new Color(1f, 1f, 0f, 1f);
    Color green = new Color(0.5f, 1f, 0f, 1f);
    Color emerald = new Color(0f, 1f, 0.75f, 1f);
    Color skyblue = new Color(0f, 0.75f, 1f, 1f);
    Color blue = new Color(0f, 0.5f, 1f, 1f);
    Color violet = new Color(0.5f, 0f, 1f, 1f);
    Color purple = new Color(0.75f, 0f, 1f, 1f);
    Color pink = new Color(1f, 0f, 0.75f, 1f);
    int maxValue;


    void Start()
    {
        slider = GetComponent<Slider>();
        maxValue = (int)slider.maxValue;
    }

    void Update()
    {
        // 인기도가 슬라이더의 최댓값을 넘을 때마다 색이 바뀌면서 다시 차오름
        slider.value = GameManager.gm.Fame % maxValue;
        if (GameManager.gm.Fame / maxValue < 1)
        {
            background.color = white;
            fill.color = red;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 1)
        {
            background.color = red;
            fill.color = orange;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 2)
        {
            background.color = orange;
            fill.color = yellow;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 3)
        {
            background.color = yellow;
            fill.color = green;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 4)
        {
            background.color = green;
            fill.color = emerald;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 5)
        {
            background.color = emerald;
            fill.color = skyblue;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 6)
        {
            background.color = skyblue;
            fill.color = blue;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 7)
        {
            background.color = blue;
            fill.color = violet;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 8)
        {
            background.color = violet;
            fill.color = purple;
        }
        else if ((GameManager.gm.Fame / maxValue) % 10 == 9)
        {
            background.color = purple;
            fill.color = pink;
        }
        else
        {
            background.color = pink;
            fill.color = red;
        }
    }
}
