using System;
using System.Collections;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = false;

    private void Awake()
    {
        StartCoroutine(LoaderStart());
    }

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }

    private IEnumerator LoaderStart()
    {
        yield return new WaitForSeconds(1.5f);
        _isFirstUpdate = true;
    }
}
