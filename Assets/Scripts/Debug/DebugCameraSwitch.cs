using Cinemachine;
using UnityEngine;

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