using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom   
 * Contributors:        
 *
 * External
 * Source Credit:       Jeff Wilson (taken from CS6457 Milestone 1-4 source)
 */

[RequireComponent(typeof(AudioSource))]
public class EventSound2D : MonoBehaviour
{
    public AudioSource audioSrc;

    // Use this for initialization
    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!audioSrc.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}

