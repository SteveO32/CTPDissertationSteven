using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:		Harry McAlpine
// Purpose:		Temporary fix for player controller
// Namespace:	TF
//
//===============================================================================//
public class PlayerController : MonoBehaviour
{
    Rewired.Player player;

    [SerializeField]
    string movementHorizontalString = "Move Horizontal";

    [SerializeField]
    string movementVerticalString = "Move Vertical";

    [SerializeField]
    Vector2 movementVector;

    public int playerNumber = 0;
    public float speed = 1.0f;
    private Rigidbody rb;
    private bool initialized;
    Vector3 movement;

    [Header("Knockback")]
    [SerializeField]
    public float knockbackForce = 20f;
    public float extraYKnockback = 50f;
    public float maxYKnockback = 1000.0f;
    public float knockbackModifier = 1.0f;
    public bool knockedBack = false;
    public float knockedDownTime = 2.0f;

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            Initialize();
        }

        movementVector.x = player.GetAxis(movementHorizontalString);
        movementVector.y = player.GetAxis(movementVerticalString);

        MoveCharacter(movementVector.x, movementVector.y);
    }

    void Initialize()
    {
        player = Rewired.ReInput.players.GetPlayer(playerNumber);
        rb = GetComponent<Rigidbody>();
        initialized = true;
    }

    void MoveCharacter(float horizontal, float vertical)
    {
        transform.Rotate(0, horizontal, 0);
       if(movementVector.y > 0)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (movementVector.y < 0)
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "FroggerCar")// && knockedBack == false)
        {
            Debug.Log("Car hit player");

            rb.velocity = Vector3.zero;                         // If there are multiple collisions, this prevents the forces from stacking
            knockedBack = true;            
            rb.constraints = RigidbodyConstraints.None;         // Let the player "ragdoll"
            Vector3 direction = (transform.position- other.gameObject.transform.position).normalized * knockbackModifier;
            direction.y += extraYKnockback;                     // Makes sure the player flies up in the air a lot
            Vector3 totalKnockback = (direction * knockbackForce + other.gameObject.GetComponent<Rigidbody>().velocity.normalized);// * Time.deltaTime);
            if (totalKnockback.y >= maxYKnockback)
            {
                totalKnockback = new Vector3(totalKnockback.x, maxYKnockback, totalKnockback.z);
            }
            Debug.Log("Total Force: " + totalKnockback.ToString());
            rb.AddForce(totalKnockback);

        }
        else if((other.gameObject.tag != "FroggerCar" || other.gameObject.tag != "Player") && knockedBack == true)
        {
            knockedBack = false;
            StartCoroutine(GetUpAgain());
        }
    }

    IEnumerator GetUpAgain()
    {
        Debug.Log("Player get back up after falling");
        
        yield return new WaitForSeconds(knockedDownTime);
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);  //moves the player up slightly so they don't clip into the ground
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);                                         //stands the player back up
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        yield break;
    }
}