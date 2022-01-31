using UnityEngine;
using System.Collections;
using TMPro;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter: MonoBehaviour 
{
    private TMP_Text _txt;
    private string _story;

    void Awake () 
    {
        _txt = GetComponent<TMP_Text>();
        _story = _txt.text;
        _txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine ("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in _story) 
        {
            _txt.text += c;
            yield return new WaitForSeconds (0.125f);
        }
    }

}
