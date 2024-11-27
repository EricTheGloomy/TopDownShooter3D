// Scripts/Weapons/WeaponManager.cs
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<WeaponConfig> equippedWeapons; // List of weapons
    private List<WeaponController> weaponControllers = new();

    public void InitializeWeapons()
    {
        foreach (var weapon in equippedWeapons)
        {
            AddWeapon(weapon);
        }
    }

    private void AddWeapon(WeaponConfig weaponConfig)
    {
        GameObject weaponObject = new GameObject($"Weapon_{weaponConfig.name}");
        weaponObject.transform.SetParent(transform);

        var weaponController = weaponObject.AddComponent<WeaponController>();
        weaponController.weaponConfig = weaponConfig;
        weaponControllers.Add(weaponController);

        weaponController.StartFiring();
    }
}
