using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int gridX; 
    public int gridY; 

    public bool IsTower;
    public Vector3 Position; 

    public Node Parent; //The node AStar came from


    public int gCost; //Cost of moving to the next square
    public int hCost; //distance to the goal from this square

    public int FCost { get { return gCost + hCost; } }

    public Node(bool a_IsTower, Vector3 a_Pos, int a_gridX, int a_gridY)
    {
        IsTower = a_IsTower;
        Position = a_Pos;
        gridX = a_gridX;
        gridY = a_gridY;
    }


}
