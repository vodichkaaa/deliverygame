using UnityEngine;

[CreateAssetMenu (fileName = "CharacterShopDatabase", menuName = "Shopping/Characters shop database")]

public class CharacterShopDatabase : ScriptableObject
{
    public Character[] characters;

    public int CharactersCount => characters.Length;

    public Character GetCharacter(int index)
    {
        return characters[index];
    }

    public void PurchaseCharacter(int index)
    {
        characters[index].isPurshared = true;
    }

}
