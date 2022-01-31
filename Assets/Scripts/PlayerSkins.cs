using UnityEngine;

public class PlayerSkins : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] _skins;
    
    private void Awake()
    {
        ChangePlayerSkin();
    }

    private void ChangePlayerSkin()
    {
        var selectedSkin = GameDataManager.GetSelectedCharacterIndex();
        _skins[selectedSkin].SetActive(true);

        for (int i = 0; i < _skins.Length; i++)
        {
            if(i != selectedSkin)
                _skins[i].SetActive(false);
        }
    }
}
