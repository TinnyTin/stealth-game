using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Template for AI state change upon scriptable object Event firing
 * External
 * Source Credit:       
 *                      
 */
public class TemplateAIThreatChangeCallback : MonoBehaviour
{
    public void callback(Component component, AIThreatPriority threatPriority)
    {
        Debug.Log("Threatpriority reached: " + threatPriority);
    }
}
