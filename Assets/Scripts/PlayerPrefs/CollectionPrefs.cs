using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionPrefs : MonoBehaviour
{
    public static void SetInts(string key, ICollection<int> collection)
    {
        var count = PlayerPrefs.GetInt(key + ".Count", 0);

        for (var i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey(key + "[" + i + "]");
        }
        
        PlayerPrefs.SetInt(key + ".Count", collection.Count);
        
        for (var i = 0; i < collection.Count; i++)
        {
            PlayerPrefs.SetInt(key + "[" + i + "]", collection.ElementAt(i));
        }
    }

    public static IEnumerable<int> GetInts(string key)
    {
        var count = PlayerPrefs.GetInt(key + ".Count", 0);
        var array = new int[count];
        
        for (var i = 0; i < count; i++)
        {
            array[i] = PlayerPrefs.GetInt(key + "[" + i + "]");
        }

        return array;
    }
}
