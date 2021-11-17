using System;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class DayNightScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeDisplay = null;
    
    private Volume _postProcessingVolume = null;
    
    [SerializeField]
    private float _tick = 0; // Increasing the _tick, increases second rate
    
    private float _seconds = 0; 
    
    private int _minutes = 0;
    private int _hours = 0;

    private bool _isActivateLights = false;
    
    
    [Header("UI Elements")]
    [SerializeField]
    private GameObject[] _lights;

    [SerializeField]
    private SpriteRenderer[] _stars;
    
    [SerializeField]
    private GameObject _sun;
    
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
        _postProcessingVolume = gameObject.GetComponent<Volume>();
    }
    
    private void FixedUpdate()
    {
        OnTime();
    }

    private void CalcTime()
    {
        _seconds += Time.fixedDeltaTime * _tick; // multiply time between fixed update by tick

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

        if (_hours >= 24)
        {
            _hours = 0;

            
            GameDataManager.AddDay(1);
            GameSharedUI.Instance.UpdateDayUIText();
            
            Loader.Load(Loader.Scene.MainMenu);
        }
        PostProcessingControl();
    }

    private void PostProcessingControl() // used to adjust the post processing slider.
    {
        
        if(_hours >= 21 && _hours < 22) // dusk at 21:00 / 9pm - until 22:00 / 10pm
        {
            _postProcessingVolume.weight =  (float)_minutes / 60; // since dusk is 1 hr, we just divide the _minutes by 60 which will slowly increase from 0 - 1 
            foreach (var star in _stars)
            {
                var starsColor = star.color;
                starsColor = new Color(starsColor.r, starsColor.g, starsColor.b, (float)_minutes / 60);
                star.color = starsColor;
            }

            if (_isActivateLights == false && _minutes > 45) 
            {
                foreach (var lights in _lights)
                {
                    lights.SetActive(true);
                }
                _isActivateLights = true;
            }
            
            _sun.SetActive(false);
        }
        
        if(_hours >= 6 && _hours < 7) // Dawn at 6:00 / 6am - until 7:00 / 7am
        {
            _postProcessingVolume.weight = 1 - (float)_minutes / 60; // we minus 1 because we want it to go from 1 - 0
            foreach (var star in _stars)
            {
                var starsColor = star.color;
                starsColor = new Color(starsColor.r, starsColor.g, starsColor.b, 1 -(float)_minutes / 60);
                star.color = starsColor;
            }
            if (_isActivateLights && _minutes > 45)
            {
                foreach (var lights in _lights)
                {
                    lights.SetActive(false);
                }
                _isActivateLights = false;
            }
            
            _sun.SetActive(true);
        }
    }

    private void DisplayTime() => _timeDisplay.text = $"{_hours:00}:{_minutes:00}";

}