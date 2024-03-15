using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSetAIActiveState : MonoBehaviour
{
    public AIManager aiManager;

    private void OnTriggerEnter(Collider other)
    {
        // toggle AI active state
        aiManager.setAIActive(!aiManager.AreAIsActive);
    }
}
