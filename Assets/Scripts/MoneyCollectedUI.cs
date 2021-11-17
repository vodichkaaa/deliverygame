using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyCollectedUI : MonoBehaviour
{
    private TMP_Text _moneyCollectedText = null;
    
    [SerializeField] 
    private GameObject _target = null;
    
    [SerializeField] 
    private Camera _camera = null;
    [SerializeField] 
    private Canvas _canvas = null;
    
    private void Start()
    {
        _moneyCollectedText = GetComponent<TMP_Text>();
        _moneyCollectedText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        transform.position = SetScreenPosition();
    }

    private Vector3 SetScreenPosition()
    {
        if (Camera.main == null) 
            return transform.position;
        
        var screen = Camera.main.WorldToScreenPoint(_target.transform.position);
        screen.z = (_canvas.transform.position - _camera.transform.position).magnitude;
        var position = _camera.ScreenToWorldPoint(screen);
        
        return position;
    }
    
    public void ShowCollected(int amount)
    {
        _moneyCollectedText.text = "+" + amount + "$";
        _moneyCollectedText.gameObject.SetActive(true);
        StartCoroutine(ShowAnimation());
    }
    
    private IEnumerator ShowAnimation()
    {
        yield return new WaitForSeconds(3f);
        _moneyCollectedText.gameObject.SetActive(false);
    }
}
