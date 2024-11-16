using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "Wolf Pelt";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ?� nh?t: " + itemName);
            // Th�m v�o h? th?ng inventory n?u c?n
            Destroy(gameObject);
        }
    }
}