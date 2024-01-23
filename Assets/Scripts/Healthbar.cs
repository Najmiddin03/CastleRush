using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public Image fill; // Reference to the fill image of the slider

    public void SetHealth(float health, float maxHealth)
    {
        // Set the value of the slider based on health percentage
        slider.value = health / maxHealth;

        // Change color based on health percentage
        Color healthColor = Color.Lerp(Color.red, Color.green, slider.value);
        fill.color = healthColor;
    }
}
