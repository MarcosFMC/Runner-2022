
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float chunkLenght;


    public Chunk ShowChunk()
    {
        BroadcastMessage("OnShowChunk", SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(true);
        return this;
    }

    public Chunk HideChunk()
    {
        gameObject.SetActive(false);
        return this;
    }
}
