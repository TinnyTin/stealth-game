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

public interface I1ParamEventListener<T1>
{
    public void OnEventRaised(Component sender, T1 param1);
}