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

public class ManagerContainer : MonoBehaviour
{
    public ScriptableObjectWithStart[] managers; 

    // Start is called before the first frame update
    void Start()
    {
        foreach (ScriptableObjectWithStart so in managers)
        {
            so.Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
