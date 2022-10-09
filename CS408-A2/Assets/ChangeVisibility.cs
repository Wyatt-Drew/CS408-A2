using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVisibility : MonoBehaviour
{

    public void HideEvent()
    {
        Debug.Log("you're in");
        //instance.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Renderer>().enabled = false;
    }
    public void UnhideEvent()
    {
        this.GetComponent<Renderer>().enabled = true;
    }
}
