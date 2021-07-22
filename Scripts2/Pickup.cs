using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameBehavior GameBehavior;

    void OnCollisionEnter(Collision colision)
    {
        if(colision.gameObject.name == "Player")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Pickup Item Collected!");
        }
    }

    
    }
