using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFSM : FSM
{
    //List of possible states
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state of agent
    public FSMState curState;

    //current speed of agent
    private float curSpeed;

    //TAgent rotation Speed
    private float curRotSpeed;

    //Bullet object
    public GameObject Bullet;


    //agent is dead?
    private bool bDead;

    //agent health value
    public int health;

    //number of bullets
    public int bulletCount;

    
    new private Rigidbody rigidbody;


    //Initialize the Finite state machine for the agent
    protected override void Initialize()
    {
        curState = FSMState.Patrol;
        curSpeed = 15.0f;
        curRotSpeed = 1.0f;
        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 5.0f;
        health = 100;


        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("wp");

        //Set Random destination point first
        FindNextPoint();

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        // Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();

        if (!playerTransform)
            print("No player found.. add aplayer with Player tag");



    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    /// <summary>
    /// Patrol state
    /// </summary>
    protected void UpdatePatrolState()
    {
        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 10.0f)
        {
            print("Reached to the destination point\ncalculating the next point");
            FindNextPoint();
        }
        
        //if agent is close enough to player, transition to chase state
        else if (Vector3.Distance(transform.position, playerTransform.position) <= 30.0f)
        {
            print("Switched to Chase Position");
            curState = FSMState.Chase;
        }

        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

    }


    /// <summary>
    /// Chase state
    /// </summary>
    protected void UpdateChaseState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

    
        //change to attack state if payer is close enough
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= 20.0f)
        {
            print("Switched to Attack Position");
            curState = FSMState.Attack;
        }

        //return to patrol state if player is too far
        else if (dist >= 30.0f)
        {
            curState = FSMState.Patrol;
        }

        //move Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    //Attack state
    protected void UpdateAttackState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with the player
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= 20.0f && dist < 30.0f)
        {
            //Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //move Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            curState = FSMState.Attack;
        }
        //change to patrol state is the agent is too far from player
        else if (dist >= 30.0f)
        {
            curState = FSMState.Patrol;
        }

        Quaternion thisRotation = Quaternion.LookRotation(destPos - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, thisRotation,
            Time.deltaTime * curRotSpeed);

        //Shoot the bullets
        ShootBullet();
    }

    /// <summary>
    /// Dead state
    /// </summary>
    protected void UpdateDeadState()
    {
        //Show the dead animation with some physics effects
        if (!bDead)
        {
            bDead = true;
            //do something to end the game
            Destroy(gameObject, 1.5f);
        }
    }

    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {


            GameObject newBullet = Instantiate(Bullet,
                this.transform.position + new Vector3(2f, 2f, 0f),
                   this.transform.rotation) as GameObject;
            newBullet.tag = "Bullet";
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.velocity = this.transform.forward * shootRate;

        }
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {
            ;
            health -= 15;
            Debug.Log("health value: " + health);
        }
        if(health <= 0)
        {
            curState = FSMState.Dead;
        }

        //Reduce health
       
        /*if (collision.gameObject.tag == "Pickup")
        {
            itemsCollected += 1;
            Debug.Log("pickup value: " + itemsCollected);
        }*/
    }

    /// <summary>
    /// Find the next semi-random patrol point
    /// </summary>
    protected void FindNextPoint()
    {
        Debug.Log("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;

        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        //Check Range
        //Prevent to decide the random point as the same as before
        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }

  
   // Check whether the next random position is in close range to current position of player

    /// <param name="pos">position to check</param>
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= 50 && zPos <= 50)
            return true;

        return false;
    }

   
}
