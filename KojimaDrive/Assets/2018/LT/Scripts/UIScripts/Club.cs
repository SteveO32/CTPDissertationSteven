using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour {

    
    public enum ClubType
    {
        Driver,
        Wedge,
        Putter
    }

    public enum PowerUpType
    {
        Grav, 
        Shell,
        Ghost,
        None
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(10.0f);
        
        powerUp = PowerUpType.None;
        
        yield return null;
    }

    public GameObject ball;
    public Transform parent;
    public Animator swing;

    public ParticleSystem explosion;
    public Material ghost;
    public Material norm;

    public Transform hole;
    public Vector3 tempHole;

    public bool ghostPU;

    
    public PowerUpType powerUp;

    [SerializeField]
    public float power;
    
    
	// Use this for initialization
	void Start () {
		//ball = GameObject.FindGameObjectWithTag ("GolfBall");

        parent = gameObject.transform.root;

        //ball = parent.Find("GolfBall(Clone)").gameObject;
        power = 100;

       // golfClub = ClubType.Driver;

        powerUp = PowerUpType.None;
	}

    public void SetSwinger(GameObject swinger)
    {
        swing = swinger.GetComponentInChildren<Animator>();
    }

	// Update is called once per frame
	void Update () {
        

    }

    void SwitchClub()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    golfClub = ClubType.Driver;
        //    Debug.Log("Driver");
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    golfClub = ClubType.Wedge;
        //    Debug.Log("Wedge");
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    golfClub = ClubType.Putter;
        //    Debug.Log("Putter");
        //}

        //switch (golfClub)
        //{
        //    case ClubType.Driver:
        //        {
        //            power = 100;
        //            break;
        //        }
        //    case ClubType.Wedge:
        //        {
        //            power = 20;
        //            break;
        //        }
        //    case ClubType.Putter:
        //        {
        //            power = 10;
        //            break;
        //        }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            powerUp = PowerUpType.Ghost;

        }
        else if (other.gameObject.tag == "Shell")
        {
            powerUp = PowerUpType.Shell;

        }
        else if (other.gameObject.tag == "Grav")
        {
            powerUp = PowerUpType.Grav;
            power = 650;
        }
    }

    Vector3 GetReflected()
	{
		//Get vector of ball and club, cross product it, 
		Vector3 ballVector  = transform.position - ball.transform.position;/*equals club vector minus ball vector */;

		//Need the tangent of the plane....

		//Vector3 planeTangent = Vector3.Cross(ballVector, direction of camera);
		Vector3 planeTangent = Vector3.Cross(ballVector, Camera.main.transform.forward);

		Vector3 planeNormal = Vector3.Cross (planeTangent, ballVector);

		//Normal of plane Vector3.Cross(planeTangent, ballVector);
		//Vector3 ReflectedVector Vector3.Reflect(camera forward, planeNormal);
		Vector3 ReflectedVector = Vector3.Reflect(Camera.main.transform.forward, planeNormal);

		return ReflectedVector.normalized;

	}


    void OnCollisionEnter(Collision collision)
    {
        //other.transform.parent.gameObject = ball.transform.parent.gameObject;
        //ball = other.transform.parent.gameObject;
        if (collision.gameObject.tag == "GolfBall" && swing.GetCurrentAnimatorStateInfo(1).IsName("Swing"))
        {
            if (powerUp == PowerUpType.Grav)
            {
                Instantiate(explosion, transform.position, transform.rotation);

                DestroyImmediate(explosion);
                power = 650;
            }
            else if (powerUp == PowerUpType.Ghost)
            {
                collision.gameObject.GetComponent<Renderer>().material = ghost;
                
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

                StartCoroutine(timer());
            }

            if (powerUp == PowerUpType.Shell)
            {
                Vector3 holeForce = new Vector3(hole.position.x, hole.position.y + 50.0f, hole.position.z) - collision.rigidbody.worldCenterOfMass;
                holeForce.Normalize();
                power = 250;
                holeForce *= power * 1000;
                collision.rigidbody.AddForce(holeForce);
               
                powerUp = PowerUpType.None;
            }
            else
            {
                
            }
            Vector3 force = collision.rigidbody.worldCenterOfMass - swing.GetComponentInParent<Collider>().bounds.center;
            force.Normalize();
            force *= power * 1000;
            collision.rigidbody.AddForce(force);

        }

        if (collision.gameObject.tag == "Player" && swing.GetCurrentAnimatorStateInfo(1).IsName("Swing"))
        {
            Vector3 force = collision.rigidbody.worldCenterOfMass - swing.GetComponentInParent<Collider>().bounds.center;
            force.y += 1.0f;
            force.Normalize();
            force *= power * 200;
            if (!collision.rigidbody.GetComponentInParent<AnimFollow.RagdollControl_AF>().gettingUp)
                collision.rigidbody.GetComponentInParent<AnimFollow.RagdollControl_AF>().falling = true;
            collision.rigidbody.AddForce(force);
        }

        

    }
}
