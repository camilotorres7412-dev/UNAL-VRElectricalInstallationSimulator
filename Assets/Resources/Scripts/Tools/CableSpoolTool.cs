using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Enables connection and disconnection of electric terminals on fixtures
/// </summary>

public class CableSpoolTool : MonoBehaviour
{
    [SerializeField] private TextMeshPro textIndicator;

    [SerializeField] private ElectricalManager electricalManager;

    private int gauge = 12;

    GameObject selectedObject;
    GameObject sourceObject;

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

    private void OnTriggerExit()
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

                    textIndicator.text  = "Conectando desde: " + sourceObject.name;

                    selectionStatus = 1;
                } 
                break;

            // Selection of target object & apply logic
            case 1:
                if (selectedObject != null)
                {
                    // Prevent self-connections
                    if (sourceObject == selectedObject)
                    {
                        sourceObject = null;
                        textIndicator.text  = "Selección abortada: No se permite conectar un objeto a si mismo";
                        selectionStatus = 0;
                        return;
                    }

                    // Get connection tables for both objects
                    List<GameObject> sourceConnections = sourceObject.GetComponent<ElectricalAttributes>().cableTargets;
                    List<GameObject> targetConnections = selectedObject.GetComponent<ElectricalAttributes>().cableTargets;

                    // Check if an exactly matching entry does not exist
                    // This logic allows for duplicate connection during power up
                    // Also allows for multiple cable gauges to exist
                    // Clean up
                    if (!sourceConnections.Contains(selectedObject) || !targetConnections.Contains(sourceObject))
                    {
                        sourceConnections.Add(selectedObject);
                        targetConnections.Add(sourceObject);

                        textIndicator.text  = "Conexión exitosa entre " + sourceObject.name + "y " + selectedObject.name;

                        sourceObject = null;

                        selectionStatus = 0;
                    }

                    else
                    {
                        sourceObject = null;
                        textIndicator.text  = "Selección abortada: No se permiten conexiones duplicadas";
                        selectionStatus = 0;
                        return;
                    }
                }

                // If none selected, reset selection status
                else
                {
                    sourceObject = null;
                    textIndicator.text  = "Selección Cancelada";
                    selectionStatus = 0;
                    return;
                }
                break;
        }
    }
}
