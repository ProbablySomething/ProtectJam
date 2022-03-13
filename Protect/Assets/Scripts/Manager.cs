using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public enum LevelPhase { Sheep, Ghost, LevelComplete }

    [SerializeField] private GameObject sheperdObject;
    [SerializeField] private GameObject lambPlayerObject;
    [SerializeField] private GameObject lambPassiveObject;

    [SerializeField] private GameObject wolfSpawnerObject;
    [SerializeField] private GameObject foxSpawnerObject;

    [SerializeField] private GameObject snakeObject;
    [SerializeField] private GameObject clover;

    [SerializeField] public int cloverGoal;

    [SerializeField] private Transform lambSpawnPoint;
    [SerializeField] private Transform sheperdSpawnPoint;


    [SerializeField] private List<Transform> wolfSpawnPoints;
    [SerializeField] private List<Transform> snakeSpawnPoints;
    [SerializeField] private List<Transform> foxSpawnPoints;
    [SerializeField] private List<Transform> cloverSpawnPoints;

    public List<GameObject> Enemies { get; set; }
    private List<GameObject> enemiesToDestroy;

    [SerializeField] private CanvasGroup gameOverMenu;
    [SerializeField] private CanvasGroup victoryMenu;

    private GameObject lambInstance;
    private GameObject sheperdInstance;
    private List<GameObject> enemyInstances;

    public LevelPhase Phase { get; private set; }
    private Queue<(Vector2 pos, float time)> _moveHistory;
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get { return _moveHistory; } set { _moveHistory = value; moveHistoryClone = new Queue<(Vector2 pos, float time)>(value); } }
    private Queue<(Vector2 pos, float time)> moveHistoryClone;

    private int level = 1;





    private int _numOfClovers;
    public int numOfClovers { get { return _numOfClovers; }
        set {
            _numOfClovers = value;
            if(_numOfClovers >= cloverGoal)
            {
                SetPhase((int)LevelPhase.Ghost);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cloverGoal = cloverSpawnPoints.Count;
        SetPhase((int)LevelPhase.Sheep);
        enemyInstances = new List<GameObject>();
        enemiesToDestroy = new List<GameObject>();
        numOfClovers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClearInstances()
    {

        if (lambInstance)
        {
            Destroy(lambInstance);
        }
        Destroy(sheperdInstance);
        if (sheperdInstance)
        {
            
        }
        if (enemyInstances != null)
        {
            enemiesToDestroy = new List<GameObject>();
            foreach (GameObject enemy in enemyInstances)
            {
                enemiesToDestroy.Add(enemy);
                if (enemy)
                {
                    if (enemy.GetComponent<Spawner>())
                    {
                        enemy.GetComponent<Spawner>().Die();
                    }
                }
                else
                {
                    enemyInstances.Remove(null);
                }

                
                Destroy(enemy);
            }
            foreach (GameObject removeable in enemiesToDestroy)
            {
                enemyInstances.Remove(removeable);
            }
        }
    }

    private int max(int a, int b)
    {
        return a > b ? a : b;
    }


    private int min(int a, int b)
    {
        return a < b ? a : b;
    }

    private void SpawnGhostPhaseActors()
    {

        sheperdInstance = Instantiate(sheperdObject, sheperdSpawnPoint);
        

        lambInstance = Instantiate(lambPassiveObject, lambSpawnPoint);
        lambInstance.GetComponent<FollowMovement>().MoveHistory = MoveHistory;
        lambInstance.GetComponent<FollowMovement>().ManagerReference = GetComponent<Manager>();


        foreach (Transform loc in snakeSpawnPoints)
        {
            enemyInstances.Add(Instantiate(snakeObject, loc));
        }
        foreach (Transform loc in cloverSpawnPoints)
        {
            enemyInstances.Add(Instantiate(clover, loc));
        }

        foreach(Transform loc in wolfSpawnPoints)
        {
            enemyInstances.Add(Instantiate(wolfSpawnerObject, loc));
        }
        foreach (Transform loc in foxSpawnPoints)
        {
            enemyInstances.Add(Instantiate(foxSpawnerObject, loc));
        }
    }

    public void ToggleMenu(CanvasGroup group)
    {
        group.alpha = 1 - group.alpha;
        group.interactable = !group.interactable;
        group.blocksRaycasts = !group.blocksRaycasts;
    }

    public void GameOver()
    {
        ToggleMenu(gameOverMenu);
    }

    public void Victory()
    {
        if (lambInstance)
        {
            Destroy(lambInstance);
        }
        ToggleMenu(victoryMenu);
    }

    public void SetPhase(int newPhase)
    {

        switch ((LevelPhase)newPhase)
        {
            case LevelPhase.Sheep:

                numOfClovers = 0;
                ClearInstances();
                lambInstance = Instantiate(lambPlayerObject, lambSpawnPoint);
                lambInstance.GetComponent<LambController>().ManagerInstance = GetComponent<Manager>();

                foreach (Transform loc in cloverSpawnPoints)
                {
                    Instantiate(clover, loc);
                }

                break;

            case LevelPhase.Ghost:
                if (lambInstance)
                {
                    lambInstance.GetComponent<LambController>().handleMoveHistory();
                }

                ClearInstances();
                SpawnGhostPhaseActors();
                
                
                
                
                
                if(moveHistoryClone != null)
                {
                    MoveHistory = moveHistoryClone;
                }
                break;

            case LevelPhase.LevelComplete:

                break;
        }
    }
    
}
