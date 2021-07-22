using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{

    private int pickupCount;
    private int playerHealth;
    private int itemsPicked = 0;
    //Used for the picking of random object for placement randomly on scene. 
    private string[] objects = new string[5];

    public int items
    {
        get { return itemsPicked; }
        set
        {
            itemsPicked = value;
            Debug.LogFormat("Items: {0} ", itemsPicked);
        }
    }


    // Start is called before the first frame update
    int Rand;
    int[] LastRand;
    int Max = 5;

    void Start()
    {
        objects[0] = "Player";
        objects[1] = "Agent";
        objects[2] = "Obj1";
        objects[3] = "Obj2";
        objects[4] = "Obj3";


        LastRand = new int[Max];

        for (int i = 1; i < Max; i++)
        {
            Rand = Random.Range(0, 4);

            for (int j = 1; j < i; j++)
            {
                while (Rand == LastRand[j])
                {
                    Rand = Random.Range(0, 4);
                }
            }
            float x = Random.Range(0.5f, 3.5f);
            float y = Random.Range(1, 2);
            float z = Random.Range(0.5f, 3.5f);
            LastRand[i] = Rand;
            GameObject.FindWithTag(objects[Rand]).transform.position = new Vector3(x, y, z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}