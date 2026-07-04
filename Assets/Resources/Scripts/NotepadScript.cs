using UnityEngine;
using System;
using TMPro;

public class NotepadScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notepadText;

    void OnEnable()
    {
        TapeScript.OnTapeActivated += TapeActivated;
    }

    void OnDisable()
    {
        TapeScript.OnTapeActivated -= TapeActivated;
    }

    public void TapeActivated()
    {
        string height = GameObject.FindWithTag("Tape").GetComponent<TapeScript>().height;

        if (height != "")
        {
            notepadText.text += "\n" + height;
        }
    }
}
