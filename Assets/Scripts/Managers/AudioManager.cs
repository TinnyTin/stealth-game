using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance 
    private static AudioManager _audioManager;

    public static AudioManager Instance
    {
        get
        {
            if (!_audioManager)
            {
                _audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

                if (!_audioManager)
                {
                    Debug.LogError("There needs to be one active AudioManager script on a GameObject in your scene.");
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

    private EventSound3D CreateEventAudio3D(AudioClip clip, Vector3 worldPos)
    {
        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        //AudioClip audioClip = Resources.Load<AudioClip>(assetPath);

        //if (audioClip == null)
        //{
        //    Debug.LogWarning($"{this.name}: Could not load audio clip with asset path {assetPath}");
        //    return null;
        //}

        snd.audioSrc.clip = clip;
        snd.audioSrc.dopplerLevel = 0f;
        return snd;
    }

    private EventSound2D CreateEventAudio2D(AudioClip clip)
    {
        EventSound2D snd = Instantiate(eventSound2DPrefab);

        //AudioClip audioClip = Resources.Load<AudioClip>(assetPath);

        //if (audioClip == null)
        //{
        //    Debug.LogWarning($"{this.name}: Could not load audio clip with asset path {assetPath}");
        //    return null;
        //}

        snd.audioSrc.clip = clip;
        return snd;
    }

    public void PlayAudio3DOneShot(Component sender, AudioClip clip, Vector3 position)
    {
        //if (clip is AudioClip clipp && position is Vector3 vector3)
        {
            EventSound3D sound = CreateEventAudio3D(clip, position);
            sound.audioSrc.Play();
        }
    }

    public void PlayAudio2DOneShot(Component sender, AudioClip clip)
    {
        //if (clip is AudioClip clipp && position is Vector3 vector3)
        {
            EventSound2D sound = CreateEventAudio2D(clip);
            sound.audioSrc.Play();
        }
    }

    public void DemoEventFunction(Component sender, AudioClip clip, Vector3 position, string message)
    {
        EventSound3D sound = CreateEventAudio3D(clip, position);
        sound.audioSrc.Play();
        Debug.Log($"{this.name} - {System.Reflection.MethodBase.GetCurrentMethod().Name} - {message}");
    }

    public int CreateAmbientAudio2DLooping(AudioClip audioClip, bool playImmediate = true)
    {
        AmbientSound2D snd = Instantiate(ambientSound2DPrefab); 
        snd.audioSrc.clip = audioClip; 
        snd.audioSrc.loop = true;
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
}
