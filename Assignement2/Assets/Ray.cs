using UnityEngine;

public class RayShooting : MonoBehaviour
{
    RaycastHit hit;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 300f))
            {
                //your code to deal damage to the object
                Debug.Log(hit.collider.gameObject);
            }

        }
    }

}