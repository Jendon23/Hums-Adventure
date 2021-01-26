using UnityEngine;
using System.Collections;

public class LoftController : Controller
{
	bool isFlying = false;
	bool isHolding = false;
	
	public GameObject[] eyes;
	public int cameraDistance = 20;
	
	public GameObject grabbedObject;
	public Transform hand;
	public LayerMask objects;
	Animator anim;
	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetInput ();
		anim.SetBool ("isFlying", isFlying);
		anim.SetBool ("isHolding", isHolding);
		if(grabbedObject != null)
		{
			grabbedObject.transform.position = hand.position;
			grabbedObject.transform.rotation = hand.rotation;
		}
	}
	void Orientate()
	{
		if(transform.rotation.eulerAngles.z > 0 && transform.rotation.eulerAngles.z <= 180)
		{
//			print ("Adjust Loft's Rotation Down! " + transform.rotation.eulerAngles.z + " is less than 180");
			rigidbody2D.angularVelocity = -45;
		}
		else if(transform.rotation.eulerAngles.z > 180)
		{
//			print ("Adjust Loft's Rotation Up! " + transform.rotation.eulerAngles.z + " is greater than 180");
			rigidbody2D.angularVelocity = 45;
		}
		if(transform.rotation.eulerAngles.z > 355 || transform.rotation.eulerAngles.z < 5)
		{
			transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0);
			rigidbody2D.fixedAngle = true;
		}
	}
	void GetInput()
	{
		if(gameCamera.GetComponent<GameManager>().character == gameObject)
		{
			if(!rigidbody2D.fixedAngle)
				Orientate();
			if(Input.GetButtonDown("Action"))
		//	if(Input.GetMouseButtonDown(1))
				Action();
			MovePlayer(new Vector2(Input.GetAxis ("Horizontal"),Input.GetAxis ("Vertical")));
		}
	}
	void Action()
	{
		print("loftaction");
		if(grabbedObject == null)
		{
			print("noobject");
			Collider2D[] objs = Physics2D.OverlapCircleAll(hand.position, 2f, objects);
			int heldObject = -1;
			print("found"+objs.Length+"objects");
			for(int i = 0; i < objs.Length; i++)
			{
				if(objs[i].gameObject != gameObject)
				{
					if(heldObject == -1)
					{
						print("addingfirstobject");
						heldObject = i;
					}
					else if((objs[i].transform.position - hand.position).magnitude < (objs[heldObject].transform.position - hand.position).magnitude)
					{
						print("addingnearestobject");
						heldObject = i;
					}
				}
			}
			if(heldObject > -1)
			{
				print("foundobject");
				grabbedObject = objs[heldObject].gameObject;
				grabbedObject.rigidbody2D.angularVelocity = 0;
				anim.SetTrigger ("Grab");
				isHolding = true;
			}
		}
		else
		{
			print("releaseobject");
			grabbedObject.rigidbody2D.velocity = rigidbody2D.velocity;
			grabbedObject = null;
			isHolding = false;
		}
	}
	void MovePlayer(Vector2 direction)
	{
		if((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
			Flip();
		//	direction.x *= speed;
		if(rigidbody2D.velocity.x <= speed && rigidbody2D.velocity.x >= speed*-1)
			if(rigidbody2D.velocity.y <= speed && rigidbody2D.velocity.y >= speed*-1)
				rigidbody2D.velocity += direction;
	}
	public override void Kill ()
	{
		base.Kill ();
	}
	public override void StartUp()
	{
		print ("STARTING UP LOFT");
		gameCamera.camera.orthographicSize = cameraDistance;
		isFlying = true;
		rigidbody2D.drag = 1;
		rigidbody2D.gravityScale = 0;
		for(int i = 0; i < eyes.Length; i++)
		{
			eyes[i].GetComponent<Animator>().SetTrigger ("Open");
		}
	}
	public override void ShutDown()
	{
		print ("SHUTTING DOWN LOFT");
		isFlying = false;
		if(isHolding == true)
			Action ();
		rigidbody2D.drag = 0;
		rigidbody2D.gravityScale = 1;
		rigidbody2D.fixedAngle = false;
		for(int i = 0; i < eyes.Length; i++)
		{
			eyes[i].GetComponent<Animator>().SetTrigger ("Close");
		}
	}
}
