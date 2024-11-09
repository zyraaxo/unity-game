using UnityEngine;
using TMPro;

public class DistanceMarker : MonoBehaviour
{
    public Transform target;
    public TextMeshProUGUI distanceText;
    public Transform player;
    public string markerText = "Distance to Target";
    public float heightOffset = 2.0f;
    public int requiredKeys = 3;

    void Update()
    {
        if (target != null && distanceText != null && player != null)
        {
            float distance = Vector3.Distance(player.position, target.position);

            distanceText.text = markerText + ": " + Mathf.Round(distance) + " units";

            Vector3 targetPosition = target.position + new Vector3(0, heightOffset, 0);
            transform.position = targetPosition;

            if (Player.Instance.GetKeys() >= requiredKeys)
            {
                distanceText.gameObject.SetActive(true);
            }
            else if (distance < 100f)
            {
                distanceText.gameObject.SetActive(true);
            }
            else
            {
                distanceText.gameObject.SetActive(false);
            }
        }
    }
}
