
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    void OnEnable()
    {
        InputController.moveEvent += OnMoveEvent;
        InputController.fireEvent += OnFireEvent;
    }

    void OnDisable()
    {
        InputController.moveEvent -= OnMoveEvent;
        InputController.fireEvent -= OnFireEvent;
    }

    private void OnFireEvent(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Fire " + e.info);
    }

    private void OnMoveEvent(object sender, InfoEventArgs<Point> e)
    {
        Debug.Log("Move " + e.info);
    }
}
