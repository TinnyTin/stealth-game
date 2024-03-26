using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Threat Bar for debugging. Ended up adding it starting in the Game Alpha
 * External
 * Source Credit:       
 *                      
 */

public class CanvasLookatCamera : MonoBehaviour
{

    public MainCameraData mainCameraData;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //FindMainCamera();
        if (mainCameraData.mainCamera != null)
        {
            this.transform.LookAt(this.transform.position - (mainCameraData.mainCamera.transform.position - this.transform.position));
        }

    }
}
