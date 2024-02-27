using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin
 * Contributors:        Tom
 *
 * External
 * Source Credit:          
 */

public class CanvasLookatCamera : MonoBehaviour
{
    public Camera tcam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FindMainCamera();

        this.transform.LookAt(this.transform.position - (tcam.transform.position - this.transform.position));
    }

    private void FindMainCamera()
    {
        // Find the main camera in the global scene
        GameObject activeCamera = SceneController.Instance.GetActiveCameraGameObject(); 
        tcam = activeCamera.GetComponent<Camera>();

        if (tcam == null)
            Debug.LogError("Could not get camera from SceneController. Canvas will not face camera.");
    }
}
