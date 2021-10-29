using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCoin : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] RotationAxis rotAxis;

    // Update is called once per frame
    void Update()
    {
        if (rotAxis == RotationAxis.X)
        {
            transform.Rotate(rotSpeed * Time.deltaTime, 0f, 0f);
        } else if (rotAxis == RotationAxis.Y)
        {
            transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
        } else if (rotAxis == RotationAxis.Z)
        {
            transform.Rotate(0f, 0f, rotSpeed * Time.deltaTime);
        }
    }
}

public enum RotationAxis
{
    X,
    Y,
    Z
}