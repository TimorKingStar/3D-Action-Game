using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCoster : MonoBehaviour
{

    private Collider _damageCastercollider;
    public int damage = 30;
    public string targetTag;

    private List<Collider> _damagedTargeList;

    private void Awake()
    {
        _damageCastercollider = GetComponent<Collider>();
        _damageCastercollider.enabled = false;
        _damagedTargeList = new List<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag)&& !_damagedTargeList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();
            if (targetCC!=null)
            {
                targetCC.ApplyDamage(damage,transform.parent.position);
                var playerVFX= transform.parent.GetComponent<PlayerVFXManager>();
                if (playerVFX!=null)
                {
                    RaycastHit hit;
                    Vector3 originPos = transform.position + (-_damageCastercollider.bounds.extents.z) * transform.forward;

                    bool isHit = Physics.BoxCast(originPos, _damageCastercollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCastercollider.bounds.extents.z, 1 << 6);
                    if (isHit)
                    {
                        playerVFX.PlaySlash(hit.point+new Vector3(0,0.5f,0));

                    }
                }
            }

            _damagedTargeList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargeList.Clear();
        _damageCastercollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damagedTargeList.Clear();
        _damageCastercollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (_damageCastercollider==null)
        {
            _damageCastercollider = GetComponent<Collider>();
        }

        RaycastHit hit;
        Vector3 originPos = transform.position + (- _damageCastercollider.bounds.extents.z) * transform.forward;

        
        bool isHit = Physics.BoxCast(originPos, _damageCastercollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCastercollider.bounds.extents.z, 1 << 6);
        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point,0.3f);
            
        }

    }
}
