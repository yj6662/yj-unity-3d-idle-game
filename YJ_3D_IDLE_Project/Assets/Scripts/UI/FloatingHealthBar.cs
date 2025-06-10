using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    private Slider slider;
    private Transform cameraTransform;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        if (maxHealth > 0)
        {
            slider.value = health / maxHealth;
        }
    }
}
