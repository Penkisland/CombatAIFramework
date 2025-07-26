using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ANS_NewANSBase", menuName = "ScriptableObjects/AnimationNotifySignals/ANS_ANSBase")]
public class ANS_ANSBase : ScriptableObject
{
    public Action<ANSC_ANSContainer> OnAnimationNotifySignal;
}
