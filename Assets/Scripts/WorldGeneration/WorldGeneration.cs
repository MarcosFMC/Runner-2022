using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunk = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    [SerializeField]
    private int firstChunkSpawnPosition = -10;
    [SerializeField]
    private int chunkOnScreen = 5;
    [SerializeField]
    private float despawnDistance = 5.0f;

    [SerializeField]
    private List<GameObject> chunkPrefab;
    [SerializeField]
    private Transform cameraTransform;

    #region TO DELETE $$
    private void Awake()
    {
        ResetWorld();
    }
    #endregion  
    private void Start()
    {
        //Check if  we have an empty chunkPrefab list
        if(chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefab found on the world generator, please  assign some chunks!");
            return;
        }

        //Try to assing the cameraTransform
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("We ve assigned a cameraTransform automaticly to the Camera.main");
        }
    }
    public void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunk.Peek();
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLenght + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
       
    }

    

    private void SpawnNewChunk()
    {
        //Get a random index  for which  prefab to spawn
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        //Does it already exist  within our pool
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "(Clone)"));

        //Create a chunk, if were not able to find one to reuse.
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        //Place the object , and show it 
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLenght;
        //Store the value, to reuse in our pool
        activeChunk.Enqueue(chunk);
        chunk.ShowChunk();

    }

    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunk.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }

    public void ResetWorld()
    {
        //Reset chunkSpawnZ
        chunkSpawnZ = firstChunkSpawnPosition;

        //Destroy all chunks
        for (int i = activeChunk.Count; i != 0; i--)
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
