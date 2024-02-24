using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO's/Managers/AudioManager")]
public class AudioManager : ScriptableObject
{
    //void OnEnable()
    //{
    //    Debug.Log("I exist!");
    //}

    public void PlayASound(Component sender, object data)
    {
        Debug.Log($"AudioManager received a single param event from {sender.name}\n" +
                  $"data: \'{data}\' - Playing sound <sound_name>");
    }

    public void PlayASound2(Component sender, object data1, object data2)
    {
        Debug.Log($"AudioManager received a 2 param event from {sender.name}\n" +
                  $"data1: \'{data1}\' - data2: \'{data2}\' - Playing sound <sound_name>");
    }
}
