using System;
using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeDisplay = null;
    
    public float tick = 0; // Increasing the tick, increases second rate
    
    private float _seconds = 0; 
    
    private int _minutes = 0;
    private int _hours = 6;
    
    private bool _isActivateLights = false;
    public bool isEnded = false;
    
    [Header("UI Elements")]
    [SerializeField]
    private GameObject[] _lights;

    private event Action OnTime = delegate { };

    private void OnEnable()
    {
        OnTime += CalcTime;
        OnTime += DisplayTime;
    }
    
    private void OnDisable()
    {
        OnTime -= CalcTime;
        OnTime -= DisplayTime;
    }

    private void Start()
    {
        _isActivateLights = true;
    }
    
    private void FixedUpdate()
    {
        OnTime();
    }

    private void CalcTime()
    {
        _seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (_seconds >= 60)
        {
            _seconds = 0;
            _minutes += 1;
        }

        if (_minutes >= 60)
        {
            _minutes = 0;
            _hours += 1;
        }

        if (_hours >= 22)
        {
            _hours = 6;

            
            GameDataManager.AddDay(1);
            GameSharedUI.Instance.UpdateDayUIText();
            
            isEnded = true;
        }
        PostProcessingControl();
    }

    private void PostProcessingControl() // used to adjust the post processing slider.
    {
        
        if(_hours >= 18 && _hours < 19)
        {
            if (_isActivateLights == false && _minutes > 45) 
            {
                foreach (var lights in _lights)
                {
                    lights.SetActive(true);
                }
                _isActivateLights = true;
            }
        }
        
        if(_hours >= 6 && _hours < 7) // Dawn at 6:00 / 6am - until 7:00 / 7am
        {
            if (_isActivateLights && _minutes > 45)
            {
                foreach (var lights in _lights)
                {
                    lights.SetActive(false);
                }
                _isActivateLights = false;
            }
        }
    }

    private void DisplayTime() => _timeDisplay.text = $"{_hours:00}:{_minutes:00}";

}