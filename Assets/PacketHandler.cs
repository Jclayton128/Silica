using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandler : MonoBehaviour
{
    Rigidbody2D rb;

    public void InitializePacket(Vector2 velocity)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        transform.up = velocity;

    }
}
