using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public enum LevelPhase { Sheep, Ghost }

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

    private GameObject lambInstance;
    private GameObject sheperdInstance;

    public LevelPhase Phase { get; private set; }
    public Queue<(Vector2 pos, float time)> MoveHistory { protected get; set; }

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

    public void ChangePhase()
    {
        if(Phase == LevelPhase.Sheep)
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
        else
        {
            //Complete Level
        }
    }
    
    public void gameOver()
    {
        Debug.Log("Game Over");
    }
}
