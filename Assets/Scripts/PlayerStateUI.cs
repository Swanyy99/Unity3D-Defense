using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI heartUI;
    [SerializeField]
    private TextMeshProUGUI energyUI;

    private void Start()
    {
        WaveManager.Instance.OnHeartChanged += ChangeHeart;
        BuildManager.Instance.OnChangeEnergy += ChangeEnergy;

        ChangeHeart(WaveManager.Instance.Heart);
        ChangeEnergy(BuildManager.Instance.Energy);
    }

    public void ChangeHeart(int heart)
    {
        heartUI.text = heart.ToString();
    }

    public void ChangeEnergy(int energy)
    {
        energyUI.text = energy.ToString();
    }
}