using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider slider;
    public TimeManager timeManager;
    public float amountOfSand;

    void Update()
    {
        amountOfSand = timeManager.currentSand;
        slider.value = amountOfSand;
    }
}
