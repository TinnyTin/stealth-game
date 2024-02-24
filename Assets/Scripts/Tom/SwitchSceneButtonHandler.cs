using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom    
 * Contributors:        
 *
 * External
 * Source Credit:
 */

public class SwitchSceneButtonHandler : MonoBehaviour
{
    public GameEvent onSceneSwitch; 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene()
    {
        if (onSceneSwitch != null) 
            onSceneSwitch.Raise(this, null);
        else
            Debug.LogWarning("OnSceneSwitch Game Event field not set in Inspector.");
    }
}
