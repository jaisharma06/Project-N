using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Anvarat/UI/Healthbar")]
public class Healthbar : MonoBehaviour
{
    public Image fillImage;
    public bool setValueMaximumOnAwake = true;
    public float minValue = 0;
    public float maxValue = 1;
    
    public float progressSmoothening = 1f;
    
    public float value { get; set; }
    private float _targetValue;
    
    private void Start()
    {
        if (setValueMaximumOnAwake)
        {
            value = maxValue;
        }
        _targetValue = value;
        SetFillAmount();
    }

    private void Update()
    {
        value = Mathf.MoveTowards(value, _targetValue, progressSmoothening * Time.deltaTime);
        SetFillAmount();
    }

    public void SetValue(float health)
    {
        value = health;
        _targetValue = value;
        SetFillAmount();
    }

    public void AddHealth(float deltaHealth)
    {
       ChangeHealth(deltaHealth);
    }

    public void SubtractHealth(float deltaHealth)
    {
        ChangeHealth(-deltaHealth);
    }

    private void ChangeHealth(float deltaHealth)
    {
        _targetValue += deltaHealth;
        _targetValue = Mathf.Clamp(_targetValue, minValue, maxValue);
    }

    private void SetFillAmount()
    { 
        fillImage.fillAmount = value / (maxValue - minValue);
    }
}
