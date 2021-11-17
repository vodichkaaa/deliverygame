using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSharedUI : MonoBehaviour
{
    #region Singleton class: GameSharedUI

    public static GameSharedUI Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    #endregion

    [SerializeField] 
    private TMP_Text[] _moneyUIText = null;
    [SerializeField] 
    private TMP_Text[] _dayUIText = null;
    
    [SerializeField] 
    private Slider[] _volumeSlider = null;

    private event Action OnUIUpdate = delegate { };

    private void OnEnable()
    {
        OnUIUpdate += UpdateMoneyUIText;
        OnUIUpdate += UpdateDayUIText;
        OnUIUpdate += UpdateVolumeSlider;

    }

    private void OnDisable()
    {
        OnUIUpdate -= UpdateMoneyUIText;
        OnUIUpdate -= UpdateDayUIText;
        OnUIUpdate -= UpdateVolumeSlider;
    }

    private void Start()
    {
        OnUIUpdate();
    }

    public void UpdateMoneyUIText()
    {
        foreach (var money in _moneyUIText)
        {
            SetMoneyText(money, GameDataManager.GetMoney());
        }
    }

    public void UpdateDayUIText()
    {
        foreach (var day in _dayUIText)
        {
            SetDayText(day, GameDataManager.GetDay());
        }
    }

    private void UpdateVolumeSlider()
    {
        foreach (var volume in _volumeSlider)
        {
            GameDataManager.LoadVolumeData(volume);
        }
    }
    
    public void SaveVolumeSlider()
    {
        foreach (var volume in _volumeSlider)
        {
            GameDataManager.SaveVolumeData(volume.value);
        }
    }
    
    private static void SetDayText(TMP_Text textMesh, int value)
    {
        textMesh.text = "DAY: " + value;
    }
    
    private static void SetMoneyText(TMP_Text textMesh, int value) => 
        textMesh.text = value >= 1000 ? textMesh.text = $"{(value / 1000)}.{GetFirstDigitNumber(value % 1000)}K" : textMesh.text = value.ToString();
    
    private static int GetFirstDigitNumber(int num) => int.Parse(num.ToString()[0].ToString());

}
