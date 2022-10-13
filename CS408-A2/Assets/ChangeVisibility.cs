using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVisibility : MonoBehaviour
{
    
    public void HideEvent()
    {
        //General items
        if (TryGetComponent(out Renderer b1))
            this.GetComponent<Renderer>().enabled = false;

        //Creative Feature *************************************

        //Terrain
        if (TryGetComponent(out Terrain b2))
            this.GetComponent<Terrain>().enabled = false;

        //Particle systems
        if (TryGetComponent(out ParticleSystem b3))
        {
            ParticleSystem ps = this.GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = false;
            gameObject.GetComponent<ParticleSystem>().Clear();
        }
        //Children that contain a renderer
        foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
        {
            rend.enabled = false;
        }

    }
    public void UnhideEvent()
    {
        if (TryGetComponent(out Renderer b1))
            this.GetComponent<Renderer>().enabled = true;

        if (TryGetComponent(out Terrain b2))
            this.GetComponent<Terrain>().enabled = true;

        if (TryGetComponent(out ParticleSystem b3))
        {
            ParticleSystem ps = this.GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = true;
            gameObject.GetComponent<ParticleSystem>().Clear();
        }
        foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
        {
            rend.enabled = true;
        }
    }
}
