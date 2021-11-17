using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] 
    private Color _itemNotSelectedColor = Color.clear;
    [SerializeField] 
    private Color _itemSelectedColor = Color.clear;
    
    
    [Space(20f)] 
    [SerializeField] 
    private Image _characterImage = null;
    [SerializeField] 
    private Image _characterAccelerationFill = null;
    [SerializeField] 
    private Image _characterSteeringFill = null;
    
    [SerializeField]
    private TMP_Text _characterPriceText = null;
    [SerializeField] 
    private TMP_Text _characterNameText = null;
    
    [SerializeField] 
    private Button _characterPurchaseButton = null;

    
    [Space(20f)] 
    [SerializeField] 
    private Button _itemButton = null;
    
    [SerializeField] 
    private Image _itemImage = null;
    
    [SerializeField] 
    private Outline _itemOutline = null;
    
    public void SetItemPosition(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }

    public void SetCharacterImage(Sprite sprite) => _characterImage.sprite = sprite;

    public void SetCharacterName(string name) => _characterNameText.text = name;

    public void SetCharacterAcceleration(float acceleration) => 
        _characterAccelerationFill.fillAmount = acceleration / 100;

    public void SetCharacterSteering(float steering) => 
        _characterSteeringFill.fillAmount = steering / 100;

    public void SetCharacterPrice(int price) => 
        _characterPriceText.text = price.ToString();

    public void SetCharacterAsPurchased()
    {
        _characterPurchaseButton.gameObject.SetActive(false);
        _itemButton.interactable = true;

        _itemImage.color = _itemNotSelectedColor;
    }

    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        _characterPurchaseButton.onClick.RemoveAllListeners();
        _characterPurchaseButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        _itemButton.interactable = true;
        _itemButton.onClick.RemoveAllListeners();
        _itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void SelectItem()
    {
        _itemOutline.enabled = true;
        _itemImage.color = _itemSelectedColor;
        _itemButton.interactable = false;
    }

    public void DeselectItem()
    {
        _itemOutline.enabled = false;
        _itemImage.color = _itemNotSelectedColor;
        _itemButton.interactable = true;
    }

}
