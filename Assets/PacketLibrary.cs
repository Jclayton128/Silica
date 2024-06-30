using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketLibrary : MonoBehaviour
{
    public static PacketLibrary Instance { get; private set; }

    [SerializeField] PacketHandler _packetPrefab = null;

    private void Awake()
    {
        Instance = this;
    }

    public PacketHandler GetPacketPrefab()
    {
        return _packetPrefab;
    }

}
