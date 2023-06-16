using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnObserver _spawnObserver;
    [SerializeField] private Vector2 _spawnRadiusBoundary;
    [SerializeField] private GameObject _agent;
    [SerializeField] private NavigationGraph _navGraph;

    private List<GameObject> _spawnedAgents;

    void Awake()
    {
        // the _agent is a template for the type of mob to spawn
        // ensure tha the _agent itself is disabled
        _agent.SetActive(false);
        _spawnObserver.nSpawned = 0;
        _spawnedAgents = new List<GameObject>();
    }

    void Update()
    {
        if(_spawnObserver.isSpawned){
            if(_spawnObserver.spawnTarget != _spawnObserver.nSpawned){
                Spawn();
            }
        } else {
            if(_spawnObserver.nSpawned > 0){
                // remove all of the agents that have been spawned by this spawner
                RemoveSpawnedAgents();
            }
        }
    }

    void RemoveSpawnedAgents()
    {
        foreach(GameObject spawned in _spawnedAgents)
        {
            GameObject.Destroy(spawned);
        }

        _spawnObserver.nSpawned = 0;
    }

    public void Spawn()
    {
        ObserverManager spawnObserverManager, agentObserverManager;
        Vector3 spawnPosition;
        float angle, radius;
        GameObject spawned;
        int nSpawns = _spawnObserver.spawnTarget - _spawnObserver.nSpawned;



        for(int i = 0; i < nSpawns; i++){
            angle = Random.Range(0, 359);
            radius = Random.Range(_spawnRadiusBoundary.x, _spawnRadiusBoundary.y);
            spawnPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

            spawned = GameObject.Instantiate(_agent, spawnPosition, Quaternion.identity);
            spawned.SetActive(true);
            _spawnedAgents.Add(spawned);
            
            _spawnObserver.nSpawned++;

            spawnObserverManager = spawned.GetComponent<ObserverManager>();
            agentObserverManager = _agent.GetComponent<ObserverManager>();

            spawnObserverManager.CopyFrom(agentObserverManager);
            spawnObserverManager.InjectObservers();

            _navGraph.AddObserver(spawnObserverManager.Field);

        }
        

    }
}
