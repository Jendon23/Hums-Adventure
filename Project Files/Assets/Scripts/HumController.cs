﻿using UnityEngine;
using System.Collections;

public class HumController : Controller
{
	
	public GameObject[] eyes;
	public int cameraDistance = 10;

	public float jumpHeight = 5.0f;

	float lastJumpTime;
	public float jumpTime;
	bool grounded;
	bool isWalking = false;
	
	public Transform groundCheck;
	public LayerMask ground;
	Animator anim;
	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0, ground);
		GetInput ();
		anim.SetBool ("isWalking", isWalking);
		anim.SetBool ("isGrounded", grounded);
	}
	void Orientate()
	{
		if(transform.rotation.eulerAngles.z > 0 && transform.rotation.eulerAngles.z <= 180)
		{
			rigidbody2D.angularVelocity = -45;
		}
		else if(transform.rotation.eulerAngles.z > 180)
		{
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
			MovePlayer(new Vector2(Input.GetAxis ("Horizontal"),0));
			if(Input.GetAxis ("Vertical") > 0 && Time.fixedTime - lastJumpTime > jumpTime)
			{
				Jump();
				lastJumpTime = Time.fixedTime;
			}
		}
	}
	void MovePlayer(Vector2 direction)
	{
		if((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
			Flip();
	//	direction.x *= speed;
		if(rigidbody2D.velocity.x <= speed && rigidbody2D.velocity.x >= speed*-1)
			rigidbody2D.velocity += direction;
		if(direction.x != 0)
			isWalking = true;
		else
			isWalking = false;
	}
	void Jump()
	{
		if(grounded)
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,rigidbody2D.velocity.y + jumpHeight);
			anim.SetTrigger ("Jump");
		}
	}
	public override void Kill ()
	{
		if(gameCamera.GetComponent<GameManager>().character != gameObject)
		{
			gameCamera.GetComponent<GameManager>().character.GetComponent<Controller>().ShutDown();
			gameCamera.transform.position = new Vector3(transform.position.x,transform.position.y,gameCamera.transform.position.z);
		}
		gameCamera.GetComponent<GameManager>().EndLevel (false);
		base.Kill();
	}
	public override void StartUp()
	{
		print ("STARTING UP HUM");
		gameCamera.camera.orthographicSize = cameraDistance;
		for(int i = 0; i < eyes.Length; i++)
		{
			eyes[i].GetComponent<Animator>().SetTrigger ("Open");
		}
	}
	public override void ShutDown()
	{
		isWalking = false;
		grounded = true;
		print ("SHUTTING DOWN HUM");
		rigidbody2D.fixedAngle = false;
		for(int i = 0; i < eyes.Length; i++)
		{
			eyes[i].GetComponent<Animator>().SetTrigger ("Close");
		}
	}
}