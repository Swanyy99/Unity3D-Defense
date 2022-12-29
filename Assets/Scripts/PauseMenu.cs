using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button X2Button;
    [SerializeField]
    private Button X1Button;

    private void OnEnable()
    {
        GameManager.Instance.Pause();
    }

    private void OnDisable()
    {
        GameManager.Instance.Resume();
    }

    private void Start()
    {
        continueButton.onClick.AddListener(Continue);
        restartButton.onClick.AddListener(Restart);
        X1Button.onClick.AddListener(SpeedX1Game);
        X2Button.onClick.AddListener(SpeedX2Game);
    }

    public void Continue()
    {
        Destroy(gameObject);
    }

    public void Restart()
    {
        GameManager.Instance.LoadScene("GameScene");
    }

    public void SpeedX1Game()
    {
        GameManager.Instance.GameSpeed = 1f;
    }

    public void SpeedX2Game()
    {
        GameManager.Instance.GameSpeed = 2f;
    }
}