using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Outline : MonoBehaviour
{
    Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
    }
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
