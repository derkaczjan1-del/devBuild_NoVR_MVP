using UnityEngine;

public class PickupDetector : MonoBehaviour
{
    private bool playerInRange = false;

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}