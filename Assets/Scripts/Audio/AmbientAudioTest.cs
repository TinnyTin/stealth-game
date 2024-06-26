using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 */

public class AmbientAudioTest : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;

    [Range(0.0f,1.0f)]
    [SerializeField] private float _volume = 1.0f;

    private int _audioClipId = 0; 

    // Start is called before the first frame update
    void Start()
    {
        if (_audioClip != null)
        {
            AudioSourceParams audioSourceParams = new AudioSourceParams
            {
                Volume = _volume
            };

            _audioClipId = AudioManager.Instance.CreateAmbientAudio2DLooping(_audioClip, audioSourceParams);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        if (AudioManager.Instance)
            AudioManager.Instance.DestroyAmbientAudio2D(_audioClipId); 
    }
}
