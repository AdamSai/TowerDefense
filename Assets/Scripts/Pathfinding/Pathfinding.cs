﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    Grid GridReference;//For referencing the grid class
    //public EnemyController enemy;
    //public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to
    public bool foundPath = false;
    private float Counter = 0;
    
    private void Awake()//When the program starts
    {
        GridReference = GameObject.Find("Game Manager").GetComponent<Grid>();//Get a reference to the game manager
        //enemy = GetComponent<EnemyController>();
        foundPath = true;
    }



    public IEnumerator FindPath()
    {
        Counter = 0;
        foundPath = false;
        while (!foundPath)
        {
            Counter += Time.deltaTime;
            if(Counter > 5)
            {
                break;
            }
            var a_StartPos = transform.position;
            var a_TargetPos = TargetPosition.position;
            Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
            Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

            List<Node> OpenList = new List<Node>();//List of nodes for the open list
            HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

            OpenList.Add(StartNode);//Add the starting node to the open list to begin the program
            while (OpenList.Count > 0)//Whilst there is something in the open list
            {
                Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
                for (int i = 0; i < OpenList.Count; i++)//Loop through the open list starting from the second object
                {
                    if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)//If the f cost of that object is less than or equal to the f cost of the current node
                    {
                        CurrentNode = OpenList[i];//Set the current node to that object
                    }
                }
                OpenList.Remove(CurrentNode);//Remove that from the open list
                ClosedList.Add(CurrentNode);//And add it to the closed list

                if (CurrentNode == TargetNode)//If the current node is the same as the target node
                {
                    GetFinalPath(StartNode, TargetNode);//Calculate the final path
                    break;
                }

                foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
                {
                    if (!NeighborNode.IsTower || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                    {
                        continue;//Skip it
                    }
                    int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                    if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                    {
                        NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                        NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                        NeighborNode.Parent = CurrentNode;//Set the parent of the node for retracing steps

                        if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                        {
                            OpenList.Add(NeighborNode);//Add it to the list
                        }
                    }
                }

            }
            yield return null;

        }

    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.Parent;//Move onto its parent node
        }
        foundPath = true;
        FinalPath.Reverse();//Reverse the path to get the correct order
        //GridReference.FinalPath = FinalPath;
        //enemy.i = 0;
        //enemy.FinalPath = FinalPath;//Set the final path

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);//y1-y2

        return ix + iy;//Return the sum
    }
}
