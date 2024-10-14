using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject gunButtonPrefab; // This should show up in the Inspector
    public Transform inventoryContent;
    public GunData startingGun; // Assign the starting pistol in the Inspector
    private List<GunData> guns = new List<GunData>(); // List to hold the guns

    void Start()
    {
        if (startingGun == null)
        {
            Debug.LogError("Starting gun is not assigned!");
            return; // Exit the method early
        }

        AddGun(startingGun);
        DisplayGuns(); // Display guns in the inventory
    }

    public void DisplayGuns()
    {
        if (gunButtonPrefab == null)
        {
            Debug.LogError("Gun button prefab is not assigned!");
            return;
        }

        if (inventoryContent == null)
        {
            Debug.LogError("Inventory content is not assigned!");
            return;
        }

        // Clear previous items
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each gun in the inventory
        foreach (GunData gun in guns)
        {
            if (gun == null)
            {
                Debug.LogError("Gun is null in the inventory!");
                continue; // Skip to the next gun if this one is null
            }

            GameObject gunButton = Instantiate(gunButtonPrefab, inventoryContent);
            //gunButton.GetComponentInChildren<Text>().text = gun.gunName; // Updated to use gun.gunName
            // (rest of the code remains the same...)
        }

        // Update layout manually
        UpdateLayout();
    }


    // Function to manually update layout
    private void UpdateLayout()
    {
        // Calculate the position for each button
        float buttonHeight = gunButtonPrefab.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < inventoryContent.childCount; i++)
        {
            RectTransform childRect = inventoryContent.GetChild(i).GetComponent<RectTransform>();
            childRect.anchoredPosition = new Vector2(0, -i * buttonHeight);
        }
    }

    // Function to handle gun selection
    private void OnGunSelected(GunData selectedGun)
    {
        Debug.Log($"Selected gun: {selectedGun.gunName}");
        // Handle gun switching logic here
    }

    // Function to add guns to the inventory
    public void AddGun(GunData newGun)
    {
        if (!guns.Contains(newGun))
        {
            guns.Add(newGun);
            DisplayGuns(); // Update the inventory display
        }
    }
}
