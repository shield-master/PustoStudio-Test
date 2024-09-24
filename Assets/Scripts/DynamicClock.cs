using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class DynamicClock : MonoBehaviour
{
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;

    private const float AnimationDuration = 1f;

    private void Start()
    {
        UpdateClockHands(DynamicTime.LocalTime);
        StartCoroutine(UpdateClockHandsEverySecond());
    }

    private IEnumerator UpdateClockHandsEverySecond()
    {
        while (true)
        {
            UpdateClockHands(DynamicTime.LocalTime);
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateClockHands(DateTime currentTime)
    {
        var secondsAngle = currentTime.Second * 6f;
        UpdateHandRotation(secondHand, secondsAngle);
        
        var minutesAngle = currentTime.Minute * 6f;
        UpdateHandRotation(minuteHand, minutesAngle);
        
        var hours = currentTime.Hour % 12 + currentTime.Minute / 60f;
        var hoursAngle = hours * 30f;
        UpdateHandRotation(hourHand, hoursAngle);
    }

    private void UpdateHandRotation(Transform hand, float targetAngle)
    {
        var currentAngle = hand.localEulerAngles.z;
        var rotation = Mathf.DeltaAngle(currentAngle, -targetAngle);
        hand.DORotate(new Vector3(0, 0, currentAngle + rotation), AnimationDuration, RotateMode.FastBeyond360);
    }
    
}
