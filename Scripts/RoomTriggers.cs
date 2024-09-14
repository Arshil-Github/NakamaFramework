using System;
using UnityEngine;

public class RoomTriggers : MonoBehaviour
{
    public enum Room
    {
        OrbRoom,
        PlantRoom,
        BackRoom
    }

    public Room type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.SetCurrentRoom(type);
        }
    }
}
