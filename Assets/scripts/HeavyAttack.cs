using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyAttack : MonoBehaviour
{
    public Slider slider;

    public void setEnergyBar(float Energy)
    {
        slider.maxValue = Energy;
    }

    public void setEnergy(float Energy)
    {
        slider.value = Energy;

    }
}
