using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        Martin
 */

[RequireComponent(typeof(BoxCollider))]
public class ExtractionPoint : MonoBehaviour
{
    public AudioClip AudioClipExtract;
    public GameEvent eventToRaise;

    private GameObject childDefault;
    private GameObject childHilighted;

    public GameEvent PlayerReachedExit;

    public PlayerData playerData;

    private bool hastExtracted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (AudioClipExtract == null)
        {
            Debug.LogError("ExtractionPoint: No AudioClipExtract");
        }

        if (playerData == null)
        {
            Debug.LogError("ExtractionPoint: No PlayerData SO");
        }

        hastExtracted = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (playerData.PlayerHasStolenObject)
        {
            Debug.Log("ExtractionPoint: collide.");
            if (!hastExtracted)
            {
                Extract();
                hastExtracted = true;
            }

        }
    }

    public void Extract()
    {
        if (AudioClipExtract != null)
            eventToRaise.Raise(AudioClipExtract, AudioSourceParams.Default);

        PlayerReachedExit.Raise();
    }
}
