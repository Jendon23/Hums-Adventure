using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public List <Transform> targets = new List <Transform> ();
	public Transform checkpoint;
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	
	float endLevelTime;
	bool gameEnded = false;
	
	public GameObject character;
	public GameObject originalCharacter;
	
	// Use this for initialization
	void Start () {
//		character.transform.position = checkpoint.position;
		character.GetComponent<Controller>().StartUp ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Find("background").localScale = new Vector3(camera.orthographicSize/2,camera.orthographicSize/2,1);
		if(gameEnded && Time.time - endLevelTime > 6.0f)
		{
			print ("GAME OVER!");
			Application.LoadLevel ("BaseScene");
		}
		if(!gameEnded && targets.Count > 0)
		{
			Vector3 positionAvg = getAveragePosition ();
			Vector3 point = camera.WorldToViewportPoint(positionAvg);
			Vector3 delta = positionAvg - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = positionAvg + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		else
		{
			//	print (checkpoint.position);
			//	Vector3 point = camera.WorldToViewportPoint(checkpoint.position);
			//	Vector3 delta = point - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			//	Vector3 destination = checkpoint.position + delta;
			//	transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		//	transform.position = new Vector3(checkpoint.position.x,checkpoint.position.y,transform.position.z);
		}
	}
	Vector3 getAveragePosition()
	{
		Vector3 positionAvg = Vector3.zero;
		for(int i = 0; i < targets.Count; i++)
		{
			if(targets[i] != null)
				positionAvg += targets[i].position;
		}
		positionAvg /= targets.Count;
		positionAvg.z = -10f;
		return positionAvg;
	}
	public void RemoveTarget(Transform target)
	{
		if(targets.Contains (target))
		{
			targets.Remove (target);
		}
		else
			Debug.LogError("A transform that was removed from the camera wasnt there.");
	}
	public void AddTarget(Transform target)
	{
		if(!targets.Contains (target))
		{
			targets.Add (target);
		}
		else
			Debug.LogError("A transform that was added to the camera was already there.");
	}
	public void EndLevel(bool won)
	{
		endLevelTime = Time.time;
		gameEnded = true;
	}
}
