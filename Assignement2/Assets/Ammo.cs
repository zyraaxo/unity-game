using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class for ammo creates to reset ammo
public class Ammo : MonoBehaviour
{
    private Shooting shootingScript;

    void Start()
    {
        shootingScript = FindObjectOfType<Shooting>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetAmmoToMax();
        }
    }

    private void SetAmmoToMax()
    {
        if (shootingScript != null)
        {
            shootingScript.SetAmmo(shootingScript.GetMaxAmmo());
        }
    }
}
