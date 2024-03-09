using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateAIThreatChangeCallback : MonoBehaviour
{
    public void callback(Component component, AIThreatPriority threatPriority)
    {
        Debug.Log("Threatpriority reached: " + threatPriority);
    }
}
