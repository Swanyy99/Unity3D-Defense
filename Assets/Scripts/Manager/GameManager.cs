using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool GameOn;

    public float GameSpeed = 1f;

    public Enemy target;

    

    private void Update()
    {
        if (GameOn == false) ;
    }

    public void Resume()
    {
        Time.timeScale = GameSpeed;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}