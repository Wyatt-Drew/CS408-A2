using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableCanvas : MonoBehaviour
{
    private bool isMenu = true;
    private bool isCredits = false;
    public GameObject panel;
    private createAnimations createAnimations;
    public GameObject canvas;

    void Start()
    {
        createAnimations = FindObjectOfType<createAnimations>();
        this.GetComponent<AudioSource>().Play();
    }

    public void toggleMenu(bool toggle)
    {
        if (this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().Stop();
        }
        else
        {
            this.GetComponent<AudioSource>().Play();
        }
        canvas.GetComponent<Canvas>().enabled = toggle;
        isMenu = toggle;
        createAnimations.toggleMenu(toggle);
        if (isCredits)
            toggleCredits();
    }
    //Creative feature (Credits)
    public void toggleCredits()
    {
        isCredits = !isCredits;
        panel.SetActive(isCredits);
    }
}
