using CodeMonkey.Utils;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] 
    private QuestPointer _questPointer = null;
    
    [SerializeField] 
    private GameObject _questStart = null;
    [SerializeField] 
    private GameObject _questEnd = null;
    
    private MoneyCollectedUI _moneyUI = null;

    private int _randomAmount = 0;
    
    private const int MinPayment = 210;
    private const int MaxPayment = 550;
    
    private const float QuestChecker = 3.0f;
    
    private void Start()
    {
        _moneyUI = GetComponentInChildren<MoneyCollectedUI>();
        
        if (Camera.main == null) 
            return;
        _questPointer.Show(_questStart.transform.position);

        int state = 0;
        FunctionUpdater.Create(() =>
        {
            switch (state)
            {
                case 0:
                    if (Vector2.Distance(Camera.main.transform.position, _questStart.transform.position) < QuestChecker)
                    {
                        _questPointer.Show(_questEnd.transform.position);
                        state = 1;
                    }

                    break;
                case 1:
                {
                    if (Vector2.Distance(Camera.main.transform.position, _questEnd.transform.position) < QuestChecker)
                    {
                        GameDataManager.AddMoney(_randomAmount);
                        _moneyUI.ShowCollected(_randomAmount);

                        GameSharedUI.Instance.UpdateMoneyUIText();
                        
                        _questPointer.Show(_questStart.transform.position);
                        state = 0;
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
    }
}
