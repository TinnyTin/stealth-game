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

public class TriggerDeActivateGameObject: MonoBehaviour
{
    public GameObject[] GOs;

    bool isDeactivated = false;

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
}
