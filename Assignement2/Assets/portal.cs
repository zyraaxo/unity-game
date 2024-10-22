using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Log the player's distance to the portal
            float distance = Vector3.Distance(other.transform.position, transform.position);
            Debug.Log("Player is " + distance + " units away from the portal.");

            // Additional logic for what happens when the player is near the portal can go here
        }
    }
}
