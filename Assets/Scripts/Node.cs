using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Renderer rend;
    private Color startColor;

    private GameObject turret;

    public Color hoverColor;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseDown ()
    {
        if (turret != null)
        {
            Debug.Log("Não é possível construir ai!");
        }
    }

    void OnMouserEnter ()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit ()
    {
        rend.material.color = startColor;
    }
}
