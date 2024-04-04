using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        
 * Description: 
 * External Source Credit:
 *
 */

public class TriggerDeActivateGameObject : MonoBehaviour
{
    public GameObject[] GOs;
    public float CooldownTimer = 3f;

    bool isDeactivated = false;

    private float tCooldown = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isDeactivated)
        {
            foreach (GameObject GO in GOs)
            {
                GO.SetActive(false);
            }
            isDeactivated = true;
        }

    }
    private void Update()
    {
        tCooldown = Mathf.Max(0f, tCooldown - Time.deltaTime);
        if (tCooldown == 0)
        {
            isDeactivated = false;
            tCooldown = CooldownTimer;
        }
    }
}
