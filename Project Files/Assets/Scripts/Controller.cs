using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public float speed = 10.0f;
	public GameObject gameCamera;
	public bool facingRight = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update (){
	
	}
	public void Flip()
	{
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		facingRight = !facingRight;
	}
	void OnMouseDown()
	{
		if(gameCamera.GetComponent<GameManager>().character != gameObject && (gameCamera.GetComponent<GameManager>().originalCharacter.transform.position - transform.position).magnitude <= 20)
		{
			gameCamera.GetComponent<GameManager>().character.GetComponent<Controller>().ShutDown();
			gameCamera.GetComponent<GameManager>().character = gameObject;
			gameCamera.GetComponent<GameManager>().targets[0] = transform;
			print ("STARTING UP SOMETHING ELSE");
			StartUp();
		}
	}
	public virtual void Kill()
	{
		ShutDown ();
		if(gameObject == gameCamera.GetComponent<GameManager>().character)
		{
			gameCamera.GetComponent<GameManager>().character = gameCamera.GetComponent<GameManager>().originalCharacter;
			gameCamera.GetComponent<GameManager>().targets[0] = gameCamera.GetComponent<GameManager>().originalCharacter.transform;
			gameCamera.GetComponent<GameManager>().originalCharacter.gameObject.GetComponent<Controller>().StartUp ();
		}
		GameObject.Destroy (gameObject);
	}
	public virtual void StartUp()
	{
	}
	public virtual void ShutDown()
	{
	}
}
