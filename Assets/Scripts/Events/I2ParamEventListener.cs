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

public interface I2ParamEventListener<T1, T2>
{
    public void OnEventRaised(T1 param1, T2 param2);
}