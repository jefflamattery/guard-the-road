using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGraph : MonoBehaviour
{
    [SerializeField] private List<FieldObserver> _observers;
    [SerializeField] private GameObject _entities;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private GameObject _graphBoundary;
    [SerializeField] private Vector2Int _nodeCount;
    [SerializeField] private float _fieldUpdateTime;
    [SerializeField] private bool _visualizeField;

    private NavNode[][] _graph;
    

    private Vector2Int WorldToIndex(Vector3 worldspace)
    {
        // this will calculate the closest index only if you are within the bounds of the Navigation Graph
        // otherwise, it will return an index that is out of bounds of the graph matrix
       Vector2Int index = Vector2Int.zero;
       worldspace -= Origin();

       index.x = Mathf.RoundToInt(worldspace.x * _nodeCount.x / _graphBoundary.transform.localScale.x);
       index.y = Mathf.RoundToInt(worldspace.z * _nodeCount.y / _graphBoundary.transform.localScale.z);

       return index;
    }

    private Vector3 IndexToWorld(int i, int j)
    {
        return Origin() + new Vector3(i * _graphBoundary.transform.localScale.x / _nodeCount.x, 
                                        0f,
                                        j * _graphBoundary.transform.localScale.z / _nodeCount.y);
    }

    private Vector3 Origin()
    {
        return _graphBoundary.transform.position - 0.5f * _graphBoundary.transform.localScale;
    }

    public NavNode GetNode(Vector3 worldspace)
    {
        Vector2Int index = WorldToIndex(worldspace);

        if(index.x >= 0 && index.x < _graph.Length){
            if(index.y >= 0 && index.y < _graph[index.x].Length){
                return _graph[index.x][index.y];
            }
        }

        return null;
    }

    IEnumerator CalculateField(float period)
    {

        while(true)
        {
            for(int i = 0; i < _graph.Length; i++){
                for(int j = 0; j < _graph[i].Length; j++){
                    _graph[i][j].PrepareField();
                }
            }

            yield return new WaitForSeconds(period / 2f);


            for(int i = 0; i < _graph.Length; i++){
                for(int j = 0; j < _graph[i].Length; j++){
                    _graph[i][j].UpdateField();
                }
            }

            ObserveField();

            yield return new WaitForSeconds(period / 2f);
        }
        
    }

    void ObserveField()
    {
        NavNode closest;
        // go through the list of observers and update information accordingly
        foreach(FieldObserver observer in _observers){
            closest = GetNode(observer.Position);

            if(closest != null)
            {
                // set the scalar field and measure the vector field
                observer.VectorField = closest.vectorField;
                if(observer.CanChangeScalarField){
                    closest.UpdateField(observer.ScalarField);
                } else {
                    observer.ScalarField = closest.scalarField;
                }
            }
        }
    }


    void Start()
    {
        // look for any field observers in the scene and keep a reference to them here in the Navigation Graph
        // this prevents the need to connect newly made Scriptable Objects to this Navigation Graph (that would be a pain!)
        IFieldObserver[] fieldComponents = _entities.GetComponentsInChildren<IFieldObserver>();

        foreach(IFieldObserver component in fieldComponents)
        {
            if(!_observers.Contains(component.Field)){
                _observers.Add(component.Field);

                // clear the vector component of the field
                component.Field.VectorField = Vector3.zero;
            }
        }
        
        StartCoroutine(CalculateField(_fieldUpdateTime));
    }

    void Awake()
    {
        // build the node graph
        _graph = new NavNode[_nodeCount.x][];

        for(int i = 0; i < _nodeCount.x; i++){
            _graph[i] = new NavNode[_nodeCount.y];
        }

        // create new nav nodes recursively and insert them into the graph
        new NavNode(ref _graph);

        // then give every nav node a reference to its neighbors
        BuildNeighbors();

        BuildWorldPositions();

        BuildNeighborDisplacements();

        
    }

    void BuildNeighborDisplacements()
    {

        for(int i = 0; i < _graph.Length; i++){
            for(int j = 0; j < _graph[i].Length; j++){
                _graph[i][j].CalculateNeigborDisplacements();

            }
        }
    }

    void BuildWorldPositions()
    {

        for(int i = 0; i < _graph.Length; i++){
            for(int j = 0; j < _graph[i].Length; j++){
                _graph[i][j].CreateWorldspaceNode(GameObject.Instantiate(_nodePrefab, IndexToWorld(i,j), Quaternion.identity));
                _graph[i][j].VisualizeField(_visualizeField);
            }
        }
    }

    void BuildNeighbors()
    {
        // go to every node on the graph and assign its neighbors
        for(int i = 0; i < _graph.Length; i++){
            for(int j = 0; j < _graph[i].Length; j++){

                // East
                if(i + 1 < _graph.Length){
                    _graph[i][j].neighbors[NavNode.EAST] = _graph[i + 1][j];
                    _graph[i][j].isBoundary[NavNode.EAST] = false;
                } else {
                    _graph[i][j].isBoundary[NavNode.EAST] = true;
                }

                // North
                if(j + 1 < _graph[i].Length){
                    _graph[i][j].neighbors[NavNode.NORTH] = _graph[i][j + 1];
                    _graph[i][j].isBoundary[NavNode.NORTH] = false;
                } else {
                    _graph[i][j].isBoundary[NavNode.NORTH] = true;
                }

                // West
                if(i > 0){
                    _graph[i][j].neighbors[NavNode.WEST] = _graph[i - 1][j];
                    _graph[i][j].isBoundary[NavNode.WEST] = false;
                } else {
                    _graph[i][j].isBoundary[NavNode.WEST] = true;
                }

                // South
                if(j > 0){
                    _graph[i][j].neighbors[NavNode.SOUTH] = _graph[i][j - 1];
                    _graph[i][j].isBoundary[NavNode.SOUTH] = false;
                } else {
                    _graph[i][j].isBoundary[NavNode.SOUTH] = true;
                }

            }
        }
    }
}


