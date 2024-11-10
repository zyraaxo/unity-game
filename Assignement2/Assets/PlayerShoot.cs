using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Player pressed shoot");
            shootInput?.Invoke();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            Debug.Log("Player pressed reload");
            reloadInput?.Invoke();
        }
    }

    private void OnDisable()
    {
        shootInput = null;
        reloadInput = null;
    }
}
