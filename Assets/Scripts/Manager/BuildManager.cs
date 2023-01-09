using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BuildManager : SingleTon<BuildManager>
{
    [Header("Gold")]
    [SerializeField]
    public int gold;

    [Header("Build")]
    [SerializeField]
    public Tower selectedTower;
    [SerializeField]
    private GameObject BuildEffect;

    [Header("BuildableTowerList")]
    public Tower TowerList1;
    public Tower TowerList2;

    public UnityAction<int> OnChangeGold;


    public Tower SelectedTower
    {
        get { return selectedTower; }
        set { selectedTower = value; }  
    }

    public int Gold
    {
        get { return gold; }
        private set { gold = value; OnChangeGold?.Invoke(gold); }
    }


    public void Build(TowerPlace place)
    {
        if (null == SelectedTower)
            return;

        if (Gold < SelectedTower.Cost)
            return;

        Instantiate(BuildEffect, place.transform.position, place.transform.rotation);
        StartCoroutine(BuildTower(place));
        
    }

    public void Sell(TowerPlace place)
    {
        Destroy(place.tower.gameObject);
        Gold += place.tower.SellCost;
        Debug.Log("타워를 판매했습니다.");
    }

    public void GoldUpdate()
    {
        Gold += 1;
        Gold -= 1;
    }

    public void GainEnergy(int gold)
    {
        Gold += gold;
    }

    public IEnumerator BuildTower(TowerPlace place)
    {
        while (true)
        {
            Tower tower = Instantiate(selectedTower, place.transform.position, place.transform.rotation);
            place.tower = tower;
            Gold -= tower.Cost;
            tower.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.35f);
            tower.gameObject.SetActive(true);
            break;
        }
    }
}