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

public interface I4ParamEventListener<T1, T2, T3, T4>
{
    public void OnEventRaised(T1 param1, T2 param2, T3 param3, T4 param4);
}