using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode
{
    public static float DECAY_RATE = 0.01f;
    public static int EAST = 0;
    public static int NORTH = 1;
    public static int WEST = 2;
    public static int SOUTH = 3;
    private GameObject _worldspaceNode;
    private MeshRenderer _nodeRenderer;
    public NavNode[] neighbors;
    public float [] displacements;
    public bool[] isBoundary;

    public float scalarField = 0f;
    public Vector3 vectorField = Vector3.zero;

    private float _nextScalarField = 0f;
    private Vector3 _nextVectorField = Vector3.zero;

    private Color _originalColor;

    public void CreateWorldspaceNode(GameObject nodeObject)
    {
        _worldspaceNode = nodeObject;

        _nodeRenderer = nodeObject.GetComponent<MeshRenderer>();
        _originalColor = _nodeRenderer.material.color;
    }

    public void VisualizeField(bool showVisualization)
    {
        if(showVisualization){
            _nodeRenderer.enabled = true;
        } else {
            _nodeRenderer.enabled = false;
        }
    }

    public void PrepareField()
    {
        // the scalar value is the weighted average of the neighbors
        _nextScalarField = 0f;
        float[] neighborhoodField = new float[4];


        for(int direction = 0; direction < 4; direction++){
            if(isBoundary[direction]){
                // BOUNDARY CONDITION directly implemented
                neighborhoodField[direction] = 0f;
            } else {
               neighborhoodField[direction] = neighbors[direction].scalarField;
            }
        }

        // average the neighborhood field to set value of this node's scalar field value
        
        _nextScalarField = (neighborhoodField[EAST] +
                            neighborhoodField[NORTH] + 
                            neighborhoodField[WEST] + 
                            neighborhoodField[SOUTH]) / 4f;

        // decay the field (bring it closer to 0)
        _nextScalarField *= (1f - DECAY_RATE);
        

        

        // calculate the gradient of the scalar field as well (this will create the vector field)
        _nextVectorField.x = ( neighborhoodField[EAST] - neighborhoodField[WEST] ) / ( displacements[EAST] + displacements[WEST] );
        _nextVectorField.z = ( neighborhoodField[NORTH] - neighborhoodField[SOUTH] ) / ( displacements[NORTH] + displacements[SOUTH] );
        
    }

    public void UpdateField()
    {
        scalarField = _nextScalarField;
        vectorField = _nextVectorField;

        //_nodeRenderer.material.color = Vector3.Dot(vectorField, referenceFrame) * _originalColor;
        _nodeRenderer.material.color = scalarField * _originalColor;
        
    }

    public void UpdateField(float forcedValue)
    {
        _nextScalarField = forcedValue;
        UpdateField();
    }

    public NavNode(ref NavNode[][] graph)
    : this(ref graph, Vector2Int.zero){}

    public NavNode(ref NavNode[][] graph, Vector2Int position)
    {
        neighbors = new NavNode[4];
        isBoundary = new bool[4];
        displacements = new float[4];

        // assign this NavNode to its proper place in the graph
        graph[position.x][position.y] = this;

        // all nodes create a new node above it (if able)
        // but only nodes at y = 0 create a new node to the right
        if(position.y == 0){
            // create a new node to the right
            if(position.x + 1 < graph.Length){
                new NavNode(ref graph, position + Vector2Int.right);
            }
        }

        // try to add a node upwards
        position += Vector2Int.up;
        if(position.y < graph[position.x].Length){
            new NavNode(ref graph, position);
        }

    }

    public void SetWorldPosition(Vector3 position)
    {
        _worldspaceNode.transform.position = position;
    }

    public void CalculateNeigborDisplacements()
    {
        for(int direction = 0; direction < 4; direction++){
            if(!isBoundary[direction]){
                displacements[direction] = (neighbors[direction]._worldspaceNode.transform.position - 
                                            _worldspaceNode.transform.position).magnitude;
            } else {
                displacements[direction] = 0f;
            }
        }
    }



}