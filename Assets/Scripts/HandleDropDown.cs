using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleDropDown : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    protected int[] amounts;
    public InputField valueField;

    private void Awake()
    {
        
        amounts = new int[dropdown.options.Capacity];
        for (int i = 1; i < dropdown.options.Capacity - 1; i++) {
            amounts[i] = int.Parse(dropdown.options[i].text.Split(' ')[0]);
        }
    }

    void Update()
    {   
        if (gameObject.GetComponentInChildren<ScrollRect>() != null) {
            gameObject.GetComponentInChildren<ScrollRect>().scrollSensitivity = 30;
        }
    }

    public void _HandlePickTarget() {
        valueField.text = amounts[dropdown.value].ToString();
        if (dropdown.value == dropdown.options.Capacity - 1) {
            InteractableInputField();
        }
    }

    protected void InteractableInputField() {
        valueField.interactable = true;
    }

}
