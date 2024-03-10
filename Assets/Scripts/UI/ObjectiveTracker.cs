using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Objective Tracker for HUD
 */

public class ObjectiveTracker : MonoBehaviour
{
    public GameObject mainObjective;

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is placeholder functionality for proof of concept.
    public void mainObjectiveObtained()
    {
        if (mainObjective != null)
        {
            TMPro.TextMeshProUGUI textMesh = mainObjective.GetComponent<TMPro.TextMeshProUGUI>();
            textMesh.color = Color.grey;
            textMesh.text = "<s>" + textMesh.text + "</s>";
        }
    }
}
