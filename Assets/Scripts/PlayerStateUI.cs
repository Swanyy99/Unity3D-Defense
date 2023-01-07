using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI heartUI;
    [SerializeField]
    private TextMeshProUGUI goldUI;

    private void Start()
    {
        WaveManager.Instance.OnHeartChanged += ChangeHeart;
        BuildManager.Instance.OnChangeGold += ChangeGold;

        ChangeHeart(WaveManager.Instance.Heart);
        ChangeGold(BuildManager.Instance.Gold);
    }

    public void ChangeHeart(int heart)
    {
        heartUI.text = heart.ToString();
    }

    public void ChangeGold(int gold)
    {
        goldUI.text = gold.ToString();
    }
}