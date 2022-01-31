using System;
using TMPro;
using UnityEngine;

public class EndingScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameMenu;
    [SerializeField]
    private GameObject _endMenu;
    
    [SerializeField]
    private DayNightScript _dayNight;
    
    private QuestHandler _questHandler;

    [SerializeField] 
    private TMP_Text _payDayText;

    private bool isPaid = false;

    private void Start()
    {
        _questHandler = GetComponent<QuestHandler>();
        isPaid = false;
    }
    
    private void Update()
    {
        if (_dayNight.isEnded)
        {
            _gameMenu.SetActive(false);
            _endMenu.SetActive(true);
            //Time.timeScale = 0;

            _payDayText.text = "" + _questHandler.payDay;
            
            if (!isPaid)
            {
                GameDataManager.AddMoney(_questHandler.payDay);
                GameSharedUI.Instance.UpdateMoneyUIText();
                isPaid = true;
            }

            Debug.Log("Added: " + _questHandler.payDay);
        }
    }
}
