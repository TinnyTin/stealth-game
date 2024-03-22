using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:    Tom 
 * Contributors:        
 */

[CreateAssetMenu(menuName = "SO's/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerHealth = 10.0f;
    [Space]
    public Vector3 PlayerPosition = Vector3.zero;
    public float PlayerSprintStamina = 1f;
}
