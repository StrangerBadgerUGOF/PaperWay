using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Timer controller
public class TimerController : MonoBehaviour
{
    // Event for runned out time
    public event Action TimeRunnedOutEvent;

    // Passed time text
    [SerializeField]
    private Text _totalPassedTimeText;
    // Passed time
    private float _totalPassedTime;
    // Timer counter
    [SerializeField]
    private float _timeCounter;
    // Timer text
    [SerializeField]
    private Text _timerText;

    // Boolean variable for timer control
    public bool IsCounting { get; set; }
    // Time counter
    public float TotalPassedTime { get { return _totalPassedTime; } }

    // Start is called before the first frame update
    void Start()
    {
        _totalPassedTime = 0;
        _timerText.text = _timeCounter.ToString();
        IsCounting = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't update anything, if time reached zero or timer wasn't started
        if (_timeCounter <= 0 || !IsCounting) { return; }
        // Update past time
        _totalPassedTime += Time.deltaTime;
        _totalPassedTimeText.text = ((int)Mathf.Round(_totalPassedTime) / 60).ToString("00") + ":" + 
            ((int)Mathf.Round(_totalPassedTime) % 60).ToString("00");
        // Update time counter
        _timeCounter -= Time.deltaTime;
        _timerText.text = Mathf.Round(_timeCounter).ToString();
        // Call event, if we have reached zero or below
        if (_timeCounter <= 0) 
        {
            // Stop timer
            IsCounting = false;
            _timeCounter = 0; 
            TimeRunnedOutEvent?.Invoke();
        }
    }

    // Adds time to the timer
    public void AddTime(int time)
    {
        _timeCounter += time;
    }
}
