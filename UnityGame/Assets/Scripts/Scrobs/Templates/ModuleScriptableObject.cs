using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "ModulesScriptableObject", menuName = "ScriptableObject/Module")]
public class ModuleScriptableObject : ScriptableObject
{
    public float RotationSpeedModifier;
    public float SpeedModifier;
    public float FireRateModifier;
    public float HealthModifier;
}
