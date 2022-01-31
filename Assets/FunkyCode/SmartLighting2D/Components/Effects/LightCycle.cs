using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightCycleBuffer {
    public Gradient gradient = new Gradient();
}

[System.Serializable]
public class LightDayProperties {
    [Range(0, 360)]
    public float shadowOffset = 0;

    public AnimationCurve shadowHeight = new AnimationCurve();

    public AnimationCurve shadowAlpha = new AnimationCurve();  
}

[ExecuteInEditMode]
public class LightCycle : MonoBehaviour {
    [Range(0, 1)]
    public float time = 0;

    private float sunCycle = 0.0017f;
    
    private float height;
    private float alpha;
    private float time360;

    public LightDayProperties dayProperties = new LightDayProperties();

    public LightCycleBuffer[] nightProperties = new LightCycleBuffer[1];

    private DayNightScript _tickCycle;
    
    private float timeSinceLastCalled;

    private float delay = 1f;

    private void Start()
    {
        time = 0f;
        _tickCycle = GetComponent<DayNightScript>();
    }

    public void SetTime(float setTime) {
        time = setTime;
    }
    
    private void LateUpdate() 
    {
        var bufferPresets = Lighting2D.Profile.bufferPresets;

        if (bufferPresets == null) 
        {
            return;
        }
        
        TimeController();
        
        // Day Lighting Properties
        height = dayProperties.shadowHeight.Evaluate(time);
        alpha = dayProperties.shadowAlpha.Evaluate(time);
        
        Lighting2D.DayLightingSettings.height = height;
        Lighting2D.DayLightingSettings.alpha = alpha;
        Lighting2D.DayLightingSettings.direction = time360 + dayProperties.shadowOffset;
        
        if (height < 0.01f) {
            height = 0.01f;
        }

        if (alpha < 0) {
            alpha = 0;
        }
        
        // Dynamic Properties
        for(int i = 0; i < nightProperties.Length; i++) 
        {
            if (i >= bufferPresets.list.Length) 
            {
                return;
            }

            LightCycleBuffer buffer = nightProperties[i];

            if (buffer == null) 
            {
                continue;
            }

            Color color = buffer.gradient.Evaluate(time);

            LightingSettings.BufferPreset bufferPreset = bufferPresets.list[i];
            bufferPreset.darknessColor = color;
        }
    }
    
    private void TimeController()
    {
        timeSinceLastCalled += Time.deltaTime;
        if (timeSinceLastCalled > delay)
        {
            time += sunCycle;
            timeSinceLastCalled = 0f;
        }
        
        time %= 1;
        time360 = time * 360;
    }
}
