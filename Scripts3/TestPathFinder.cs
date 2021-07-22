using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinder : MonoBehaviour
{
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray1, pathArray2, pathArray3;

    GameObject objStartCube, objEndCube, Agent;

    private float elapsedTime = 0.0f;
    public float intervalTime = 2.0f; //Interval time between path finding

    // Use this for initialization
    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        pathArray1 = new ArrayList();
        pathArray2 = new ArrayList();
        pathArray3 = new ArrayList();

        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
            int i = 0;
            /*
            while(true)
            {
                try
                {
                    Agent.transform.position = Vector3.MoveTowards(x, y, z);
                    i++;
                }
                catch
                {
                    break;
                }
            }
            */        }
        
    }

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        //Assign StartNode and Goal Node
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        pathArray1 = PathFinder.DFS(startNode, goalNode);

        CheckNode();

    }

    void OnDrawGizmos()
    {
        if (pathArray1 == null)
            return;

        if (pathArray1.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray1)
            {
                if (index < pathArray1.Count)
                {
                    Node nextNode = (Node)pathArray1[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);

                    index++;
                }
            };
        }

        
    }

    void CheckNode()
    {
        if (pathArray1.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray1)
            {
                if (index < pathArray1.Count)
                {
                    OnDrawGizmos();
                    Node nextNode = (Node)pathArray1[index];
                    Debug.Log("DFS now visiting index " + index + " " + node.position);

                    index++;
                }                
            };
            Debug.Log("DFS reached goal in " + index + "steps");
        }

        if (pathArray2.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray2)
            {
                if (index < pathArray2.Count)
                {
                    Node nextNode = (Node)pathArray2[index];
                    Debug.Log("BFS now visiting index " + index + " " + node.position);

                    index++;
                }
            };
            Debug.Log("BFS reached goal in " + index + "steps");
        }


        if (pathArray3.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray3)
            {
                if (index < pathArray3.Count)
                {
                    Node nextNode = (Node)pathArray3[index];
                    Debug.Log("AStar now visiting index " + index + " " + node.position);

                    index++;
                }
            };
            Debug.Log("DFS reached goal in " + index + "steps");
        }

    }

}
