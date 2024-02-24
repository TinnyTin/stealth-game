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
        FindMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.transform.position - (tcam.transform.position - this.transform.position));
    }

    private void FindMainCamera()
    {
        // Find the main camera in the global scene
        GameObject[] globalSceneGameObjects = SceneManager.GetSceneAt(0).GetRootGameObjects();
        
        foreach (GameObject gameObject in globalSceneGameObjects)
        {
            tcam = gameObject.GetComponent<Camera>();
            if (tcam != null)
                break;
        }

        if (tcam == null)
            Debug.LogError("Cannot find Main Camera in Global Scene");
    }
}
