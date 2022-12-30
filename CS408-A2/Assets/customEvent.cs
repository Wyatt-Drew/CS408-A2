using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creative Feature *************************************
public class customEvent : MonoBehaviour
{
    Camera[] allCameras;
    //Insert any custom event here
    void testEvent()
    {
        Debug.Log("Test event worked");
    }
    void useThisCamera()
    {
        allCameras = FindObjectsOfType<Camera>();
        //Camera.allCameras(Camera[] cameras);
        for (int i = 0; i < Camera.allCamerasCount; i++)
        {
            allCameras[i].enabled = false;
        }
        this.GetComponent<Camera>().enabled = true;
    }
    void playMusic()
    {
        if (this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().Stop();
        }
        else
        {
            this.GetComponent<AudioSource>().Play();
        }
    }
    void hideTerrain()
    {
        //this.GetComponent<Terrain>().enabled = false;
    }
    void showTerrain()
    {
        //this.GetComponent<Terrain>().enabled = true;
    }
    void hideParticles()
    {
        //ParticleSystem ps = this.GetComponent<ParticleSystem>();
        //var emission = ps.emission;
        //emission.enabled = false;
        //gameObject.GetComponent<ParticleSystem>().Clear();
    }
    void showParticles()
    {
        //this.GetComponent<ParticleSystem>().Play();
        //ParticleSystem ps = this.GetComponent<ParticleSystem>();
        //var emission = ps.emission;
        //emission.enabled = true;
        //gameObject.GetComponent<ParticleSystem>().Clear();
    }
    void hideChildren()
    {
        //foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
        //{ 
        //    rend.enabled = false; 
        //}
    }
    void showChildren()
    {
        //foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
        //{ 
        //    rend.enabled = true; 
        //}
    }
}
