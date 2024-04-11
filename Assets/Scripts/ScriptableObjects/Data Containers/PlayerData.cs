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

    //public float PlayerHealth = 10.0f;
    [Space]
    public Vector3 PlayerPosition;
    public float PlayerSprintStamina;
    public bool PlayerHasStolenObject;
    [Header("Player Visibility / Camo")]

    public float DefaultCamo = 0f;
    private float _playerCamouflage = 0f;
    private float _playerVisibility = 1f;
    public float PlayerCamouflage { get { return _playerCamouflage; } } // if the player is fully camo'd, then the AI basically cannot sense them??
    public float PlayerVisibility { get { return _playerVisibility; } }

    public void SetPlayerCamo(float camo)
    {
        camo = Mathf.Clamp(camo, 0f, 1f);
        _playerCamouflage = camo;
        _playerVisibility = 1 - camo;
    }

    public void ResetPlayerCamo()
    {
        SetPlayerCamo(DefaultCamo);
    }

}
