using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private class LoadingMonoBehaviour: MonoBehaviour {}
    
    public enum Scene
    {
        Test,
        LoadingScene,
        MainMenu
    }
    
    private static event Action OnLoaderCallback = delegate {};
    private static AsyncOperation _loadingAsyncOperation = null;

    public static void Load(Scene scene)
    {
        OnLoaderCallback = () =>
        {
            var loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        _loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!_loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress() => _loadingAsyncOperation?.progress ?? 1f;

    public static void LoaderCallback()
    {
        if (OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }
}
