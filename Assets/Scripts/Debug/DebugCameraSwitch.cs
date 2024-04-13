using Cinemachine;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:   Tom  
 * Contributors:        
 *
 * External
 * Source Credit:
 */

public class DebugCameraSwitch : MonoBehaviour
{
    private FreeCam freecam;
    private CinemachineBrain cinemachineCam;

    void Awake()
    {
        cinemachineCam = this.GetComponentInParent<CinemachineBrain>(true);
        freecam = this.GetComponentInParent<FreeCam>(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            freecam.enabled = !freecam.enabled;
            cinemachineCam.enabled = !cinemachineCam.enabled;
        }
    }
}