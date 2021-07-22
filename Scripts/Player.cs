using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5.0f;
    float rotSpeed = 70f;
    private float x;
    private float z;
    private float y;
    private float shootRate = 100f;
    private float health = 100f;

    public float jumpSpeed = 5f;    // instantiate the jump force
    private Rigidbody rb;

    private CapsuleCollider col;
    public GameObject bullet;
    public float bulletSpeed = 100f;

    public LayerMask groundLM;
    public float h = 0.1f;

    public GameObject Bullet;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //1: sets rb as a Rigidbody component which will allow us to use physics ect. on this object. 
         col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * speed; //2: gets the input of the Horizontal axis over time and multiplies it by the speed constant and stores it is x, this value will be used for the fowards movement of the rigid body
        z = Input.GetAxis("Vertical") * Time.deltaTime * speed; //3: this does the same as the previous line as far as normalizing the speed, however this time the input is the Vertical axis
        y = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed; //4: this line is for getting the y input (horizontal) of motion to be used for the rotation of the player object. it is also multiplied by deltatime but instead of speed uses rotspeed so the player can rotate at a different speed than it moves fowards

        transform.Translate(Vector3.forward * z);    //5: This line is responsible for applying the fowards force on the player that corresponds with the previous z input
        transform.Rotate(Vector3.up * y); //6: This line is responsible for applying the desired rotation proportional to the y input to the player

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
               rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                
            }
          
        }
     

        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
        
    }

    void FixedUpdate() //called before each internal physics update
    {
        Vector3 rotation = Vector3.up * x; //7: This takes the rotaional value of y from before and makes it into a vector. 
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime); //8: This is resbonsible for determining the angle  of rotation using the rotation vector. This value will be used in the roation of the player
        rb.MovePosition(this.transform.position + this.transform.forward * x * Time.fixedDeltaTime); //9: This line uses the previously set x value to move the ridgidbody rb fowards accordingly. 
        rb.MoveRotation(rb.rotation * angleRot); //10: This line is responsible for rotating the player according to the proper rotation angle value which is found from the rotation vector which is obtained using the value of y. 
    }

   private bool IsGrounded()
    {
        Vector3 CapsuleBottom = new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(col.bounds.center, CapsuleBottom, h, groundLM, QueryTriggerInteraction.Ignore);                                                          
       return false;
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 50;
            Debug.Log("Player health value: " + health);
        }
        if(health <= 0)
        {
            Destroy(gameObject, 1.5f);
        }
        /*if (collision.gameObject.tag == "Pickup")
        {
            itemsCollected += 1;
            Debug.Log("pickup value: " + itemsCollected);
        }*/
    }

    private void ShootBullet()
    {

        GameObject newBullet = Instantiate(Bullet,
            this.transform.position + new Vector3(1, 0, 2),
               this.transform.rotation) as GameObject;
        newBullet.tag = "Bullet";
        Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

        bulletRB.velocity = this.transform.forward * shootRate;

    }

}
