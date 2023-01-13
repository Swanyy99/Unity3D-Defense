using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    private Enemy enemyPrefab;
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

        if (GameManager.Instance.GameOn == false)
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

        if (SpawnedMonster >= Wave * 10)
        {
            GameManager.Instance.GameOn = false;
            Wave += 1;
            NowWaveText.text = "WAVE " + Wave.ToString();
            SpawnedMonster = 0;
            WaveMonsterDeath = 0;
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

            if (SpawnedMonster < Wave * 10)
            {
                SpawnedMonster += 1;
                Instantiate(enemyPrefab, WayPoints.First().position, WayPoints.First().rotation);
            }

            
        }
    }

    //public void TakeDamage(int damage)
    //{
    //    Heart -= damage;

    //    // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    //}
}