using UnityEngine;
using System.Collections;

public class door : MonoBehaviour
{
	GameObject thedoor;

	public void OnTriggerEnter(Collider obj)
	{
		thedoor = GameObject.FindWithTag("SF_Door");
		thedoor.GetComponent<Animation>().Play("open");
	}

	public void OnTriggerExit(Collider obj)
	{
		thedoor = GameObject.FindWithTag("SF_Door");
		thedoor.GetComponent<Animation>().Play("close");
	}
}