using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
   // public Vector3 camDist = new Vector3(1f, 1.5f, -2.6f);
    public Vector3 camDist = new Vector3(2f, 3f, -5f);
    private Transform playerInfo;


    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = playerInfo.TransformPoint(camDist);
        this.transform.LookAt(playerInfo);

    }
}
