using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="GunData", menuName="Gun")]
public class GunData : ScriptableObject {

    public enum GunType { Rifles, Pistols, Shotguns, SniperRifles }
    [Header("Info")]
    [SerializeField] GunType typeOfGun;
    public string modelName;

    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 90;
    public int damage = 10;
    public int bulletRange = 10;

    //Aiming
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    [Tooltip("If gun type is sniper")]public float xScope = 2f;

    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //You only need to assign this if randomize recoil is off
    public Vector2[] recoilPattern;
}
