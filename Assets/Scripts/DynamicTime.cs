using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public sealed class DynamicTime : MonoBehaviour
{
    public static DateTime LocalTime { get; set; }
    public static event Action OnTimeSynced; 
    
    [SerializeField] private TextMeshProUGUI timeText;
    
    private const float SecondUpdateInterval = 1f;
    private const float HourUpdateInterval = 20f;
    private Coroutine _updateTimeRoutine;
    
    [Serializable]
    private class TimeResponse
    {
        public string utc_offset;
        public string timezone;
        public long unixtime;
        public string datetime;
    }

    private void Start()
    {
        StartCoroutine(GetTimeFromAPI("https://worldtimeapi.org/api/timezone/Europe/Moscow"));
        StartCoroutine(SyncEveryHour());
        
        _updateTimeRoutine = StartCoroutine(UpdateLocalTime());
    }

    private IEnumerator GetTimeFromAPI(string url)
    {
        int retries = 3;
        while (retries > 0)
        {
            using var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var timeResponse = JsonUtility.FromJson<TimeResponse>(request.downloadHandler.text);
                
                LocalTime = DateTimeOffset.FromUnixTimeSeconds(timeResponse.unixtime).DateTime;

                UpdateTimeUI(LocalTime);
                OnTimeSynced?.Invoke();
                yield break;
            }
            else
            {
                Debug.LogError($"Failed from API: {request.error}. Retry");
                retries--;
                yield return new WaitForSeconds(2);
            }
        }
    }

    private IEnumerator UpdateLocalTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(SecondUpdateInterval);
            LocalTime = LocalTime.AddSeconds(1);
            UpdateTimeUI(LocalTime);
        }
    }
    
    private void UpdateTimeUI(DateTime dateTime)
    {
        var hours = dateTime.ToString("HH");
        var minutes = dateTime.ToString("mm");
        var seconds = dateTime.ToString("ss");
        
        timeText.text = $"{hours}:{minutes}:{seconds}";
    }

    private IEnumerator SyncEveryHour()
    {
        while (true)
        {
            yield return new WaitForSeconds(HourUpdateInterval);
            StartCoroutine(GetTimeFromAPI("https://worldtimeapi.org/api/timezone/Europe/Moscow"));
            Debug.Log("Sync time");
        }
    }
}