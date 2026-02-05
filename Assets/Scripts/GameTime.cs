using UnityEngine;
using System;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance { get; private set; }

    public DateTime currentDateTime = new DateTime(2287, 1, 1, 8, 0, 0); // example starting date
    public event Action<DateTime> OnTimeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddSeconds(int seconds)
    {
        currentDateTime = currentDateTime.AddSeconds(seconds);
        OnTimeChanged?.Invoke(currentDateTime);
    }

    public void AddMinutes(int minutes)
    {
        currentDateTime = currentDateTime.AddMinutes(minutes);
        OnTimeChanged?.Invoke(currentDateTime);
    }
}
