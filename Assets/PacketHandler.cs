using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandler : MonoBehaviour
{
    Rigidbody2D _rb;
    Collider2D _coll;
    SpriteRenderer _sr;
    ParticleSystem _ps;

    //state
    int _ownerIndex = -1;
    public int OwnerIndex => _ownerIndex;

    public void InitializePacket(Vector2 velocity, int ownerIndex)
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = velocity;
        transform.up = velocity;
        _coll = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ps = GetComponent<ParticleSystem>();
        _ownerIndex = ownerIndex;
        _sr.color = ColorLibrary.Instance.PlayerColors[_ownerIndex -1];
    }

    public void DeactivatePacket()
    {
        _rb.simulated = false;
        _coll.enabled = false;
        _sr.enabled = false;
        _ps.Stop();
        Destroy(gameObject, 3);
    }
}
