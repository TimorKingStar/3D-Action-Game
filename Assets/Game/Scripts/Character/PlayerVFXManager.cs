using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect _footStep;
    public ParticleSystem blade01;
    public ParticleSystem blade02;
    public ParticleSystem blade03;

    public VisualEffect slash;

    public VisualEffect heal;

    public void Update_FootStep(bool state)
    {
        if (state)
        {
            _footStep.Play();
        }
        else
        {
            _footStep.Stop();
        }
    }


    public void PlayBlade01()
    {
        blade01.Play();
    }

    public void PlayBlade02()
    {
        blade02.Play();
    }

    public void PlayBlade03()
    {
        blade03.Play();
    }

    public void StopBlade()
    {
        blade01.Simulate(0);
        blade01.Stop();

        blade02.Simulate(0);
        blade02.Stop();

        blade03.Simulate(0);
        blade03.Stop();
    }
        
    public void PlaySlash(Vector3 point)
    {
        slash.transform.position = point;
        slash.Play();
    }

    public void PlayHeal()
    {
        heal.Play();
    }

}
