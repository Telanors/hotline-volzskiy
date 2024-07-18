using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    //private IInteractable currentFocusedObject;
    //[SerializeField] private Transform cameraTransform;
    //[SerializeField] private RightArmController rightArmController;
    //private void FixedUpdate()
    //{
    //    CheckOnFocus();
    //}
    //private void CheckOnFocus()
    //{
    //    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, rightArmController.pickUpDistance, rightArmController.pickUpableMask))
    //    {
    //        if (hit.transform.TryGetComponent(out IInteractable interactable))
    //        {
    //            if (currentFocusedObject != null && interactable != currentFocusedObject)
    //            {
    //                currentFocusedObject.FocusExit();
    //                currentFocusedObject = interactable;
    //                currentFocusedObject.FocusEnter();
    //            }
    //            else if (currentFocusedObject == null)
    //            {
    //                currentFocusedObject = interactable;
    //                currentFocusedObject.FocusEnter();
    //            }
    //        }
    //    }
    //    else if (currentFocusedObject != null)
    //    {
    //        currentFocusedObject?.FocusExit();
    //        currentFocusedObject = null;
    //    }
    //}
}
