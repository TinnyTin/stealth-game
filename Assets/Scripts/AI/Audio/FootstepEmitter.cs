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

public class FootstepEmitter : MonoBehaviour
{
    [SerializeField] private FootStepFactory _footStepFactoryEvent;

    [Space] [SerializeField] private float _epsilon; 

    // joints in the player skeleton rig for footstep calculations
    [Space]
    [Header("Rig References")]
    [SerializeField] private GameObject _rigBase;
    [SerializeField] private GameObject _leftFoot;
    [SerializeField] private GameObject _rightFoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
