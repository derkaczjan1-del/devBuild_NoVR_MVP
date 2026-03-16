using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public static void MakeNoise(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.HearNoise(position);
            }
        }
    }
}