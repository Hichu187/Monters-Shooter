using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public static GunSystem instance;
    private void Awake()
    {
        instance = this;
    }

    public List<GunData> gunDatas;
    public List<GameObject> gunModels;

    public GunData currentGun;
}
