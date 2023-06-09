using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnObserver _spawn;
    [SerializeField] private Vector2 _spawnRadiusBoundary;
    [SerializeField] private GameObject _agent;
    [SerializeField] private NavigationGraph _navGraph;

    private List<GameObject> _spawnedAgents;

    void Awake()
    {
        // the _agent is a template for the type of mob to spawn
        // ensure tha the _agent itself is disabled
        _agent.SetActive(false);
        _spawn.nSpawned = 0;
        _spawnedAgents = new List<GameObject>();
    }

    void Update()
    {
        if(_spawn.isSpawned){
            if(_spawn.spawnTarget != _spawn.nSpawned){
                Spawn();
            }
        } else {
            if(_spawn.nSpawned > 0){
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

        _spawn.nSpawned = 0;
    }

    public void Spawn()
    {
        Vector3 spawnPosition;
        float angle, radius;
        GameObject spawned;
        MotionObserver motionObserver;
        FieldObserver fieldObserver;
        IMotionObserver[] motionComponents;
        IFieldObserver[] fieldComponents;

        int nSpawns = _spawn.spawnTarget - _spawn.nSpawned;


        for(int i = 0; i < nSpawns; i++){
            angle = Random.Range(0, 359);
            radius = Random.Range(_spawnRadiusBoundary.x, _spawnRadiusBoundary.y);
            spawnPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

            spawned = GameObject.Instantiate(_agent, spawnPosition, Quaternion.identity);
            spawned.SetActive(true);
            _spawnedAgents.Add(spawned);


            // create a copy of the MotionObserver and FieldObserver for the agent, and attach them to any script that needs them
            motionObserver = ScriptableObject.CreateInstance<MotionObserver>();
            fieldObserver = ScriptableObject.CreateInstance<FieldObserver>();


            // attach these newly created observers to any component that implements them
            motionComponents = spawned.GetComponentsInChildren<IMotionObserver>();
            fieldComponents = spawned.GetComponentsInChildren<IFieldObserver>();

            foreach(IMotionObserver motionComponent in motionComponents)
            {
                motionComponent.Motion = motionObserver;
            }

            foreach(IFieldObserver fieldComponent in fieldComponents)
            {
                fieldComponent.Field = fieldObserver;
            }

            _navGraph.AddObserver(fieldObserver);

            _spawn.nSpawned++;
        }

    }
}
