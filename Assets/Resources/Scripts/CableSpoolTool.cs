using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

/// <summary>
/// Enables connection and disconnection of electric terminals on fixtures
/// </summary>

public class CableSpoolTool : MonoBehaviour
{
    public TextMeshPro textIndicator;
    private int gauge = 12;

    GameObject selectedObject;
    GameObject sourceObject;
    GameObject targetObject;

    private int selectionStatus = 0;

    public void OnSelect()
    {
        textIndicator.enabled = true;
    }

    public void OnUnselect()
    {
        textIndicator.enabled = false;
    }

    // The latest object to enter the trigger is registered as the "selected" object
    private void OnTriggerEnter(Collider other)
    {
        selectedObject = other.gameObject;

        switch (selectionStatus)
        {
            case 0:
                textIndicator.text = "Objeto en rango: " + selectedObject.name;
                break;

            case 1:
                textIndicator.text = "Conectando desde: " + sourceObject.name + 
                                     "\nObjeto en rango: " + selectedObject.name;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        selectedObject = null;

        switch (selectionStatus)
        {
            case 0:
                textIndicator.text = "Sin Selección";
                break;

            case 1:
                textIndicator.text  = "Conectando desde: " + sourceObject.name + 
                                      "\nObjeto en rango: Sin Selección";
                break;
        }
    }

    // Method called once with every trigger pull, selects the highlighted object and enables connection logic
    public void OnActivated()
    {
        switch (selectionStatus)
        {
            // Selection of source object
            case 0:
               if (selectedObject != null)
                {
                    sourceObject = selectedObject;

                    sourceObject.GetComponent<Collider>().enabled = false;

                    textIndicator.text  = "Conectando desde: " + sourceObject.name;

                    selectionStatus = 1;
                } 
                break;

            // Selection of target object & apply logic
            case 1:
                if (selectedObject != null)
                {
                    targetObject = selectedObject;

                    textIndicator.text  = "Conexión exitosa desde " + sourceObject.name + "hacia " + targetObject.name;

                    List<GameObject> sourceConnections = sourceObject.GetComponent<ElectricalAttributes>().connections;
                    List<GameObject> targetConnections = targetObject.GetComponent<ElectricalAttributes>().connections;

                    sourceConnections.Add(targetObject);
                    targetConnections.Add(sourceObject);

                    sourceObject.GetComponent<Collider>().enabled = true;


                    Debug.Log("Got here!");

                    sourceObject = null;
                    targetObject = null;

                    selectionStatus = 0;
                }

                // If none selected, reset selection status
                else
                {
                    sourceObject = null;
                    textIndicator.text  = "Selección Cancelada";
                    selectionStatus = 0;
                }
                break;
        }
    }
}
