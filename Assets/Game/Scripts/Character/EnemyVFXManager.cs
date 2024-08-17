using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public VisualEffect attaclVFX;

    public ParticleSystem beingHit;

    public GameObject beingHitSphlePrefab;

    public void BurshFootStep()
    {
        footStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        attaclVFX.Play();
    }

    public void PlayBeingHit(Vector3 attackHit)
    {
        Vector3 forceForword = transform.position - attackHit;
        forceForword.Normalize();
        forceForword.y = 0;
        beingHit.transform.rotation = Quaternion.LookRotation(forceForword);
        beingHit.Play();

        var sphlePos = transform.position;
        sphlePos.y += 2;
        var beingSphle= Instantiate(beingHitSphlePrefab, sphlePos, Quaternion.identity);
        Destroy(beingSphle,10);

    }



}
