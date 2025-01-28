using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void setmaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void sethealth(float health)
    {
        slider.value = health;

    }
}
