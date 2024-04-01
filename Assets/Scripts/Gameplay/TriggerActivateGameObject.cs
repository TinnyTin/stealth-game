using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: 
 * External Source Credit:
 *
 */

public class TriggerActivateGameObject : MonoBehaviour
{
    public GameObject[] GOs;

    public GameEvent AudioChannel;
    public AudioClip AudioClip;

    bool isEnabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
        {
            foreach (GameObject GO in GOs)
            {
                if (GO != null) 
                    GO.SetActive(true);
            }

            if (AudioChannel != null)
                AudioChannel.Raise(AudioClip, this.transform.position, AudioSourceParams.Default);
            
            isEnabled = true;
        }
    }
}
