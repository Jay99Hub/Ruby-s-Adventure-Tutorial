using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    enum TimerType {Countdown, Stopwatch}
    [SerializeField] private TimerType timerType;
    public const int secondsInMinute = 60;
    [SerializeField] private float timeToDisplay = 30.0f;

    private bool _isRunning;

    
    void Awake()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    public float GetSeconds()
    {
        return(timeToDisplay);
    }

    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.TimerUpdate += EventManagerOnTimerUpdate;
       
    }

    private void OnDisable()
    {

        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.TimerUpdate -= EventManagerOnTimerUpdate;
       
    }

    private void EventManagerOnTimerPause() => _isRunning = false;
    private void EventManagerOnTimerStop() => _isRunning = false;

    private void EventManagerOnTimerStart() => _isRunning = true;


    private void EventManagerOnTimerUpdate(float value) => timeToDisplay += value;

    private void Update()
    {
        if (!_isRunning) return;
        if (timerType == TimerType.Countdown && timeToDisplay < 0.0f)
        {
            EventManager.OnTimerStop();
            return;
        }
        timeToDisplay += timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        _timerText.text = timeSpan.ToString(@"mm\:ss");

    }


}
