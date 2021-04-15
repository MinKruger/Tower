using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("Mais de uma instanciação feita");
            return;
        }
        instance = this;
    }

    public GameObject standardTurretPrefab;
    public GameObject LoliGunTurretPrefab;
    public GameObject UnityChanTurretPrefab;

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild ()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild (GameObject turret)
    {
        turretToBuild = turret;
    }
}
