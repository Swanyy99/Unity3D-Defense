using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BuildManager : SingleTon<BuildManager>
{
    [Header("Energy")]
    [SerializeField]
    private int energy;

    [Header("Build")]
    [SerializeField]
    public Tower selectedTower;

    [Header("BuildableTowerList")]
    public Tower TowerList1;
    public Tower TowerList2;

    public UnityAction<int> OnChangeEnergy;


    public Tower SelectedTower
    {
        get { return selectedTower; }
        set { selectedTower = value; }  
    }

    public int Energy
    {
        get { return energy; }
        private set { energy = value; OnChangeEnergy?.Invoke(energy); }
    }

    public void Build(TowerPlace place)
    {
        if (null == SelectedTower)
            return;

        if (Energy < SelectedTower.Cost)
            return;

        Tower tower = Instantiate(selectedTower, place.transform.position, place.transform.rotation);
        place.tower = tower;
        Energy -= tower.Cost;
    }

    public void Sell(TowerPlace place)
    {
        Destroy(place.tower.gameObject);
        Energy += place.tower.Cost;
        Debug.Log("타워를 판매했습니다.");
    }

    public void GainEnergy(int energy)
    {
        Energy += energy;
    }
}