using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    public Sprite image = null;
    public string name = "";
    [Range(0, 100)] public float acceleration = .0f;
    [Range(0, 100)] public float steering = .0f;
    public int price = 0;
    public bool isPurshared = false;
}
