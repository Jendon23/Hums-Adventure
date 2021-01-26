using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public GameObject wall;
	public Transform startPos;
	public Transform endPos;
	
	public Transform camPos;
	
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public float smooth = 5.0F;
	private bool travelling = false;
	
	public float endCamPan;
	
	public GameObject gameCamera;
	public GameObject button;
	
	public bool panCamera;
	public bool canBePushed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!canBePushed && button.transform.localScale.x > 0)
		{
			button.transform.localScale -= new Vector3(0.01f,0f,0f);
		}
		else if(canBePushed && button.transform.localScale.x < 1)
		{
			button.transform.localScale += new Vector3(0.01f,0f,0f);
		}
		if(button.transform.localScale.x <= 0 && button.GetComponent<SpriteRenderer>().enabled)
		{
			button.GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(button.transform.localScale.x > 0 && !button.GetComponent<SpriteRenderer>().enabled)
		{
			button.GetComponent<SpriteRenderer>().enabled = true;
		}
		
		if(travelling)
		{
			if(panCamera && Time.time - startTime > endCamPan)
			{
				gameCamera.GetComponent<GameManager>().targets[0] = gameCamera.GetComponent<GameManager>().character.transform;
				panCamera = false;
			}
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			wall.transform.position = Vector3.Lerp(startPos.position, endPos.position, fracJourney);
			if(wall.transform.position == endPos.position)
			{
				travelling = false;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D victim)
	{
		if(canBePushed)
		{
			GameObject mainPart = victim.gameObject;
			while(mainPart.transform.parent != null)
			{
				mainPart = mainPart.transform.parent.gameObject;
			}
			if(mainPart == gameCamera.GetComponent<GameManager>().originalCharacter)
			{
				canBePushed = false;
				Activate();
			}
		}
	}
	void Activate()
	{
		startTime = Time.time;
		journeyLength = Vector3.Distance(startPos.position, endPos.position);
		travelling = true;
		if(panCamera)
		{
			gameCamera.GetComponent<GameManager>().targets[0] = camPos.transform;
		}
	}
}
