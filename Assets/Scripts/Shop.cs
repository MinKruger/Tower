using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start ()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseStandardTurret ()
    {
        Debug.Log("Torre inicial comprada com sucesso!");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    public void PurchaseLoliGunTurret()
    {
        Debug.Log("Torre Loli com uma bazuca comprada com sucesso!");
        buildManager.SetTurretToBuild(buildManager.LoliGunTurretPrefab);
    }

    public void PurchaseUnityChanTurret()
    {
        Debug.Log("Torre Unity-chan com uma pistola comprada com sucesso!");
        buildManager.SetTurretToBuild(buildManager.UnityChanTurretPrefab);
    }
}
