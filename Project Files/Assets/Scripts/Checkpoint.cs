using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public GameObject gameCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D activator)
	{
		GameObject mainPart = activator.gameObject;
		while(mainPart.transform.parent != null)
		{
			mainPart = mainPart.transform.parent.gameObject;
		}
		if(gameCamera.GetComponent<GameManager>().originalCharacter == mainPart)
		{
			Destroy(gameCamera.GetComponent<GameManager>().checkpoint.gameObject);
			gameCamera.GetComponent<GameManager>().checkpoint = transform;
		}
	}
}
