﻿using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:
 *
 * External
 * Source Credit:
 */

public interface I3ParamEventListener<T1, T2, T3>
{
    public void OnEventRaised(Component sender, T1 param1, T2 param2, T3 param3);
}