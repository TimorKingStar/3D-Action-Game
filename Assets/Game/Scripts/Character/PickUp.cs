using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Heal,
        Coin,
    }

    public PickUpType type;

    public int value=20;
    public ParticleSystem collige;

     
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            if (collige!=null)
            {
                Instantiate(collige, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
