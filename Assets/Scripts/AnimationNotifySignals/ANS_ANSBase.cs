using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ANS_NewANSBase", menuName = "ScriptableObjects/AnimationNotifySignals/ANS_ANSBase")]
public class ANS_ANSBase : ScriptableObject
{
    /// <summary>
    /// when impleting this, the class should call this method to handle the signal
    /// args is an array of strings that can be used to pass additional parameters
    /// for example, if the signal is "Hit,100", args would be ["100"]
    /// </summary>
    public Action<ANSC_ANSContainer, string[]> OnAnimationNotifySignal;
}
