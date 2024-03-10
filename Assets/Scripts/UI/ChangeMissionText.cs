using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Change mission text for mission summary HUD
 */
public class ChangeMissionText : MonoBehaviour
{
    public string missionSuccess = "Mission Success";
    public string missionFail = "Caught by a Guard";
    private TMPro.TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMissionTextToFailure() { 
        tmp.text = missionFail;
    }
}
