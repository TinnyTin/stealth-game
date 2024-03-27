using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AIStateMachine_Wife : AIStateMachine
{
    [Header("----- Derived Class Fields -----")]
    [Space]

    [Header("AudioClips")]
    [SerializeField] private AudioClip _audioClipCatch;

    [Space]
    [Header("FOV Grow/Shrink (Units: Degrees & Seconds")]
    public float minFOVAngle = 0f;
    public float maxFOVAngle = 180f;
    public float growFOVAngleTime = 5f;
    public float shrinkFOVAngleTime = 5f;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void BehaviorInvestigate()
    {
        _currentState.SwitchState(_states.WifeInvestigate());
    }

    public override void BehaviorPursuit()
    {
        _currentState.SwitchState(_states.WifePursuit());
    }

    public override void CatchPlayer()
    {
        StartCoroutine(CoroutinePlayCaughtPlayerSound(0.3f));
        StartCoroutine(CoroutineCatchPlayer(1.0f));
    }

    private IEnumerator CoroutinePlayCaughtPlayerSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioSourceParams audioSourceParams = new();
        audioSourceParams.MinDistance = 500;
        AudioChannel.Raise(_audioClipCatch, transform.position, audioSourceParams);
    }

    private IEnumerator CoroutineCatchPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerCaught.Raise();
    }
}
