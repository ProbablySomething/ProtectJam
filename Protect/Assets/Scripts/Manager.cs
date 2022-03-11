using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public enum LevelPhase { Sheep, Ghost, LevelComplete }

    [SerializeField] private Transform lambSpawnPoint;
    [SerializeField] private Transform sheperdSpawnPoint;
    [SerializeField] private Transform wolfSpawnPoints;
    [SerializeField] private Transform snakeSpawnPoints;
    [SerializeField] private Transform foxSpawnPoints;

    [SerializeField] private GameObject sheperdObject;
    [SerializeField] private GameObject lambPlayerObject;
    [SerializeField] private GameObject lambPassiveObject;
    [SerializeField] private GameObject wolfObject;
    [SerializeField] private GameObject snakeObject;
    [SerializeField] private GameObject foxObject;

    [SerializeField] private List<GameObject> enemies;

    [SerializeField] private CanvasGroup gameOverMenu;

    private GameObject lambInstance;
    private GameObject sheperdInstance;
    private List<GameObject> enemyInstances;

    public LevelPhase Phase { get; private set; }
    private Queue<(Vector2 pos, float time)> _moveHistory;
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get { return _moveHistory; } set { _moveHistory = value; moveHistoryClone = new Queue<(Vector2 pos, float time)>(value); } }
    private Queue<(Vector2 pos, float time)> moveHistoryClone;

    private (Vector2 pos, float time)[] queueElements; 

    // Start is called before the first frame update
    void Start()
    {
        lambInstance = Instantiate(lambPlayerObject, lambSpawnPoint);
        lambInstance.GetComponent<LambController>().ManagerInstance = GetComponent<Manager>();
        Phase = LevelPhase.Sheep;
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

            Phase = LevelPhase.Ghost;
            sheperdInstance = Instantiate(sheperdObject, sheperdSpawnPoint);
            lambInstance = Instantiate(lambPassiveObject, lambSpawnPoint);
            lambInstance.GetComponent<FollowMovement>().MoveHistory = MoveHistory;
            //spawn ghosts
            foreach (Transform child in wolfSpawnPoints)
            {
                Instantiate(wolfObject, child);
            }
            foreach (Transform child in snakeSpawnPoints)
            {
                Instantiate(snakeObject, child);
            }
            foreach (Transform child in foxSpawnPoints)
            {
                Instantiate(foxObject, child);
            }
        }
        if (sheperdInstance)
        {
            Destroy(sheperdInstance);
        }
        if (enemyInstances != null)
        {
            foreach(GameObject enemy in enemyInstances)
            {
                Destroy(enemy);
            }
        }
    }
<<<<<<< HEAD
    

=======

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

    public void SetPhase(int newPhase)
    {

        switch ((LevelPhase)newPhase)
        {
            case 0:
                
                ClearInstances();
                Phase = LevelPhase.Sheep;
                lambInstance = Instantiate(lambPlayerObject, lambSpawnPoint);
                lambInstance.GetComponent<LambController>().ManagerInstance = GetComponent<Manager>();
                break;

            case LevelPhase.Ghost:

                ClearInstances();
                
                
                if(moveHistoryClone != null)
                {
                    MoveHistory = moveHistoryClone;
                }
                

                Phase = LevelPhase.Ghost;
                sheperdInstance = Instantiate(sheperdObject, sheperdSpawnPoint);
                lambInstance = Instantiate(lambPassiveObject, lambSpawnPoint);
                lambInstance.GetComponent<FollowMovement>().MoveHistory = MoveHistory;

                //spawn ghosts
                break;

            case LevelPhase.LevelComplete:

                break;
        }
    }

>>>>>>> main
}
