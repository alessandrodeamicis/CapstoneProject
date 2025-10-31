using SGM;
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
    [SerializeField] private EdgeCollider2D fourthAreaLeftEdgeCollider;
    [SerializeField] private EdgeCollider2D fifthAreaLeftEdgeCollider;
    [SerializeField] private GameObject firstLevelEnemiesPrefab;
    [SerializeField] private GameObject secondLevelEnemiesPrefab;
    [SerializeField] private GameObject thirdLevelEnemiesPrefab;
    [SerializeField] private GameObject fourthLevelEnemiesPrefab;
    [SerializeField] private GameObject fifthLevelEnemiesPrefab;
    [SerializeField] private GameObject[] firstLevelEnemiesSpawnPoints;
    [SerializeField] private GameObject[] secondLevelEnemiesSpawnPoints;
    [SerializeField] private GameObject[] thirdLevelEnemiesSpawnPoints;
    [SerializeField] private GameObject[] fourthLevelEnemiesSpawnPoints;
    [SerializeField] private GameObject[] fifthLevelEnemiesSpawnPoints;
    [SerializeField] private GameObject firstStepTutorial;
    [SerializeField] private GameObject secondStepTutorial;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _directionArrowUI;
    private bool showingFirstStepTutorial = false;
    private bool showingSecondStepTutorial = false;
    private enum LEVEL { FIRST, SECOND, THIRD, FOURTH, FIFTH };
    private LEVEL _currentLevel = LEVEL.FIRST;
    private List<GameObject> firstLevelEnemies = new List<GameObject>();
    private List<GameObject> secondLevelEnemies = new List<GameObject>();
    private List<GameObject> thirdLevelEnemies = new List<GameObject>();
    private List<GameObject> fourthLevelEnemies = new List<GameObject>();
    private List<GameObject> fifthLevelEnemies = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!S_SaveManager.IsTutorialDone())
        {
            firstStepTutorial.SetActive(true);
            showingFirstStepTutorial = true;
        }
        SpawnNewEnemies();
    }

    void Update()
    {
        if (!S_SaveManager.IsTutorialDone())
        {
            CheckTutorial();
        }
        CheckEnemiesHealth();
    }

    void CheckTutorial()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (showingFirstStepTutorial)
            {
                showingFirstStepTutorial = false;
                firstStepTutorial.SetActive(false);
                showingSecondStepTutorial = true;
                secondStepTutorial.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showingSecondStepTutorial)
            {
                showingSecondStepTutorial = false;
                secondStepTutorial.SetActive(false);
                S_SaveManager.TutorialDone();
            }
        }
    }

    void CheckEnemiesHealth()
    {
        switch (_currentLevel)
        {
            case LEVEL.FIRST:
                {
                    if (firstLevelEnemies.Count > 0)
                    {
                        bool areEnemiesAlive = firstLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            secondAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.SECOND;
                            ShowDirectionArrow(true);
                        }
                    }
                    else
                    {
                        SpawnNewEnemies();
                    }
                    break;
                }
            case LEVEL.SECOND:
                {
                    if (secondLevelEnemies.Count > 0)
                    {
                        bool areEnemiesAlive = secondLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            thirdAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.THIRD;
                            ShowDirectionArrow(true);
                        }
                    }
                    break;
                }
            case LEVEL.THIRD:
                {
                    if (thirdLevelEnemies.Count > 0)
                    {
                        bool areEnemiesAlive = thirdLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            fourthAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.FOURTH;
                            ShowDirectionArrow(true);
                        }
                    }
                    break;
                }
            case LEVEL.FOURTH:
                {
                    if (fourthLevelEnemies.Count > 0)
                    {
                        bool areEnemiesAlive = fourthLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            fifthAreaLeftEdgeCollider.isTrigger = true;
                            _currentLevel = LEVEL.FIFTH;
                            ShowDirectionArrow(true);
                        }
                    }
                    break;
                }
            case LEVEL.FIFTH:
                {
                    if (fifthLevelEnemies.Count > 0)
                    {
                        bool areEnemiesAlive = fifthLevelEnemies.Select(x => x.GetComponent<EnemyLifeController>()).Any(y => y.CurrentHp > 0);
                        if (!areEnemiesAlive)
                        {
                            ShowWinUI();
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
                    foreach (GameObject spawnPoint in firstLevelEnemiesSpawnPoints)
                    {
                        GameObject enemy = Instantiate(firstLevelEnemiesPrefab, spawnPoint.transform.position, Quaternion.identity);
                        firstLevelEnemies.Add(enemy);
                    }
                    break;
                }
            case LEVEL.SECOND:
                {
                    foreach (GameObject spawnPoint in secondLevelEnemiesSpawnPoints)
                    {
                        GameObject enemy = Instantiate(secondLevelEnemiesPrefab, spawnPoint.transform.position, Quaternion.identity);
                        secondLevelEnemies.Add(enemy);
                    }
                    break;
                }
            case LEVEL.THIRD:
                {
                    foreach (GameObject spawnPoint in thirdLevelEnemiesSpawnPoints)
                    {
                        GameObject enemy = Instantiate(thirdLevelEnemiesPrefab, spawnPoint.transform.position, Quaternion.identity);
                        thirdLevelEnemies.Add(enemy);
                    }
                    break;
                }
            case LEVEL.FOURTH:
                {
                    foreach (GameObject spawnPoint in fourthLevelEnemiesSpawnPoints)
                    {
                        GameObject enemy = Instantiate(fourthLevelEnemiesPrefab, spawnPoint.transform.position, Quaternion.identity);
                        fourthLevelEnemies.Add(enemy);
                    }
                    break;
                }
            case LEVEL.FIFTH:
                {
                    foreach (GameObject spawnPoint in fifthLevelEnemiesSpawnPoints)
                    {
                        GameObject enemy = Instantiate(fifthLevelEnemiesPrefab, spawnPoint.transform.position, Quaternion.identity);
                        fifthLevelEnemies.Add(enemy);
                    }
                    break;
                }
        }
    }

    private void ShowDirectionArrow(bool show)
    {
        _directionArrowUI.SetActive(show);
    }

    private void ShowWinUI()
    {
        _winUI.SetActive(true);
    }
}
