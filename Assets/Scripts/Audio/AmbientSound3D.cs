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
    [Tooltip("Automatically loop through the playlist. If unchecked, once all sounds have been played one no further sounds will be emitted.")]
    private bool _loop = false;

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("Maximum delay on playback of first audio clip. Prevents overlap from multiple sources playing the same sound. Value is only " +
             "relevant if there is a single audio clip in the playlist. If there are more than one use the min/max times below.")]
    private float _maxDelayOnFirstPlayback;

    [SerializeField]
    [Range(0f, 60f)]
    [Tooltip("Minimum time until next clip is played in seconds. Set the same as Max Time to introduce no randomness.")]
    private float _minTimeBetweenClips;

    [SerializeField]
    [Range(0f, 60f)]
    [Tooltip("Maximum time until next clip is played in seconds. Set the same as Min Time to introduce no randomness.")]
    private float _maxTimeBetweenClips;

    [SerializeField] 
    [Tooltip("Optional name for emitter. Name will be inherited from the GameObject that owns this script if empty. Will only be used in console log messages.")]
    private string _emitterName = string.Empty;

    private int _currClipIndex = 0;

    private List<bool> _playedState = new(); 

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
            foreach (AudioClip clip in _audioClips)
            {
                _playedState.Add(false);
            }
        }
        else
        {
            Debug.LogWarning($"AmbientSound3D: AudioClip list is empty. (Emitter name: {_emitterName})");
        }
    }

    void Update()
    {
        // Don't play any more clips if all have been played 
        // looping has been disabled. 
        if (_loop == false && _playedState.All(e => e) == true)
            return; 

        if (_audioSrc.isPlaying == false && _audioClips.Any())
        {
            if (_audioClips.Count == 1)
            {
                // Play just the first item in the list, possibly forever
                _audioSrc.loop = _loop;
                _audioSrc.clip = _audioClips.First();
                _audioSrc.PlayDelayed(_maxDelayOnFirstPlayback);
                _playedState[0] = true;
            }
            else
            {
                // Pick the next item in the list to play. If looping is 
                // disabled then the clip is marked as played and won't 
                // be picked again. 
                AudioClip clipToPlay = null;

                if (_randomOrdering)
                {
                    // Pick random clip to play
                    int rand = -1;
                    do
                    {
                        rand = Random.Range(0, _audioClips.Count);
                    } while (_playedState[rand] == true);

                    clipToPlay = _audioClips.ElementAt(rand);

                    // Only mark clip as played if looping is disabled
                    if (_loop == false)
                        _playedState[rand] = true; 
                }
                else
                {
                    clipToPlay = _audioClips.ElementAt(_currClipIndex);

                    // Only mark clip as played if looping is disabled
                    if (_loop == false)
                        _playedState[_currClipIndex] = true;

                    _currClipIndex++;
                    if (_currClipIndex >= _audioClips.Count)
                        _currClipIndex = 0;
                }

                // Set initial playback delay
                float delay = Random.Range(_minTimeBetweenClips, _maxTimeBetweenClips);

                // Play!
                _audioSrc.clip = clipToPlay;
                _audioSrc.PlayDelayed(delay);
            }
        }
    }

    private void OnValidate()
    {
        if (_maxTimeBetweenClips < _minTimeBetweenClips)
            _maxTimeBetweenClips = _minTimeBetweenClips;

        if (string.IsNullOrEmpty(_emitterName))
            _emitterName = this.gameObject.name; 
    }

    void OnDisable()
    {
        if (_audioSrc.isPlaying)
            _audioSrc.Stop();
    }
}
