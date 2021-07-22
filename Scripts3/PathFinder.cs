using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    #region List fields

    //public static PriorityQueue openList;
    public static HashSet<Node> closedList;

    #endregion

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    /// <summary>
    /// Calculate the estimated Heuristic cost to the goal
    /// </summary>
    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }

    




    public static ArrayList DFS(Node start, Node goal)
    {
        //Start Finding the path
        Stack openList = new Stack(); //change
        openList.Push(start);
        //  start.nodeTotalCost = 0.0f;
        //  start.estimatedCost = HeuristicEstimateCost(start, goal);
        closedList = new HashSet<Node>();
        //closedList = new HashSet<Node>();
        Node node = null;

        // while (openList.Length != 0)

        while (openList.Count != 0)
        {
            node = (Node)openList.Pop();

            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }

            ArrayList neighbours = new ArrayList();
            GridManager.instance.GetNeighbours(node, neighbours);

            #region CheckNeighbours

            //Get the Neighbours
            for (int i = 0; i < neighbours.Count; i++)
            {
                //Cost between neighbour nodes
                Node neighbourNode = (Node)neighbours[i];

                if (!closedList.Contains(neighbourNode))
                {
                    //Cost from current node to this neighbour node
                    /*  float cost = HeuristicEstimateCost(node, neighbourNode);

                      //Total Cost So Far from start to this neighbour node
                      float totalCost = node.nodeTotalCost + cost;

                      //Estimated cost for neighbour node to the goal
                      float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                      //Assign neighbour node properties
                      neighbourNode.nodeTotalCost = totalCost;
                      neighbourNode.parent = node;
                      neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;*/


                    neighbourNode.parent = node;

                    //Add the neighbour node to the list if not already existed in the list
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }

            #endregion
            closedList.Add(node);
            //openList.Pop(node);
        }

        //If finished looping and cannot find the goal then return null
        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        //Calculate the path based on the final node
        return CalculatePath(node);
    }



    
}
