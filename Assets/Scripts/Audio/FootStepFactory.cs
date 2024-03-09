using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorCharacteristic
{
    Dirt,
    Wood,
    Road,
    Concrete,
    Total
}

public enum StepCharacteristic
{
    Walk,
    Run,
    Total
}


[CreateAssetMenu(menuName = "SO's/FootstepLookup")]
public class FootStepFactory: ScriptableObjectWithInit
{
    [SerializeField]
    public FootStepInfo[] footStepInfoArray;
    public GameEvent eventToRaise;

    private FootStepCollection[,] footStepCollectionArray = new FootStepCollection[(int)FloorCharacteristic.Total, (int)StepCharacteristic.Total];

    public override void Init()
    {
        foreach (FootStepInfo footstepInfo in footStepInfoArray)
        {
            footStepCollectionArray[
                (int)footstepInfo._floorCharacteristic,
                (int)footstepInfo._stepCharacteristic]
                = footstepInfo._footStepCollection;
        }
    }

    public AudioClip getFootStepRandom(FloorCharacteristic floorCharacteristic, StepCharacteristic stepCharacteristic)
    {
        if (footStepCollectionArray[(int)floorCharacteristic, (int)stepCharacteristic] != null)
        {
            return footStepCollectionArray[(int)floorCharacteristic, (int)stepCharacteristic].getRandomAudioClip();
        }
        return null;
    }

    public AudioClip playFootStepRandom(FloorCharacteristic floorCharacteristic, StepCharacteristic stepCharacteristic, Vector3 position)
    {
        AudioClip footstep = null;
        footstep = getFootStepRandom(floorCharacteristic, stepCharacteristic);
        // play sound
        eventToRaise.Raise(footstep, position);
        return footstep;
    }
}

[System.Serializable]
public struct FootStepInfo
{
    public string name;
    public FloorCharacteristic _floorCharacteristic;
    public StepCharacteristic _stepCharacteristic;
    public FootStepCollection _footStepCollection;
}

