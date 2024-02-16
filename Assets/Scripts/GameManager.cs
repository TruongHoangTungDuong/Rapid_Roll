using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject safeBar;
    [SerializeField] GameObject dangerousBar;
    public GameObject newestSafeBar;
    private int poolSize;
    private List<GameObject> safeBarPool;
    private List<GameObject> dangerousBarPool;
    public GameObject playerPrefab;
    public GameObject foodPrefab;
    private bool spawnPlayerCalled = false;
    private bool isEnd = false;
    private GameObject m_player;
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 25f;
    private float spawnInterval;
    public float spawnChance = 0.5f;
    void Start()
    {
        poolSize = 5;
        InitializeObjectPool();
        StartCoroutine(Spawn());
        StartCoroutine(SpawnFoodCoroutine());
        GameObject activeObject = safeBarPool.Find(obj => obj.activeInHierarchy);
        if (activeObject != null)
        {
            SpawnPlayerAtFirstObjectPosition(activeObject.transform.position);
        }
    }

    void InitializeObjectPool()
    {
        safeBarPool = new List<GameObject>();
        dangerousBarPool = new List<GameObject>();
        newestSafeBar = null;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject safeBarObj = Instantiate(safeBar);
            safeBarObj.SetActive(false);
            safeBarPool.Add(safeBarObj);
            GameObject dangerousBarObj = Instantiate(dangerousBar);
            dangerousBarObj.SetActive(false);
            dangerousBarPool.Add(dangerousBarObj);
        }
    }
    GameObject GetBarFromPool(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        GameObject newObj = Instantiate(pool[0]);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
    IEnumerator Spawn()
    {
        while (!isEnd)
        {
            if (Random.value < 0.2f)
            {
                GameObject dangerousBar = GetBarFromPool(dangerousBarPool);
                yield return new WaitForSeconds(1.5f);
                dangerousBar.transform.position = new Vector2(Random.Range(-4f, 4f), -5f);
                dangerousBar.SetActive(true);
            }
            else
            {
                GameObject safeBarObj = GetBarFromPool(safeBarPool);
                Vector2 spawnpos = new Vector2(Random.Range(-4, 4), -5);
                yield return new WaitForSeconds(1.5f);
                safeBarObj.transform.position = spawnpos;
                safeBarObj.SetActive(true);
                newestSafeBar = safeBarObj;
                if (!spawnPlayerCalled && safeBarObj.activeInHierarchy)
                {
                    SpawnPlayerAtFirstObjectPosition(new Vector2(safeBarObj.transform.position.x, safeBarObj.transform.position.y + 1));
                    spawnPlayerCalled = true;
                }
            }
        }
    }
    void SpawnPlayerAtFirstObjectPosition(Vector3 spawnPosition)
    {
        m_player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
    IEnumerator SpawnFoodCoroutine()
    {
        while (true)
        {
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
            Vector3 spawnPosition = new Vector2( newestSafeBar.transform.position.x, newestSafeBar.transform.position.y+1);
            GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void Update()
    {
        if (m_player != null)
        {
            Player player = m_player.GetComponent<Player>();
            if (player != null && player.cur_health == 0)
            {
                isEnd = true;
                Destroy(player);
                UIManager.Instance.audioList[2].Play();
                UIManager.Instance.audioBackground.Stop();
                UIManager.Instance.ShowGameOverPanel();
                Time.timeScale = 0f;
            }
        }
    }
}
