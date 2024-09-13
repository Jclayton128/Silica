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
    float _lifetimeRemaining;
    int _ownerIndex = -1;
    public int OwnerIndex => _ownerIndex;

    public void InitializePacket(int ownerIndex)
    {
        _rb = GetComponent<Rigidbody2D>();

        _coll = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ps = GetComponent<ParticleSystem>();
        _ownerIndex = ownerIndex;
        _sr.color = ColorLibrary.Instance.PlayerColors[_ownerIndex -1];
    }

    public void ActivatePacket(Vector2 velocity, float lifetime)
    {
        enabled = true;
        _rb.simulated = true;
        _coll.enabled = true;
        _sr.enabled = true;
        _ps.Play();
        _rb.velocity = velocity;
        transform.up = velocity;
        _lifetimeRemaining = lifetime;
    }


    public void DeactivatePacket()
    {
        _rb.simulated = false;
        _coll.enabled = false;
        _sr.enabled = false;
        _ps.Stop();
        enabled = false;
        //Destroy(gameObject, 0.1f);
    }

    private void Update()
    {
        _lifetimeRemaining -= Time.deltaTime;
        if (_lifetimeRemaining <= 0)
        {
            DeactivatePacket();
        }
    }
}
