using System.Collections.Generic;
using System.Linq;
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

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Ambient Sound 3D")]
public class AmbientSound3D : MonoBehaviour
{
    private AudioSource _audioSrc;

    [SerializeField]
    private List<AudioClip> _audioClips = new();

    [SerializeField]
    [Tooltip("Select to play clips in list in random order. Otherwise they will be played in sequence.")]
    private bool _randomOrdering = false;

    [SerializeField]
    [Tooltip("Automatically loop. All but the first clip in the list will be ignored.")]
    private bool _loop = false;

    [SerializeField]
    [Range(0f, 60f)]
    [Tooltip("Minimum time until next clip is played in seconds. Set the same as Max Time to introduce no randomness.")]
    private float _minTimeBetweenClips;

    [SerializeField]
    [Range(0f, 60f)]
    [Tooltip("Maximum time until next clip is played in seconds. Set the same as Min Time to introduce no randomness.")]
    private float _maxTimeBetweenClips;

    [SerializeField] 
    [Tooltip("Optional name for emitter. Will only be used in console log messages.")]
    private string _emitterName = string.Empty;

    private int _currClipIndex = 0; 

    // Use this for initialization
    void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();

        if (_audioSrc == null)
        {
            Debug.LogWarning($"AmbientSound3D: Could not find AudioSource component. (Emitter name: {_emitterName})");
        }
        else
        {
            if (_audioSrc.loop)
                _loop = true; 
        }

        if (_audioClips.Any() && _audioSrc != null)
        {
            if (_loop)
            {
                _audioSrc.loop = true;
                _audioSrc.clip = _audioClips.First(); 
                _audioSrc.Play();
            }
        }
        else
        {
            Debug.LogWarning($"AmbientSound3D: AudioClip list is empty. (Emitter name: {_emitterName})");
        }
    }

    void Update()
    {
        if (_audioSrc.isPlaying == false && _loop == false && _audioClips.Any())
        {
            AudioClip clipToPlay = null;

            if (_randomOrdering)
            {
                clipToPlay = _audioClips.ElementAt(Random.Range(0, _audioClips.Count));
            }
            else
            {
                clipToPlay = _audioClips.ElementAt(_currClipIndex);
                
                _currClipIndex++;
                if (_currClipIndex >= _audioClips.Count)
                    _currClipIndex = 0;
            }

            float delay = Random.Range(_minTimeBetweenClips, _maxTimeBetweenClips);

            _audioSrc.clip = clipToPlay;
            _audioSrc.PlayDelayed(delay);
        }
    }

    private void OnValidate()
    {
        if (_maxTimeBetweenClips < _minTimeBetweenClips)
            _maxTimeBetweenClips = _minTimeBetweenClips;
    }
}
