using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShopUI : MonoBehaviour
{
    [SerializeField]
    private CharacterShopDatabase _characterDB = null;
    
    
    [Header("Layout Settings")] 
    [SerializeField]
    private float _itemSpacing = .5f;

    private float _itemHeight = .0f;

    
    [Header("UI elements")]
    [SerializeField]
    private Transform _shopItemContainer = null;
    
    [SerializeField]
    private GameObject _itemPrefab = null;
    
    [SerializeField]
    private Image _mainMenuCharacterImage = null;
    
    [SerializeField] 
    private TMP_Text _noEnoughCoinsText = null;
    
    private int _newSelectedItemIndex = 0;
    private int _previousSelectedItemIndex = 0;

    private event Action OnSkinChange = delegate { };

    private void OnEnable()
    {
        OnSkinChange += GenerateShopItemsUI;
        OnSkinChange += SetSelectedCharacter;
        OnSkinChange += ChangePlayerSkin;
    }

    private void OnDisable() 
    {
        OnSkinChange -= GenerateShopItemsUI; 
        OnSkinChange -= SetSelectedCharacter;
        OnSkinChange -= ChangePlayerSkin;
    }

    private void Start()
    {
        _noEnoughCoinsText.gameObject.SetActive(false);
        
        OnSkinChange();
        SelectItemUI(GameDataManager.GetSelectedCharacterIndex());
    }

    private void SetSelectedCharacter()
    {
        var index = GameDataManager.GetSelectedCharacterIndex();
        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);
    }
    
    private void GenerateShopItemsUI()
    {
        for (var i = 0; i < GameDataManager.GetAllPurchasedCharacter().Count; i++)
        {
            var purchasedCharacterIndex = GameDataManager.GetAllPurchasedCharacter(i);
            _characterDB.PurchaseCharacter(purchasedCharacterIndex);
        }
        
        _itemHeight = _shopItemContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        
        Destroy(_shopItemContainer.GetChild(0).gameObject);
        _shopItemContainer.DetachChildren();

        for (var i = 0; i < _characterDB.CharactersCount; i++)
        {
            var character = _characterDB.GetCharacter(i);
            var uiItem = Instantiate(_itemPrefab, _shopItemContainer).GetComponent<CharacterItemUI>();

            uiItem.SetItemPosition(Vector2.down * i * (_itemHeight + _itemSpacing));

            uiItem.gameObject.name = "Item" + i + "-" + character.name;

            uiItem.SetCharacterName(character.name);
            uiItem.SetCharacterImage(character.image);
            uiItem.SetCharacterAcceleration(character.acceleration);
            uiItem.SetCharacterSteering(character.steering);
            uiItem.SetCharacterPrice(character.price);

            if (character.isPurshared)
            {
                uiItem.SetCharacterAsPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                uiItem.SetCharacterPrice(character.price);
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }

            _shopItemContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((_itemHeight + _itemSpacing) * _characterDB.CharactersCount + _itemSpacing);
        }
    }

    private void ChangePlayerSkin()
    {
        var character = GameDataManager.GetSelectedCharacter();
        
        if (character.image != null)
            _mainMenuCharacterImage.sprite = character.image;
    }
    private void OnItemSelected(int index)
    {
        SelectItemUI(index);
        
        GameDataManager.SetSelectedCharacter(_characterDB.GetCharacter(index), index);
        ChangePlayerSkin();
    }

    private void SelectItemUI(int itemIndex)
    {
        _previousSelectedItemIndex = _newSelectedItemIndex;
        _newSelectedItemIndex = itemIndex;

        var prevUIItem = GetItemUI(_previousSelectedItemIndex);
        var newUIItem = GetItemUI(_newSelectedItemIndex);
        
        prevUIItem.DeselectItem();
        newUIItem.SelectItem();
    }

    private CharacterItemUI GetItemUI(int index) => 
        _shopItemContainer.GetChild(index).GetComponent<CharacterItemUI>();

    private void OnItemPurchased(int index)
    {
        var character = _characterDB.GetCharacter(index);
        var uiItem = GetItemUI(index);

        if (GameDataManager.CanSpendMoney(character.price))
        {
            GameDataManager.SpendMoney(character.price);
            GameSharedUI.Instance.UpdateMoneyUIText();
            
            _characterDB.PurchaseCharacter(index);
            
            uiItem.SetCharacterAsPurchased();
            uiItem.OnItemSelect(index, OnItemSelected);
            
            GameDataManager.AddPurchasedCharacter(index);
        }
        else StartCoroutine(CoinsTextAnimation());
    }

    private IEnumerator CoinsTextAnimation()
    {
        _noEnoughCoinsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        _noEnoughCoinsText.gameObject.SetActive(false);
    }
}
