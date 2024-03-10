using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance 
    private static AudioManager _audioManager;
    private static bool _hasShutdown = false;

    public static AudioManager Instance
    {
        get
        {
            if (_hasShutdown)
            {
                // Debug.LogWarning("AudioManager Instance accessed after shutdown.");
                return null;
            }

            if (!_audioManager)
            {
                _audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

                if (!_audioManager)
                {
                    Debug.LogError("There needs to be one active AudioManager script on a GameObject in your scene.");
                    return null;
                }
            }

            return _audioManager;
        }
    }

    // Inspector Visible Variables
    public EventSound3D eventSound3DPrefab;
    public EventSound2D eventSound2DPrefab;
    public AmbientSound2D ambientSound2DPrefab;

    // Local Variables
    private List<AmbientSound2D> _2dAudioSources = new();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDestroy()
    {
        DestroyAll2DAudioSources();
        _hasShutdown = true;
    }

    private EventSound3D CreateEventAudio3D(AudioClip clip, Vector3 worldPos, AudioSourceParams audioSourceParams)
    {
        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        // Use default audio source params if none provided
        audioSourceParams ??= AudioSourceParams.Default;

        snd.audioSrc.clip = clip;
        snd.audioSrc.dopplerLevel = audioSourceParams.DopplerLevel;
        snd.audioSrc.maxDistance = audioSourceParams.MaxDistance;
        snd.audioSrc.minDistance = audioSourceParams.MinDistance;
        snd.audioSrc.rolloffMode = audioSourceParams.RolloffMode;
        snd.audioSrc.panStereo = audioSourceParams.StereoPan;
        snd.audioSrc.volume = audioSourceParams.Volume;

        return snd;
    }

    private EventSound2D CreateEventAudio2D(AudioClip clip, AudioSourceParams audioSourceParams)
    {
        EventSound2D snd = Instantiate(eventSound2DPrefab);

        //AudioClip audioClip = Resources.Load<AudioClip>(assetPath);

        //if (audioClip == null)
        //{
        //    Debug.LogWarning($"{this.name}: Could not load audio clip with asset path {assetPath}");
        //    return null;
        //}

        // Use default audio source params if none provided
        audioSourceParams ??= AudioSourceParams.Default;

        snd.audioSrc.clip = clip;
        snd.audioSrc.spatialBlend = 0.0f; 
        snd.audioSrc.volume = audioSourceParams.Volume;

        return snd;
    }

    public void PlayAudio3DOneShot(AudioClip clip, Vector3 position, AudioSourceParams audioSourceParams)
    {
        EventSound3D sound = CreateEventAudio3D(clip, position, audioSourceParams);
        sound.audioSrc.Play();
    }

    public void PlayAudio2DOneShot(AudioClip clip, AudioSourceParams audioSourceParams)
    {
        EventSound2D sound = CreateEventAudio2D(clip, audioSourceParams);
        sound.audioSrc.Play();
    }

    public void PlayAudio3DOneShotWithStr(AudioClip clip, Vector3 position, string message, AudioSourceParams audioSourceParams)
    {
        EventSound3D sound = CreateEventAudio3D(clip, position, audioSourceParams);
        sound.audioSrc.Play();
        Debug.Log($"{this.name} - {System.Reflection.MethodBase.GetCurrentMethod().Name} - {message}");
    }

    public int CreateAmbientAudio2DLooping(AudioClip audioClip, AudioSourceParams audioSourceParams, bool playImmediate = true)
    {
        AmbientSound2D snd = Instantiate(ambientSound2DPrefab);

        audioSourceParams ??= AudioSourceParams.Default;

        snd.audioSrc.clip = audioClip;
        snd.audioSrc.loop = true;
        snd.audioSrc.volume = audioSourceParams.Volume;

        _2dAudioSources.Add(snd);

        if (playImmediate)
            snd.audioSrc.Play();

        return snd.GetInstanceID();
    }

    public bool PlayAmbientAudio2D(int ambientSoundId)
    {
        AmbientSound2D snd = _2dAudioSources.FirstOrDefault(snd => snd.GetInstanceID() == ambientSoundId);

        if (snd == null)
            return false;

        snd.audioSrc.Play();
        return true;
    }

    public bool PauseAmbientAudio2D(int ambientSoundId)
    {
        AmbientSound2D snd = _2dAudioSources.FirstOrDefault(snd => snd.GetInstanceID() == ambientSoundId);

        if (snd == null)
            return false;

        snd.audioSrc.Pause();
        return true;
    }

    public bool DestroyAmbientAudio2D(int ambientSoundId)
    {
        AmbientSound2D snd = _2dAudioSources.FirstOrDefault(snd => snd.GetInstanceID() == ambientSoundId);

        if (snd == null)
            return false;

        snd.Destroy();
        _2dAudioSources.Remove(snd);

        return true;
    }

    public bool SetAmbientAudio2DVolume(int ambientSoundId, float volume)
    {
        AmbientSound2D snd = _2dAudioSources.FirstOrDefault(snd => snd.GetInstanceID() == ambientSoundId);

        if (snd == null)
            return false;

        snd.audioSrc.volume = Mathf.Clamp01(volume);

        return true;
    }

    public bool IsAmbientAudio2DPlaying(int ambientSoundId)
    {
        AmbientSound2D snd = _2dAudioSources.FirstOrDefault(snd => snd.GetInstanceID() == ambientSoundId);

        if (snd == null)
            return false;

        return snd.audioSrc.isPlaying;
    }

    private void DestroyAll2DAudioSources()
    {
        foreach (AmbientSound2D audioSource in _2dAudioSources)
        {
            if (audioSource != null)
                audioSource.Destroy();
        }
    }
}
