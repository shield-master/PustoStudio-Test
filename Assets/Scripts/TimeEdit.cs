using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TimeEdit : MonoBehaviour
{
    [SerializeField] private Button editButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject editLayout;
    [SerializeField] private TMP_InputField hoursField;
    [SerializeField] private TMP_InputField minutesField;
    [SerializeField] private TMP_InputField secondsField;
    
    private void Start()
    {
        editLayout.SetActive(false);
        
        editButton.onClick.AddListener(OpenEditLayout);
        submitButton.onClick.AddListener(SubmitTime);
        cancelButton.onClick.AddListener(CancelEdit);
    }

    private void OpenEditLayout()
    {
        editLayout.SetActive(true);
        
        hoursField.text = DynamicTime.LocalTime.Hour.ToString("00");
        minutesField.text = DynamicTime.LocalTime.Minute.ToString("00");
        secondsField.text = DynamicTime.LocalTime.Second.ToString("00");
    }

    private void SubmitTime()
    {
        if (ValidateInput())
        {
            int hours = int.Parse(hoursField.text);
            int minutes = int.Parse(minutesField.text);
            int seconds = int.Parse(secondsField.text);

            DynamicTime.LocalTime = new DateTime(1, 1, 1, hours, minutes, seconds);
            editLayout.SetActive(false);
        }
        else
        {
            Debug.Log("Некорректный ввод времени!");
        }
    }

    private void CancelEdit()
    {
        editLayout.SetActive(false);
    }
    
    private bool ValidateInput()
    {
        return ValidateField(hoursField.text, 0, 24) &&
               ValidateField(minutesField.text, 0, 59) &&
               ValidateField(secondsField.text, 0, 59);
    }

    private bool ValidateField(string input, int minValue, int maxValue)
    {
        if (input.Length == 0 || input.Length > 2) return false;
        if (!int.TryParse(input, out int value)) return false;

        return value >= minValue && value <= maxValue;
    }
}