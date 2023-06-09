using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode
{
    public static float VECTOR_PRECISION = 0.000000001f;
    public static int NAVIGATION_LAYER = 3;
    public static Vector3[] UNIT_VECTORS = {Vector3.right, Vector3.forward, Vector3.left, Vector3.back};
    public static int EAST = 0;
    public static int NORTH = 1;
    public static int WEST = 2;
    public static int SOUTH = 3;
    public Vector3 worldPosition;
    public NavNode[] neighbors;
    public bool[] isBoundary;
    public bool inCollider = false;
    private int _nNeighbors = 0;

    public float scalarField = 0f;
    public Vector3 vectorField = Vector3.zero;

    public float nextScalarField = 0f;
    public Vector3 nextVectorField;

    public float charge;

    public void BuildBoundaries(Vector2 nodeDistance, float graphHeight)
    {
        // Send rays in all 4 directions, if the ray encounters a collider that is closer than the node distance
        // then there is a boundary in that direction

        float maxDistance;
        int navigationLayer = LayerMask.GetMask("Navigation");

        Vector3 localOrigin = worldPosition + graphHeight * Vector3.up;


        for(int direction = 0; direction < 4; direction++){

            if(direction == 0 || direction == 2){
                maxDistance = nodeDistance.x;
            } else {
                maxDistance = nodeDistance.y;
            }

            if(Physics.Raycast(localOrigin, UNIT_VECTORS[direction], maxDistance, navigationLayer)){
                isBoundary[direction] = true;
            } else if(Physics.Raycast(localOrigin + UNIT_VECTORS[direction], -UNIT_VECTORS[direction], maxDistance, navigationLayer)){
                isBoundary[direction] = true;
                inCollider = true;
            } else {
                isBoundary[direction] = false;
                _nNeighbors++;
            }

        }
        
    }

    public void PrepareField()
    {
        
        float[] neighborhoodField = new float[4];
        float neighborAverage = 0f;
        Vector3 gradient = Vector3.zero;

        
        if(_nNeighbors > 0){

            for(int direction = 0; direction < 4; direction++){
               
                if(isBoundary[direction] || neighbors[direction] == null){
                    // BOUNDARY CONDITION directly implemented (choose one)
                    
                    // the partial derivative at the boundary is 0
                    //neighborhoodField[direction] = neighborhoodField[(direction + 2) % 4];
                    
                    // the field at the boundary is 0
                    neighborhoodField[direction] = 0f;

                } else {
                    // this neighbor does exist
                    neighborhoodField[direction] = neighbors[direction].scalarField;
                    neighborAverage += neighborhoodField[direction];
                }
            }

            nextScalarField = (charge + neighborAverage) / (_nNeighbors + 1);
            

            // calculate the gradient of the scalar field as well (this will create the vector field)
            gradient.x = neighborhoodField[EAST] - neighborhoodField[WEST];
            gradient.z = neighborhoodField[NORTH] - neighborhoodField[SOUTH];

            if(gradient.sqrMagnitude >= VECTOR_PRECISION){
                nextVectorField = gradient;
            } else {
                nextVectorField = vectorField;
            }

        }
    }

    public void DrawField(float visualizationHeight)
    {
        Vector3 localOrigin = worldPosition + visualizationHeight * Vector3.up;

        Debug.DrawLine(localOrigin, localOrigin + scalarField * Vector3.up, Color.green);

        if(vectorField.magnitude > 0f)
        {
            Debug.DrawLine(localOrigin, localOrigin + vectorField / (2f * vectorField.magnitude), Color.blue);
        }

        // draw a line toward each neighbor
        
        for(int direction = 0; direction < 4; direction++){
            if(isBoundary[direction]){
                Debug.DrawLine(localOrigin, localOrigin + 0.5f * UNIT_VECTORS[direction], Color.red);
            } else {
                Debug.DrawLine(localOrigin, localOrigin + 0.5f * UNIT_VECTORS[direction], Color.white);
            }
        }
        

        

       
    }

    public void UpdateField()
    {
        scalarField = nextScalarField;
        vectorField = nextVectorField;

        charge = 0f;    // charge will be reset every time by the NavigationGraph
        nextScalarField = 0f;
        nextVectorField = Vector3.zero;
    }

    public void UpdateField(float forcedValue)
    {
        nextScalarField = forcedValue;
        UpdateField();
    }

    public NavNode(ref NavNode[][] graph)
    : this(ref graph, Vector2Int.zero){}

    public NavNode(ref NavNode[][] graph, Vector2Int position)
    {
        neighbors = new NavNode[4];
        isBoundary = new bool[4];
        charge = 0f;
        nextVectorField = Vector3.zero;
        scalarField = 0f;
        nextScalarField = 0f;
        _nNeighbors = 0;

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




}