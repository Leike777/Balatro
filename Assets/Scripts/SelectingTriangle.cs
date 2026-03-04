using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingTriangle : MonoBehaviour
{
    
    public Vector2[] positions;

    private void OnEnable()
    {
        GetComponent<RectTransform>().localPosition = positions[0];
    }

    public void SetPosition(short index)
    {
        GetComponent<RectTransform>().localPosition = positions[index];
    }
}
