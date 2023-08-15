using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using DG.Tweening;
public class GunController : MonoBehaviour
{
    public static GunController instance;

    [Header("REFERENCE")]
    public FirstPersonMobileTools.DynamicFirstPerson.MovementController movement;
    public enum GunType {Rifles, Pistols, Shotguns, SniperRifles }
    [Header("GUN DATA")]
    public GunData scriptableGun;
    [SerializeField]
    GunType typeOfGun;
    [Header("GUN SETTINGS")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 90;
    public int damage = 10;
    public int bulletRange = 10;
    //Variables that change throughout code
    private bool _canShoot;
    private int _currentAmmoInClip;
    private int _ammoInReserve;
    public bool shootingBtnIsPress = false;
    //Muzzleflash
    public Image muzzleFlashImage;
    public Sprite[] flashes;
    public TextMeshProUGUI ammo;
    //Aiming
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    bool isAiming = false;
    public float aimSmoothing = 10;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 1;
    private Vector2 _currentRotation;
    public float weaponSwayAmount = 10;

    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //You only need to assign this if randomize recoil is off
    public Vector2[] recoilPattern;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _currentAmmoInClip = clipSize;
        _ammoInReserve = reservedAmmoCapacity;
        _canShoot = true;
        ammo.text = clipSize + "/" + clipSize;
    }

    private void Update()
    {
        DetermineAim();
        
        if(shootingBtnIsPress == true)
        {
            movement.Walk_Speed = 2f;
            Shooting();
        }
    }

    private void DetermineAim()
    {
        if(isAiming == false)
        {
            Vector3 target = normalLocalPosition;

            Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
            Camera.main.DOFieldOfView(60, 1f);
            movement.Walk_Speed = 6;
            transform.localPosition = desiredPosition;
        }
        else
        {
            Vector3 target = normalLocalPosition;
            target = aimingLocalPosition;
            movement.Walk_Speed = 2f;
            Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
            Camera.main.DOFieldOfView(30, 1f);
            transform.localPosition = desiredPosition;
        }

    }

    private void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;

        if (randomizeRecoil)
        {
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

            _currentRotation += recoil;
        }
        else
        {
            int currentStep = clipSize + 1 - _currentAmmoInClip;
            currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);

            _currentRotation += recoilPattern[currentStep];
        }
    }

    private IEnumerator ShootGun()
    {
        DetermineRecoil();
        StartCoroutine(MuzzleFlash());

        RaycastForEnemy();

        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }

    private void RaycastForEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit,bulletRange,1 << LayerMask.NameToLayer("Enemy")))
        {
            try
            {
                Debug.Log(hit.transform.name);
                hit.transform.GetComponent<EnemyData>().currentHp = hit.transform.GetComponent<EnemyData>().currentHp - damage;
            }
            catch
            {

            }
        }
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlashImage.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleFlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.sprite = null;
        muzzleFlashImage.color = new Color(0, 0, 0, 0);
    }

    public void Shooting()
    {
        if ( _canShoot && _currentAmmoInClip > 0)
        {
            _canShoot = false;
            _currentAmmoInClip--;
            ammo.text = _currentAmmoInClip + "/" + clipSize;
            StartCoroutine(ShootGun());
        }
    }

    public void ReloadBullet()
    {
        if ( _currentAmmoInClip < clipSize && _ammoInReserve > 0)
        {
            int amountNeeded = clipSize - _currentAmmoInClip;

            if (amountNeeded >= _ammoInReserve)
            {
                _currentAmmoInClip += _ammoInReserve;
                _ammoInReserve -= amountNeeded;
            }
            else
            {
                _currentAmmoInClip = clipSize;
                _ammoInReserve -= amountNeeded;
            }
            ammo.text = _currentAmmoInClip + "/" + clipSize;
        }
    }
    public void Aiming()
    {
        isAiming = !isAiming;   
    }
}