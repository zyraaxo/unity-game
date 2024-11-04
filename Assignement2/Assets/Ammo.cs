using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private Shooting shootingScript;

    void Start()
    {
        shootingScript = FindObjectOfType<Shooting>(); // Find the Shooting script in the scene
    }

    void Update()
    {
        // Check if the player presses the 'E' key
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetAmmoToMax();
        }
    }

    private void SetAmmoToMax()
    {
        if (shootingScript != null)
        {
            shootingScript.SetAmmo(shootingScript.GetMaxAmmo()); // Call SetAmmo method to set ammo to max
        }
    }
}
