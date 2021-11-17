using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Awake()
    {
        GameDataManager.LoadPlayerData();
        GameDataManager.LoadCharactersShopData();
    }

    public void PlayGame()
    {
        Loader.Load(Loader.Scene.Test);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
