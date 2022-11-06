using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovementSpeed;
    public float SpreadSpeed;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) Movement(1);
        if (Input.GetKey(KeyCode.S)) Movement(-1);
        if (Input.GetKey(KeyCode.D)) Spread(1);
        if (Input.GetKey(KeyCode.A)) Spread(-1);
    }

    private void Movement(int direction)
    {
        transform.Translate(transform.forward * MovementSpeed * direction, Space.World);
    }

    private void Spread(int direction)
    {
        transform.Translate(transform.right * SpreadSpeed * direction, Space.World);
    }
}
