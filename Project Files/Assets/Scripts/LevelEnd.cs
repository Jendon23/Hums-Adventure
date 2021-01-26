using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour {
	public GameObject gameCamera;
	bool activated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(activated && transform.localScale.x < 100000)
		{
			transform.localScale += new Vector3(3,3,0);
		}
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
			activated = true;
			gameCamera.GetComponent<GameManager>().EndLevel (true);
		}
	}
}
