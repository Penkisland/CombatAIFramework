using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DAB_NewBasicAttributes", menuName = "ScriptableObjects/DataAsset/DAB_BasicAttributes")]
public class DAB_BasicAttributes : ScriptableObject
{
    [Header("Basic Attributes")]
    public float maxHealth = 100f;
    public float baseAtk = 10f;
    public float baseDef = 5f;
    public float maxToughness = 100f;

    [Header("Rate Attributes")]
    public float atkRate = 1f;
    public float defRate = 1f;
    public float moveRate = 1f;
    public float toughnessRate = 1f;
    public float impactResistanceRate = 1f;

    [Header("Other Attributes")]
    public List<float> impactResistance = new List<float>();
}
