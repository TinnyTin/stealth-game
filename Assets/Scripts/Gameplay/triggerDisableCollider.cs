using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: disables colliders 
 * External
 * Source Credit:       
 *                      
 */

public class triggerDisableCollider : MonoBehaviour
{
    public GameObject GO;

    bool isDisabled = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isDisabled)
        {
            GO.SetActive(false);
            isDisabled = true;
        }
        
        
    }
}
