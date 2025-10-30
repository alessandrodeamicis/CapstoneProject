using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Singleton")]
    public static LevelManager Instance;

    [SerializeField] private EdgeCollider2D secondAreaLeftEdgeCollider;
    [SerializeField] private EdgeCollider2D thirdAreaLeftEdgeCollider;
    [SerializeField] private GameObject[] firstLevelEnemies;
    [SerializeField] private GameObject[] secondLevelEnemies;
    [SerializeField] private GameObject[] thirdLevelEnemies;
    private enum LEVEL { FIRST, SECOND, THIRD };
    private LEVEL _currentLevel = LEVEL.FIRST;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SpawnNewEnemies();
    }

    void Update()
    {
        CheckEnemiesHealth();
    }

    void CheckEnemiesHealth()
    {
        switch (_currentLevel)
        {
            case LEVEL.FIRST:
                {
                    if (firstLevelEnemies.Length > 0)
                    {
                        bool areEnemiesAlive = firstLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            secondAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.SECOND;
                        }
                    }
                    break;
                }
            case LEVEL.SECOND:
                {
                    if (secondLevelEnemies.Length > 0)
                    {
                        bool areEnemiesAlive = secondLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            thirdAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.THIRD;
                        }
                    }
                    break;
                }
            case LEVEL.THIRD:
                {
                    if (thirdLevelEnemies.Length > 0)
                    {
                        bool areEnemiesAlive = thirdLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            Debug.Log("Livello completato");
                        }
                    }
                    break;
                }
        }
    }

    public void SpawnNewEnemies()
    {
        switch (_currentLevel)
        {
            case LEVEL.FIRST:
                {
                    foreach (GameObject enemy in firstLevelEnemies)
                    {
                        enemy.SetActive(true);
                    }
                    break;
                }
            case LEVEL.SECOND:
                {
                    foreach (GameObject enemy in secondLevelEnemies)
                    {
                        enemy.SetActive(true);
                    }
                    break;
                }
            case LEVEL.THIRD:
                {
                    foreach (GameObject enemy in thirdLevelEnemies)
                    {
                        enemy.SetActive(true);
                    }
                    break;
                }
        }
    }
}
