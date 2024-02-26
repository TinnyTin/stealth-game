using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:    Tom 
 * Contributors:        
 *
 * External
 * Source Credit:
 */

[CreateAssetMenu(menuName = "SO's/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerHealth = 10.0f;
    [Space]
    public Vector3 PlayerPosition = Vector3.zero;
}