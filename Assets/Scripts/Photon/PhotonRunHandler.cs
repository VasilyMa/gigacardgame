using MemoryPack;
using UnityEngine;
using Photon.Pun;

public class PhotonRunHandler : MonoBehaviourPunCallbacks
{
    public static byte[] Serialize<T>(T obj) where T : IMemoryPackable<T>
    {
        try
        {
            return MemoryPackSerializer.Serialize(obj);
        }
        catch (MemoryPackSerializationException ex)
        {
            Debug.LogError($"Serialization failed: {ex.Message}");
            return null;
        }
    }

    public static T Deserialize<T>(byte[] bytes) where T : IMemoryPackable<T>
    {
        try
        {
            return MemoryPackSerializer.Deserialize<T>(bytes);
        }
        catch (MemoryPackSerializationException ex)
        {
            Debug.LogError($"Deserialization failed: {ex.Message}");
            return default;
        }
    }
}

