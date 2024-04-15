using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Toggle AI active state through test trigger
 * External
 * Source Credit:       
 *                      
 */

public class testSetAIActiveState : MonoBehaviour
{
    public AIManager aiManager;

    private void OnTriggerEnter(Collider other)
    {
        // toggle AI active state
        aiManager.setAIActive(!aiManager.AreAIsActive);
    }
}
