// Scripts/Weapons/WeaponConfig.cs
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [Header("Stats")]
    public float Damage = 10f;
    public float Range = 20f;
    public int ClipSize = 10;
    public float RateOfFire = 0.5f; // Seconds between shots
    public float ReloadSpeed = 2f;

    [Header("Projectiles")]
    public GameObject ProjectilePrefab; // Prefab for the projectile
    public float ProjectileSpeed = 10f;
    public float Spread = 0f; // Spread angle in degrees

    [Header("Effects")]
    public bool ApplyPoison = false;
    public bool ApplySlow = false;
}
