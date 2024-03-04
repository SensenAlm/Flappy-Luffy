using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float _maxTime = 1.5f;
    [SerializeField] private float _heightRange = 0.45f;
    [SerializeField] private GameObject _pipePrefab;
    [SerializeField] private GameObject _shrinkPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _doublePrefab;
    [SerializeField] private float _shrinkSpawnChance = .01f;
    [SerializeField] private float _shieldSpawnChance = .01f;
    [SerializeField] private float _doubleSpawnChance = .01f;
    private float _timer;
    private bool _shrinkSpawned = false;
    public bool _shieldSpawned = false;
    public bool _doubleSpawned = false;
    public static PipeSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SpawnPipe();
    }

    private void Update()
    {
        if (_timer > _maxTime)
        {
            SpawnPipe();
            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    private void SpawnPipe()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-_heightRange, _heightRange));

        GameObject pipe = Instantiate(_pipePrefab, spawnPos, Quaternion.identity);
        Destroy(pipe, 5f);

        if (Random.value < _shrinkSpawnChance && !_shrinkSpawned)
        {
            Vector3 shrinkSpawnPos = spawnPos + new Vector3(0, 0);
            Instantiate(_shrinkPrefab, shrinkSpawnPos, Quaternion.identity);
            _shrinkSpawned = true;
        }
        if (Random.value < _shieldSpawnChance && !_shieldSpawned)
        {
            Vector3 shieldSpawnPos = spawnPos + new Vector3(0, 0);
            Instantiate(_shieldPrefab, shieldSpawnPos, Quaternion.identity);
            _shieldSpawned = true;

        }
        if (Random.value < _doubleSpawnChance && !_doubleSpawned)
        {
            Vector3 doubleSpawnPos = spawnPos + new Vector3(0, 0);
            Instantiate(_doublePrefab, doubleSpawnPos, Quaternion.identity);
            _doubleSpawned = true;

        }
    }
}
