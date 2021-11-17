using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _gameMenu = null;
    [SerializeField] 
    private GameObject _pauseMenu = null;

    private bool _isPaused = false;

    private void Start()
    {
        _isPaused = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (_isPaused)
        {
            Time.timeScale = 0;
            
            _gameMenu.SetActive(false);
            _pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            
            _gameMenu.SetActive(true);
            _pauseMenu.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Loader.Load(Loader.Scene.MainMenu);
    }
}
