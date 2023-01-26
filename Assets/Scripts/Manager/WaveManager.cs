using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class WaveManager : SingleTon<WaveManager>
{
    [Header("Way")]
    [SerializeField]
    private Transform way;
    public List<Transform> WayPoints { get; private set; }

    //[Header("Heart")]
    //[SerializeField]
    //private int heart;

    //public UnityAction<int> OnHeartChanged;

    [Header("Enemy")]
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private float spawnDelay;
    private Coroutine spawnRoutine;

    [Header("Wave Status")]
    public int Wave = 1 ;
    public int SpawnedMonster = 0;
    public int WaveMonsterDeath = 0;

    [SerializeField]
    private TextMeshProUGUI NowWaveText;

    public bool WaveStart ;

    //public int Heart
    //{
    //    get { return heart; }
    //    private set { heart = value; OnHeartChanged?.Invoke(heart); }
    //}

    private void Awake()
    {
        GetWayPoints();
    }
    private void Start()
    {
        //spawnRoutine = StartCoroutine(SpawnRoutine());

    }

    private void Update()
    {
        //if (spawnDelay > 0.7f)
        //{
        //    if (Wave < 10)
        //        spawnDelay = 1 - (Wave / 10f);
        //}

        //if (GameManager.Instance.GameOn == false)
        //{
        //    StopCoroutine(SpawnRoutine());
        //}

        if (GameManager.Instance.GameOn == true && WaveStart == false)
        {
            spawnRoutine = StartCoroutine(SpawnRoutine());
            WaveStart = true;
        }

        if (GameManager.Instance.GameOn == false && SpawnedMonster == 0)
        {
            WaveMonsterDeath = 0;
            SpawnedMonster = 0;
            WaveStart = false;
        }

        //else if (GameManager.Instance.GameOn == true && WaveStart == false)
        //{
        //    spawnRoutine = StartCoroutine(SpawnRoutine());



        //    WaveStart = true;
        //}


       
            if (Wave % 5 != 0)
            {
                if (SpawnedMonster >= Wave * 5) 
                {
                    GameManager.Instance.GameOn = false;

                    if (WaveMonsterDeath >= Wave * 5)
                    {
                        Wave += 1;
                        NowWaveText.text = "WAVE " + Wave.ToString();
                        SpawnedMonster = 0;
                        WaveMonsterDeath = 0;
                    }
                }
            }

            else
            {
                if (SpawnedMonster >= 1)
                {
                    GameManager.Instance.GameOn = false;

                    if (WaveMonsterDeath >= 1)
                    {
                        Wave += 1;
                        NowWaveText.text = "WAVE " + Wave.ToString();
                        SpawnedMonster = 0;
                        WaveMonsterDeath = 0;
                    }
                }
            }
        

        
    }

    private void GetWayPoints()
    {
        WayPoints = new List<Transform>();
        for (int i = 0; i < way.childCount; i++)
        {
            WayPoints.Add(way.GetChild(i));
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (GameManager.Instance.GameOn == false)
            {
                break;
            }

            if (Wave % 5 != 0)
            {
                if (SpawnedMonster < Wave * 5)
                {
                    SpawnedMonster += 1;
                    GameObject instance = PoolManager.Instance.Get(enemyPrefab, WayPoints.First().position, WayPoints.First().rotation);
                    instance.GetComponent<NavMeshAgent>().enabled = true;
                    if (instance == null)
                        break;

                }
            }

            else
            {
                if (SpawnedMonster < 1)
                {
                    SpawnedMonster += 1;
                    Instantiate(bossPrefab, WayPoints.First().position, WayPoints.First().rotation);
                }
                
            }

            
        }
    }


}