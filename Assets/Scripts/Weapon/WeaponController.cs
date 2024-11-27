// Scripts/Weapons/WeaponController.cs
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponConfig weaponConfig; // Made public to fix the access issue

    private int currentClip;
    private bool isReloading;
    private Transform playerTransform;
    private Coroutine firingCoroutine;

    private void Start()
    {
        if (weaponConfig == null)
        {
            Debug.LogError("WeaponConfig is not assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(WaitForPlayerTransform());
    }

    private IEnumerator WaitForPlayerTransform()
    {
        // Wait until playerTransform is assigned
        while (playerTransform == null)
        {
            playerTransform = transform.parent; // Adjust as needed for your hierarchy
            yield return null;
        }

        currentClip = weaponConfig.ClipSize;
        Debug.Log("WeaponController initialized.");
    }

    public void StartFiring()
    {
        if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireWeapon());
        }
    }

    public void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            if (isReloading)
            {
                yield return null; // Wait until reloading is complete
                continue;
            }

            if (currentClip > 0)
            {
                FireProjectile();
                currentClip--;
                yield return new WaitForSeconds(weaponConfig.RateOfFire);
            }
            else
            {
                StartCoroutine(ReloadWeapon());
            }
        }
    }

    private void FireProjectile()
    {
        if (weaponConfig.ProjectilePrefab == null)
        {
            Debug.LogWarning($"No ProjectilePrefab assigned for {weaponConfig.name}.");
            return;
        }

        Vector3 firePosition = playerTransform.position;
        Quaternion fireRotation = playerTransform.rotation;

        // Add spread
        float spreadAngle = Random.Range(-weaponConfig.Spread / 2, weaponConfig.Spread / 2);
        fireRotation *= Quaternion.Euler(0, spreadAngle, 0);

        GameObject projectile = Instantiate(weaponConfig.ProjectilePrefab, firePosition, fireRotation);
        var projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            Debug.Log($"Initialized projectile with damage: {weaponConfig.Damage}, range: {weaponConfig.Range}, speed: {weaponConfig.ProjectileSpeed}");
            projectileScript.Initialize(weaponConfig.Damage, weaponConfig.Range, weaponConfig.ProjectileSpeed);
        }
    }

    private IEnumerator ReloadWeapon()
    {
        isReloading = true;
        yield return new WaitForSeconds(weaponConfig.ReloadSpeed);
        currentClip = weaponConfig.ClipSize;
        isReloading = false;
    }
}
