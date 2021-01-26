using UnityEngine;
using System.Collections;

public class KillScript : MonoBehaviour {
	public GameObject plosion;
	public GameObject gameCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D victim)
	{
		GameObject mainPart = victim.gameObject;
		while(mainPart.transform.parent != null)
		{
			mainPart = mainPart.transform.parent.gameObject;
		}
		if(mainPart.GetComponent<Rigidbody2D>())
		{
			Instantiate(plosion,mainPart.transform.position,mainPart.transform.rotation);
			if(mainPart.gameObject.GetComponent<Controller>())
			{
				mainPart.gameObject.GetComponent<Controller>().Kill();
			}
			else
				GameObject.Destroy (mainPart);
		}
	}
}
