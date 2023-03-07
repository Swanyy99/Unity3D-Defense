using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private Slider MASTERSlider;
    [SerializeField]
    private TextMeshProUGUI MASTERSliderValueText;
    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private TextMeshProUGUI BGMSliderValueText;
    [SerializeField]
    private Slider SFXSlider;
    [SerializeField]
    private TextMeshProUGUI SFXSliderValueText;

    private void OnEnable()
    {
        GameManager.Instance.Pause();
        Cursor.lockState = CursorLockMode.Confined;
        BGMSliderValueSet();
        SFXSliderValueSet();
    }

    private void OnDisable()
    {
        GameManager.Instance.Resume();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        continueButton.onClick.AddListener(Continue);
        restartButton.onClick.AddListener(Restart);
        quitButton.onClick.AddListener(QuitGame);

        MASTERSliderValueSet();
        BGMSliderValueSet();
        SFXSliderValueSet();
    }

    public void Continue()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        GameManager.Instance.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SpeedX1Game()
    {
        GameManager.Instance.GameSpeed = 1f;
    }

    public void SpeedX2Game()
    {
        GameManager.Instance.GameSpeed = 2f;
    }

    public void MASTERSliderValueSet()
    {
        int val = (int)MASTERSlider.value;
        //MASTERSliderValueText.text = val.ToString();
        SoundManager.Instance.MASTERSliderValueSet(val);
    }

    public void BGMSliderValueSet()
    {
        int val = (int)BGMSlider.value;
        //BGMSliderValueText.text = val.ToString();
        SoundManager.Instance.BGMSliderValueSet(val);
    }

    public void SFXSliderValueSet()
    {
        int val = (int)SFXSlider.value;
        //SFXSliderValueText.text = val.ToString();
        SoundManager.Instance.SFXSliderValueSet(val);
    }
}