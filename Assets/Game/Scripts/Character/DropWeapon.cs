using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> weaponList;

    public void DropSwords()
    {
        foreach (var w in weaponList)
        {
            w.AddComponent<Rigidbody>();
            w.AddComponent<BoxCollider>();
            w.transform.parent = null;
        }
    }


}
