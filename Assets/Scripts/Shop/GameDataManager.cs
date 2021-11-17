using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class CharactersShopData
{
    public List<int> purchasedCharactersIndexes = new List<int>();
}

[System.Serializable]
public class PlayerData
{
    public int money = 0;
    public int selectedCharacterIndex = 0;
    public int day = 0;
    public float volume = 0;
}

public static class GameDataManager
{
    private static readonly PlayerData PlayerData = new PlayerData();
    private static readonly CharactersShopData CharactersShopData = new CharactersShopData();
    private static Character SelectedCharacter = null;
    
    public static Character GetSelectedCharacter() => SelectedCharacter;

    public static void SetSelectedCharacter(Character character, int index)
    {
        SelectedCharacter = character;
        PlayerData.selectedCharacterIndex = index;
        SavePlayerData();
    }

    public static int GetSelectedCharacterIndex() => 
        PlayerData.selectedCharacterIndex;

    public static int GetMoney() => PlayerData.money;

    public static void AddMoney(int amount)
    {
        PlayerData.money += amount;
        SavePlayerData();
    }
    
    public static int GetDay() => PlayerData.day;

    public static void AddDay(int amount)
    {
        PlayerData.day += amount;
        SavePlayerData();
    }

    public static bool CanSpendMoney(int amount) => PlayerData.money >= amount;

    public static void SpendMoney(int amount)
    {
        PlayerData.money -= amount;
        SavePlayerData();
    }

    public static void LoadPlayerData()
    {
        PlayerData.money = PlayerPrefs.GetInt("money");
        PlayerData.day = PlayerPrefs.GetInt("day");
        PlayerData.selectedCharacterIndex = PlayerPrefs.GetInt("selectedCharacterIndex");

        Debug.Log("<color=green>[PlayerData] Loaded. </color>");
    }

    private static void SavePlayerData()
    {
        PlayerPrefs.SetInt("money", PlayerData.money);
        PlayerPrefs.SetInt("day", PlayerData.day);
        PlayerPrefs.SetInt("selectedCharacterIndex", PlayerData.selectedCharacterIndex);

        PlayerPrefs.Save();
        
        Debug.Log("<color=magenta>[PlayerData] Saved. </color>");
    }

    public static void AddPurchasedCharacter(int characterIndex)
    {
        CharactersShopData.purchasedCharactersIndexes.Add(characterIndex);
        SaveCharactersShopData();
    }

    public static List<int> GetAllPurchasedCharacter() => 
        CharactersShopData.purchasedCharactersIndexes;

    public static int GetAllPurchasedCharacter(int index) => 
        CharactersShopData.purchasedCharactersIndexes[index];

    public static void LoadCharactersShopData()
    {
        IEnumerable<int> collection = CollectionPrefs.GetInts("List");
        CharactersShopData.purchasedCharactersIndexes = collection.ToList();
        
        Debug.Log("<color=green>[CharactersShopData] Loaded. </color>");
    }

    private static void SaveCharactersShopData()
    {
        CollectionPrefs.SetInts("List", CharactersShopData.purchasedCharactersIndexes);
        
        Debug.Log("<color=magenta>[CharactersShopData] Saved. </color>");
    }

    public static void SaveVolumeData(float amount)
    {
        PlayerData.volume = amount;
        PlayerPrefs.SetFloat("volume", PlayerData.volume);
        PlayerPrefs.Save();
        
        Debug.Log("<color=magenta>[Volume] Saved. </color>");
    }

    public static void LoadVolumeData(Slider slider)
    {
        PlayerData.volume = PlayerPrefs.GetFloat("volume");
        slider.value = PlayerData.volume;
        AudioListener.volume = PlayerData.volume;
        
        Debug.Log("<color=green>[Volume] Loaded. </color>");
    }
}
