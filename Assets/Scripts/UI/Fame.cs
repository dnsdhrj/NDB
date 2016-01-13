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
    Color lightGreen = new Color(0.5f, 1f, 0f, 1f);
    Color green = new Color(0f, 1f, 0f, 1f);
    Color emelald = new Color(0f, 1f, 0.5f, 1f);
    Color cyan = new Color(0f, 1f, 1f, 1f);
    Color skyBlue = new Color(0f, 0.5f, 1f, 1f);
    Color blue = new Color(0f, 0f, 1f, 1f);
    Color violet = new Color(0.5f, 0f, 1f, 1f);
    Color magenta = new Color(1f, 0f, 1f, 1f);
    Color pink = new Color(1f, 0f, 0.5f, 1f);
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
        else if ((GameManager.gm.Fame / maxValue) % 12 == 1)
        {
            background.color = red;
            fill.color = orange;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 2)
        {
            background.color = orange;
            fill.color = yellow;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 3)
        {
            background.color = yellow;
            fill.color = lightGreen;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 4)
        {
            background.color = lightGreen;
            fill.color = green;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 5)
        {
            background.color = green;
            fill.color = emelald;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 6)
        {
            background.color = emelald;
            fill.color = cyan;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 7)
        {
            background.color = cyan;
            fill.color = skyBlue;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 8)
        {
            background.color = skyBlue;
            fill.color = blue;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 9)
        {
            background.color = blue;
            fill.color = violet;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 10)
        {
            background.color = violet;
            fill.color = magenta;
        }
        else if ((GameManager.gm.Fame / maxValue) % 12 == 11)
        {
            background.color = magenta;
            fill.color = pink;
        }
        else
        {
            background.color = pink;
            fill.color = red;
        }
    }
}
