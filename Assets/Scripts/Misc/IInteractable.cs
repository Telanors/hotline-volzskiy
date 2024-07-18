using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool isFocuse { get; set; }
    void FocusEnter();
    void FocusExit();
}
