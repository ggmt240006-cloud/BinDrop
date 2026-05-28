// TrashSpawner.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashSpawner : MonoBehaviour
{
    public static TrashSpawner Instance { get; private set; }

    [Header("Spawn Settings")]
    public RectTransform spawnAreaRect;   // Canvas 상단 영역 RectTransform
    public GameObject[] generalPrefabs;   // 일반 쓰레기 프리팹들
    public GameObject[] plasticPrefabs;   // 플라스틱
    public GameObject[] paperPrefabs;     // 종이
    public GameObject[] glassPrefabs;     // 유리

    [Header("Timing")]
    public float baseInterval = 2f;       // 기본 스폰 간격
    public float minInterval = 0.5f;      // 최소 간격

    private List<GameObject[]> _activeTypes = new List<GameObject[]>();
    private float _currentInterval;
    private Coroutine _spawnRoutine;

    [Header("Heart Item")]
    public GameObject heartPrefab;        // 하트 프리팹
    public float heartSpawnChance = 0.1f; // 10% 확률
    public float heartSpawnInterval = 15f; // 최소 15초마다 등장 가능

    private float _heartTimer = 0f;
    private List<GameObject> _spawnedObjects = new List<GameObject>();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _currentInterval = baseInterval;
        UpdateSpawnTypes(1);
    }

    // 레벨에 따라 스폰 타입 갱신
    public void UpdateSpawnTypes(int level)
    {
        _activeTypes.Clear();
        _activeTypes.Add(generalPrefabs);
        if (level >= 2) _activeTypes.Add(plasticPrefabs);
        if (level >= 3) _activeTypes.Add(paperPrefabs);
        if (level >= 4) _activeTypes.Add(glassPrefabs);

        // 레벨 오를수록 간격 단축
        _currentInterval = Mathf.Max(minInterval, baseInterval - (level - 1) * 0.3f);

        if (_spawnRoutine != null) StopCoroutine(_spawnRoutine);
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_currentInterval);

            _heartTimer += _currentInterval;

            // 하트 스폰 체크
            if (_heartTimer >= heartSpawnInterval && Random.value < heartSpawnChance)
            {
                SpawnHeart();
                _heartTimer = 0f;
            }
            else
            {
                SpawnTrash();
            }
        }
    }

    void SpawnHeart()
    {
        if (heartPrefab == null) return;

        float canvasWidth = spawnAreaRect.rect.width;
        float randomX = Random.Range(-canvasWidth / 2f, canvasWidth / 2f);
        float spawnY = spawnAreaRect.rect.height / 2f;

        GameObject obj = Instantiate(heartPrefab, spawnAreaRect);
        _spawnedObjects.Add(obj); // 추가
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(randomX, spawnY);
    }

    void SpawnTrash()
    {
        // 랜덤 타입 선택
        int typeIndex = Random.Range(0, _activeTypes.Count);
        GameObject[] pool = _activeTypes[typeIndex];
        GameObject prefab = pool[Random.Range(0, pool.Length)];

        // Canvas 상단 랜덤 X 위치
        float canvasWidth = spawnAreaRect.rect.width;
        float randomX = Random.Range(-canvasWidth / 2f, canvasWidth / 2f);
        float spawnY = spawnAreaRect.rect.height / 2f;

        GameObject obj = Instantiate(prefab, spawnAreaRect);
        _spawnedObjects.Add(obj); // 추가
        obj.SetActive(true);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(randomX, spawnY);

        // TrashType 설정
        TrashItem item = obj.GetComponent<TrashItem>();
        TrashType[] types = { TrashType.General, TrashType.Plastic, TrashType.Paper, TrashType.Glass };
        item.Initialize(types[typeIndex]);
    }

    public void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }


    public void ClearAllSpawnedObjects()
    {
        foreach (var obj in _spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        _spawnedObjects.Clear();
        _heartTimer = 0f;
    }
}