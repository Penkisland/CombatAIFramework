using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DA_NewAIHatreConfig", menuName = "ScriptableObjects/AIHatreConfig/AIHatreConfig")]
public class DA_AIHatreConfig : ScriptableObject
{
    public float maxHatre = 100f;
    public float minHatre = 0f;

    /// <summary>
    /// When should the AI start to alert the player?
    /// </summary>
    public float hatreWhenAlert = 50f;

    /// <summary>
    /// How much Hatre should be added when the AI attacked by the player?
    /// Applies only once.
    /// </summary>
    public float hatreWhenAttacked = 80f;

    /// <summary>
    /// How much Hatre should be added when the AI is killed by the player?
    /// A negative value should be used to reduce Hatre.
    /// Apllies only once.
    /// </summary>
    public float hatreWhenKilled = 100f;

    /// <summary>
    /// How much Hatre should be added when the AI lost the player?
    /// A negative value should be used to reduce Hatre.
    /// Applies per tick.
    /// </summary>
    public float hatreWhenLost = 20f;


    /// <summary>
    /// How much Hatre should be added when the AI detects the player?
    /// Applies per tick.
    /// </summary>
    public float hatreWhenDetected = 30f;
}
