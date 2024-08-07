using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sensitivity : MonoBehaviour
{
    private float sens;
    public TextMeshPro sensText; //Sensitivity text
    public TextMeshPro increase01Text;
    public TextMeshPro increase1Text;
    public TextMeshPro decrease01Text;
    public TextMeshPro decrease1Text;

    void Start()
    {
        sens = 0.5f;
    }
    public float GetSensitivity()
    {
        return sens;
    }
    // Update is called once per frame
    void Update()
    {
        if (sens <= 0)
        {
            sens = 0.01f;
            sensText.text = "Sensitivity: " + sens;
        }
    }
    public void changeSens(string name)
    {
        if(name == "Increase 0.1 Text")
        {
            sens += 0.1f;
            sens = Mathf.Round(sens * 100f) / 100f;
            sensText.text = "Sensitivity: " + sens;
        }else if (name == "Increase 0.01 Text")
        {
            sens += 0.01f;
            sens = Mathf.Round(sens * 100f) / 100f;
            sensText.text = "Sensitivity: " + sens;
        }else if (name == "Decrease 0.1 Text")
        {
            sens -= 0.1f;
            sens = Mathf.Round(sens * 100f) / 100f;
            sensText.text = "Sensitivity: " + sens;
        }
        else if (name == "Decrease 0.01 Text")
        {
            sens -= 0.01f;
            sens = Mathf.Round(sens * 100f) / 100f;
            sensText.text = "Sensitivity: " + sens;
        }
    }
}
