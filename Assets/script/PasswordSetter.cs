using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordSetter : MonoBehaviour
{
    public TMP_InputField PasswordField;
    private void Start()
    {
        UpdatePassword();
    }
    public void UpdatePassword()
    {
        if(PasswordField.text == "")
        {
            PasswordField.text = "default";
        }
        FireBaseManager.main.password = PasswordField.text;
    }
}
