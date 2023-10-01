using UnityEngine;

[CreateAssetMenu(fileName = "ModulesScriptableObject", menuName = "ScriptableObject/Module")]
public class ModuleScriptableObject : ScriptableObject
{
    public float RotationSpeedModifier;
    public float SpeedModifier;
    public float DamageModifier;
    public float FireRateModifier;
    public float BulletSpeedModifier;
    public float ArmorModifier;
}
