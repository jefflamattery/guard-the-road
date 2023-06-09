using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGraph : MonoBehaviour
{
    [SerializeField] private List<FieldObserver> _observers;
    [SerializeField] private GameObject _agentContainer;
    [SerializeField] private GameObject _graphBoundary;
    [SerializeField] private Vector2Int _nodeCount;
    [SerializeField] private float _fieldUpdateTime;
    [SerializeField] private bool _visualizeField;
    [SerializeField] private float _graphHeight;

    private Vector2 _nodeDistance;

    private NavNode[][] _graph;
    
    public void AddObserver(FieldObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(FieldObserver observer)
    {
        _observers.Remove(observer);
    }

    private Vector2Int WorldToIndex(Vector3 worldspace)
    {
        // this will calculate the closest index only if you are within the bounds of the Navigation Graph
        // otherwise, it will return an index that is out of bounds of the graph matrix
       Vector2Int index = Vector2Int.zero;
       Vector2 preciseIndex = Vector2.zero;

       worldspace -= Origin();

       preciseIndex.x = worldspace.x / _nodeDistance.x;
       preciseIndex.y = worldspace.z / _nodeDistance.y;
       
       index.x = Mathf.RoundToInt(preciseIndex.x);
       index.y = Mathf.RoundToInt(preciseIndex.y);

       return index;
    }

    private Vector3 IndexToWorld(int i, int j)
    {
        return Origin() + new Vector3(i * _nodeDistance.x, 
                                        0f,
                                        j * _nodeDistance.y);
    }

    private Vector3 Origin()
    {
        return _graphBoundary.transform.position - 0.5f * _graphBoundary.transform.localScale;
    }

    public bool GetNode(Vector3 worldspace, out NavNode node)
    {

        Vector2Int index = WorldToIndex(worldspace);
        NavNode candidate;

        if(index.x >= 0 && index.x < _graph.Length){
            if(index.y >= 0 && index.y < _graph[index.x].Length){

                // the index is in bounds, now check for boundaries
                candidate = _graph[index.x][index.y];

                if(!candidate.inCollider){
                    // only set node if a valid candidate was found, 
                    // otherwise node will remain what it was before this method was called
                    node = candidate;
                    return true;
                } 
            }
        }
        node = null;
        return false;
    }

    IEnumerator CalculateField()
    {

        while(true)
        {

            for(int i = 0; i < _graph.Length; i++){
                for(int j = 0; j < _graph[i].Length; j++){
                    _graph[i][j].PrepareField();
                }
            }

            yield return new WaitForSeconds(_fieldUpdateTime / 2f);


            for(int i = 0; i < _graph.Length; i++){
                for(int j = 0; j < _graph[i].Length; j++){
                    _graph[i][j].UpdateField();
                }
            }



            ObserveField();

            yield return new WaitForSeconds(_fieldUpdateTime / 2f);
        }
        
    }

    void ObserveField()
    {
        NavNode closest;
        // go through the list of observers and update information accordingly
        foreach(FieldObserver observer in _observers){
            
            if(!GetNode(observer.Position, out closest)){
                closest = observer.lastNode;
            }

            if(closest != null){
                // set the charge and measure the field
                closest.charge = observer.Charge;
                observer.ScalarField = closest.scalarField;
                observer.VectorField = closest.vectorField;

                observer.lastNode = closest;
            }
            
        }
    }

    void Update()
    {
        if(_visualizeField){
            for(int i = 0; i < _graph.Length; i++){
                for(int j = 0; j < _graph[i].Length; j++){
                    _graph[i][j].DrawField(_graphHeight);
                }
            }
        }
    }


    void Start()
    {
        // look for any field observers in the scene and keep a reference to them here in the Navigation Graph
        // this prevents the need to connect newly made Scriptable Objects to this Navigation Graph (that would be a pain!)
        IFieldObserver[] fieldComponents = _agentContainer.GetComponentsInChildren<IFieldObserver>();

        foreach(IFieldObserver component in fieldComponents)
        {
            if(!_observers.Contains(component.Field)){
                _observers.Add(component.Field);

                // clear the vector component of the field
                component.Field.VectorField = Vector3.zero;
            }
        }
        
        StartCoroutine(CalculateField());
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

        // determine the horizontal and vertical distance between nodes
        _nodeDistance = new Vector2(_graphBoundary.transform.localScale.x / _nodeCount.x, _graphBoundary.transform.localScale.z / _nodeCount.y);

        // then give every nav node a reference to its neighbors
        BuildNeighbors();

        BuildWorldPositions();
        
    }

    void BuildWorldPositions()
    {
        for(int i = 0; i < _graph.Length; i++){
            for(int j = 0; j < _graph[i].Length; j++){
                _graph[i][j].worldPosition = IndexToWorld(i,j);
                _graph[i][j].BuildBoundaries(_nodeDistance, _graphHeight);
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


