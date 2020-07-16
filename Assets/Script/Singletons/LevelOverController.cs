using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{

    private static int totalLevelCount = 10;
    public int CurrentLevel { get; set; }

    public static LevelOverController Instance { get; private set; }
    public static int TotalLevels { get { return totalLevelCount; } }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Someone trying to create a duplicate of Singleton!");
            Destroy(this);
        }
        EventServices eventServices = new EventServices();
    }

    void Start()
    {
        EventServices.GenericInstance.OnWin += WinRoutine;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        CurrentLevel = 0;
    }

    public void ChooseLevel(int level)
    {
        SceneManager.LoadScene("GameScene");
        CurrentLevel = level;
    }

    private void WinRoutine()
    {
        if(CurrentLevel < totalLevelCount)
        {
            CurrentLevel++;
            return;
        }        
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
