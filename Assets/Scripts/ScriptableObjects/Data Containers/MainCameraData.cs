using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: save the main camera reference into a gameobject
 * External
 * Source Credit:       
 *                      
 */

[CreateAssetMenu(menuName = "SO's/MainCameraData")]
public class MainCameraData : ScriptableObject
{
    public GameObject mainCamera;
}
