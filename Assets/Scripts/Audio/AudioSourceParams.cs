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

public class AudioSourceParams
{
    public static readonly AudioSourceParams Default = new(); 

    private float _volume = 1.0f;
    public float Volume
    {
        get => _volume;
        set => _volume = Mathf.Clamp01(value);
    }

    private float _dopplerLevel = 0.0f;
    public float DopplerLevel
    {
        get => _dopplerLevel;
        set => _dopplerLevel = Mathf.Clamp(value, 0.0f, 5.0f);
    }

    private AudioRolloffMode _rolloffMode = AudioRolloffMode.Logarithmic;
    public AudioRolloffMode RolloffMode
    {
        get => _rolloffMode;
        set
        {
            if (value != AudioRolloffMode.Custom)
                _rolloffMode = value;
            else
                Debug.LogWarning("AudioRolloffMode.Custom not supported.");
        }
    }

    private float _minDistance = 1.0f;
    public float MinDistance
    {
        get => _minDistance;
        set => _minDistance = Mathf.Clamp(value, 0.0f, float.PositiveInfinity);
    }

    private float _maxDistance = 500.0f;
    public float MaxDistance
    {
        get => _maxDistance;
        set => _maxDistance = Mathf.Clamp(value, 0.0f, float.PositiveInfinity);
    }

    private float _stereoPan = 0.0f;
    public float StereoPan
    {
        get => _stereoPan;
        set => _stereoPan = Mathf.Clamp(value, -1.0f, 1.0f); 
    }

    public bool Loop { get; set; }

    public AudioSourceParams()
    {
        Volume = 1.0f;
        DopplerLevel = 0.0f;
        RolloffMode = AudioRolloffMode.Logarithmic;
        MinDistance = 1.0f;
        MaxDistance = 500.0f;
        Loop = false; 
    }
}
