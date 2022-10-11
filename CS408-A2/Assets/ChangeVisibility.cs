using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVisibility : MonoBehaviour
{
    
    public void HideEvent()
    {

        this.GetComponent<Renderer>().enabled = false;
    }
    public void UnhideEvent()
    {
        this.GetComponent<Renderer>().enabled = true;
    }
}
