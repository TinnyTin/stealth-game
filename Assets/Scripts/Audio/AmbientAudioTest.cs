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

public class AmbientAudioTest : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private int _audioClipId = 0; 

    // Start is called before the first frame update
    void Start()
    {
        if (_audioClip != null)
        {
            _audioClipId = AudioManager.Instance.CreateAmbientAudio2DLooping(_audioClip);
            AudioManager.Instance.SetAmbientAudio2DVolume(_audioClipId, 0.6f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        AudioManager.Instance.DestroyAmbientAudio2D(_audioClipId); 
    }
}
