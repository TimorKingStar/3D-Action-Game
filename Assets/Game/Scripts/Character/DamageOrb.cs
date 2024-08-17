using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{

    public float speed = 2f;
    public int damage = 10;
    public ParticleSystem hitVFX;

    private Rigidbody _rg;

    private void Awake()
    {
        _rg = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        _rg.MovePosition(transform.position+ speed * Time.deltaTime * transform.forward);
    }
    private void OnTriggerEnter(Collider other)
    {
        Character cc = other.gameObject.GetComponent<Character>();
        if (cc!=null&& cc.isPlayer)
        {
            cc.ApplyDamage(damage, transform.position);
        }

        Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
