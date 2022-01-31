using System;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] 
    private QuestPointer _questPointer = null;
    
    [SerializeField] 
    private GameObject[] _questStart = null;
    [SerializeField] 
    private GameObject[] _questEnd = null;
    
    public MoneyCollectedUI moneyUI = null;
    
    [SerializeField]
    private TMP_Text _payDayText = null;
    
    [SerializeField]
    private TMP_Text _timerText = null;

    private int _randomAmount = 0;
    
    [SerializeField]
    private int _i = 0;
    [SerializeField]
    private int _k = 0;
    
    public int payDay = 0;
    
    private float timeRemaining = 61;
    
    private const int MinPayment = 210;
    private const int MaxPayment = 550;
    
    private const float QuestChecker = 3.0f;

    public AudioClip notificationSound = null;
    public AudioClip moneySound = null;
    public AudioClip badEndingSound = null;

    private void Start()
    {
        _timerText.gameObject.SetActive(false);
        
        moneyUI = GetComponentInChildren<MoneyCollectedUI>();
        AudioSource audio = GetComponent<AudioSource>();

        if (Camera.main == null) 
            return;
        
        QuestUpdate();
        
        audio.clip = notificationSound;
        audio.Play();
        
        int state = 0;
        FunctionUpdater.Create(() =>
        {
            switch (state)
            {
                case 0:
                    _questPointer.Show(_questStart[_i].transform.position);
                    
                    timeRemaining = 61;
                    
                    if (Vector2.Distance(Camera.main.transform.position, _questStart[_i].transform.position) < QuestChecker)
                    {
                        _timerText.gameObject.SetActive(true);
                        _questPointer.Show(_questEnd[_k].transform.position);
                        state = 1;
                        audio.clip = notificationSound;
                        audio.Play();
                    }
                    break;
                case 1:
                {
                    if (Vector2.Distance(Camera.main.transform.position, _questEnd[_k].transform.position) < QuestChecker)
                    {
                        _timerText.gameObject.SetActive(false);
                        _questPointer.Show(_questStart[_i].transform.position);
                        state = 0;
                        
                        if (timeRemaining > 0)
                        {
                            payDay += _randomAmount;
                            audio.clip = moneySound;
                            audio.Play();
                        }
                        else
                        {
                            _randomAmount = 0;
                            audio.clip = badEndingSound;
                            audio.Play();
                        }
                        
                        moneyUI.ShowCollected(_randomAmount);

                        QuestUpdate();
                    }
                    break;
                }
            }
        });
    }
    
    private void Update()
    {
        if (Camera.main == null) 
            return;

        _randomAmount = Random.Range(MinPayment, MaxPayment);

        _payDayText.text = payDay.ToString();
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        int time = (int) timeRemaining;

        _timerText.text = ":" + time;

        if (timeRemaining < 1)
        {
            _timerText.gameObject.SetActive(false);
        }

    }

    private void QuestUpdate()
    {
        _i = Random.Range(0, _questStart.Length);
        _k = Random.Range(0, _questEnd.Length);
        
        Debug.Log("Quest Updated");

    }
}
