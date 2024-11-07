using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private bool isPlayerInRange = false; // To check if the player is in range
    public GameObject gunModel; // The gun model to pick up
    public GameObject positionReference; // The reference GameObject for positioning the new gun
    private static bool firstGunPickedUp = false; // Static flag to check if this is the first gun pickup

    void Update()
    {
        // Check if the player is in range and pressing the "E" key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpGun();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            string gunName = gunModel.name;
            UIManager.Instance?.UpdatePickupText($"Press E to pick up {gunName}");
            UIManager.Instance?.TogglePickupText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.Instance?.TogglePickupText(false);
        }
    }

    private void PickUpGun()
    {
        PlayerMovementManager playerMovement = FindObjectOfType<PlayerMovementManager>();

        if (playerMovement != null)
        {
            GameObject currentGun = playerMovement.currentGun;

            if (currentGun != null)
            {
                Vector3 spawnPosition = positionReference.transform.position;
                GameObject newGun = Instantiate(gunModel, spawnPosition, currentGun.transform.rotation);

                newGun.transform.SetParent(Camera.main.transform);
                newGun.transform.localPosition = new Vector3(0.42f, -0.8f, 1.15f);

                currentGun.SetActive(false);
                playerMovement.SetNewGun(newGun);
                playerMovement.currentGun = newGun;

                Debug.Log("Gun picked up and replaced!");

                string weaponName = gunModel.name;
                UIManager.Instance?.DisplaySwitchWeaponText(weaponName);

                if (!firstGunPickedUp)
                {
                    firstGunPickedUp = true;
                    UIManager.Instance?.DisplaySwitchWeaponHint("Press Tab to switch weapons");
                }
            }
        }

        Destroy(gameObject);
        UIManager.Instance?.TogglePickupText(false);
    }
}
