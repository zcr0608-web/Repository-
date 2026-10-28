using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MobileStrikeWeapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponConfig
    {
        public string weaponName = "Assault Rifle";
        [Min(1)] public int magazineSize = 30;
        [Min(0.01f)] public float fireInterval = 0.12f;
        [Min(0f)] public float damage = 20f;
        [Min(1f)] public float range = 100f;
    }

    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public Transform recoilPivot;
    public WeaponConfig[] weapons =
    {
        new WeaponConfig { weaponName = "Assault Rifle", magazineSize = 30, fireInterval = 0.12f, damage = 20f, range = 100f },
        new WeaponConfig { weaponName = "Pistol", magazineSize = 12, fireInterval = 0.28f, damage = 35f, range = 75f }
    };
    public GameObject[] weaponVisuals;
    public UnityEvent<int, int, string> onAmmoChanged;
    public UnityEvent<string> onHit;

    public int CurrentAmmo { get; private set; }
    public int WeaponIndex { get; private set; }
    private float nextFireTime;
    private bool reloading;
    private Quaternion recoilRestRotation;

    private WeaponConfig CurrentWeapon
    {
        get
        {
            if (weapons == null || weapons.Length == 0) return null;
            return weapons[Mathf.Clamp(WeaponIndex, 0, weapons.Length - 1)];
        }
    }

    private void Awake()
    {
        if (playerCamera == null) playerCamera = Camera.main;
        if (recoilPivot != null) recoilRestRotation = recoilPivot.localRotation;
        if (CurrentWeapon != null) CurrentAmmo = CurrentWeapon.magazineSize;
        ApplyWeaponVisuals();
        NotifyAmmo();
    }

    // Wire a mobile UI Fire button's OnClick event to this method.
    public void ShootOnce()
    {
        WeaponConfig config = CurrentWeapon;
        if (config == null || reloading || Time.time < nextFireTime) return;
        if (CurrentAmmo <= 0) { onHit?.Invoke("Reload"); return; }
        if (playerCamera == null) return;

        nextFireTime = Time.time + config.fireInterval;
        CurrentAmmo--;
        if (muzzleFlash != null) muzzleFlash.Play();
        ApplyRecoil();
        NotifyAmmo();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, config.range))
        {
            MobileStrikePlayerHealth health = hit.collider.GetComponentInParent<MobileStrikePlayerHealth>();
            if (health != null) health.TakeDamage(config.damage);
            onHit?.Invoke(hit.collider.gameObject.name);
        }
    }

    public void Reload()
    {
        if (!isActiveAndEnabled || reloading || CurrentWeapon == null || CurrentAmmo == CurrentWeapon.magazineSize) return;
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        reloading = true;
        onHit?.Invoke("Reloading...");
        yield return new WaitForSeconds(1.2f);
        if (CurrentWeapon != null) CurrentAmmo = CurrentWeapon.magazineSize;
        reloading = false;
        NotifyAmmo();
    }

    public void SwitchWeapon()
    {
        if (weapons == null || weapons.Length < 2) return;
        WeaponIndex = (WeaponIndex + 1) % weapons.Length;
        CurrentAmmo = CurrentWeapon.magazineSize;
        reloading = false;
        ApplyWeaponVisuals();
        NotifyAmmo();
    }

    private void ApplyWeaponVisuals()
    {
        if (weaponVisuals == null) return;
        for (int i = 0; i < weaponVisuals.Length; i++)
            if (weaponVisuals[i] != null) weaponVisuals[i].SetActive(i == WeaponIndex);
    }

    private void ApplyRecoil()
    {
        if (recoilPivot == null) return;
        StopCoroutine(nameof(RecoilRoutine));
        StartCoroutine(nameof(RecoilRoutine));
    }

    private IEnumerator RecoilRoutine()
    {
        Quaternion kick = recoilRestRotation * Quaternion.Euler(-2.5f, Random.Range(-0.7f, 0.7f), 0f);
        recoilPivot.localRotation = kick;
        yield return new WaitForSeconds(0.045f);
        float elapsed = 0f;
        Quaternion from = recoilPivot.localRotation;
        while (elapsed < 0.12f)
        {
            elapsed += Time.deltaTime;
            recoilPivot.localRotation = Quaternion.Slerp(from, recoilRestRotation, elapsed / 0.12f);
            yield return null;
        }
        recoilPivot.localRotation = recoilRestRotation;
    }

    private void NotifyAmmo()
    {
        if (CurrentWeapon != null)
            onAmmoChanged?.Invoke(CurrentAmmo, CurrentWeapon.magazineSize, CurrentWeapon.weaponName);
    }
}
