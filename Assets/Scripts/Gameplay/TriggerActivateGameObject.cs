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

    public float CooldownTimer = 3f;
    bool isEnabled = false;

    private float tCooldown = 0f;


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

    private void Update()
    {
        tCooldown = Mathf.Max(0f, tCooldown - Time.deltaTime);
        if (tCooldown == 0)
        {
            isEnabled = false;
            tCooldown = CooldownTimer;
        }
    }
}
