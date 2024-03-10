using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 */

[RequireComponent(typeof(CapsuleCollider))]
public class AudioTestCapsule : MonoBehaviour
{
    public AudioClip audioClip;
    public GameEvent eventToRaise;
    private CapsuleCollider capsuleCollider;
    public int soundId = -1;
    public float volume = 1.0f;

    public enum TestType
    {
        OneShot3D,
        OneShot2D,
        DemoTest,
        AmbientSound2D,
    }

    public TestType testType;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testType == TestType.AmbientSound2D)
        {
            if (Input.GetKeyDown(KeyCode.F8))
                volume += 0.1f;
            if (Input.GetKeyDown(KeyCode.F7))
                volume -= 0.1f;

            volume = Mathf.Clamp01(volume);

            AudioManager.Instance.SetAmbientAudio2DVolume(soundId, volume);

            if (Input.GetKeyDown(KeyCode.F6))
            {
                AudioManager.Instance.DestroyAmbientAudio2D(soundId);
                soundId = -1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (eventToRaise != null)
        {
            if (testType == TestType.OneShot3D)
                eventToRaise.Raise(audioClip, transform.position, AudioSourceParams.Default);
            else if (testType == TestType.OneShot2D)
                eventToRaise.Raise(audioClip);
            else if (testType == TestType.DemoTest)
                eventToRaise.Raise(audioClip, transform.position, "This is a test");
        }
        else
        {
            if (testType == TestType.AmbientSound2D)
            {
                if (soundId == -1)
                    soundId = AudioManager.Instance.CreateAmbientAudio2DLooping(audioClip, AudioSourceParams.Default);
                else
                {
                    if (AudioManager.Instance.IsAmbientAudio2DPlaying(soundId))
                        AudioManager.Instance.PauseAmbientAudio2D(soundId);
                    else
                        AudioManager.Instance.PlayAmbientAudio2D(soundId);
                }
            }
        }
    }
}
