using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour {
	public static GlobalController instance;

    public LevelManager levelManager;
    public CutsceneController cutsceneController;

    public int latestLevelUnlocked = 1;
    public string currentLevel = "Level 1";
	public bool bandaidTaken = false;
    public bool needleTaken = false;

	void Awake()
	{
		if (instance == null) 
		{
			DontDestroyOnLoad (gameObject);
			instance = this;
		} 
		else if (instance != this)
			Destroy (gameObject);
	}

	void Start()
	{
        currentLevel = PlayerPrefs.GetString("CurrentLevel");
        latestLevelUnlocked = PlayerPrefs.GetInt("LatestLevel");
        if (latestLevelUnlocked < levelManager.levelNum)
            latestLevelUnlocked = levelManager.levelNum;
        
        if (levelManager.levelNum != -1)
            currentLevel = levelManager.thisLevel;
	}

	void OnApplicationQuit()
	{
        PlayerPrefs.SetString("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("LatestLevel", latestLevelUnlocked);
	}

    public void NextLevel()
    {
        SceneManager.LoadScene(levelManager.nextLevel);
    }

    public void PlayCutscene(int id)
    {
        cutsceneController.SetSceneNum(id);
        SceneManager.LoadScene("Cutscenes");
    }

    public void ExitCutscene()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
