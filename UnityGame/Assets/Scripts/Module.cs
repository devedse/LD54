using System;
using System.Text;
using UnityEngine;

public class Module : MonoBehaviour
{
    public string DisplayName;
    public ShipModuleType ModuleType;
    public ModuleScriptableObject ModuleModifiers;

    public string ToStatsString(bool richText)
    {
        var sb = new StringBuilder();
        sb.AppendLine(DisplayName);
        sb.Append(AddModifierLine("Speed", ModuleModifiers.SpeedModifier, richText));
        sb.Append(AddModifierLine("Rotation speed", ModuleModifiers.RotationSpeedModifier, richText));
        sb.Append(AddModifierLine("Armor", ModuleModifiers.ArmorModifier, richText));
        sb.Append(AddModifierLine("Damage", ModuleModifiers.DamageModifier, richText));
        sb.Append(AddModifierLine("Fire rate", ModuleModifiers.FireRateModifier, richText));
        sb.Append(AddModifierLine("Bullet speed", ModuleModifiers.BulletSpeedModifier, richText));

        return sb.ToString().Trim();
    }

    public string AddModifierLine(string name, float value, bool richText)
    {
        if (value == 0)
            return "";

        if (!richText)
            return $"{name.ToUpper()}: {value.ToString("0.##")}\n";

        if (value > 0)
        {
            return $"{name.ToUpper()}: <color=green>{value.ToString("0.##")}</color>\n";
        }
        else
        {
            return $"{name.ToUpper()}: <color=red>{value.ToString("0.##")}</color>\n";
        }
    }
}
