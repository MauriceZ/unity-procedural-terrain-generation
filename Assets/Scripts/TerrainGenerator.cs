using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
  public Chunk chunkPrefab;

  public int xChunks;
  public int zChunks;
  public int maxHeight;
  public int waterHeight;
  public float scale;
  public int lookAhead;
  public int lookAheadLimit;

  public HeightMap HeightMap { get; private set; }
  public BiomeState BiomeState { get; private set; }

  private GameObject player;
  private Dictionary<Vector2, Chunk> renderedChunks;
  private Dictionary<Vector2, Chunk> nonRenderedChunks;

  // Use this for initialization
  void Start () {
    player = GameObject.Find("Player");

    var seed = Random.Range(100, 1000);
    var perlinNoise = new PerlinNoise(seed, scale);
    HeightMap = new HeightMap(perlinNoise, maxHeight, waterHeight);
    BiomeState = new BiomeState(200f, waterHeight, maxHeight);

    renderedChunks = new Dictionary<Vector2, Chunk>();
    nonRenderedChunks = new Dictionary<Vector2, Chunk>();

    for (int i = -xChunks/2; i < xChunks/2; i++) {
      for (int j = -zChunks/2; j < zChunks/2; j++) {
        generateChunk(i, j);
      }
    }

    StartCoroutine(generateInfiniteTerrain());
	}

  private void generateChunk(int x, int y) {
    var chunkOrigin = new Vector2(x, y);

    if (renderedChunks.ContainsKey(chunkOrigin))
      return;

    if (nonRenderedChunks.ContainsKey(chunkOrigin)) {
      var chunk = nonRenderedChunks[chunkOrigin];
      chunk.gameObject.SetActive(true);
      nonRenderedChunks.Remove(chunkOrigin);
      renderedChunks[chunkOrigin] = chunk;
    } else {
      var chunk = Chunk.Instantiate(this, x, y) as Chunk;
      renderedChunks[chunkOrigin] = chunk;
    }
  }

  private IEnumerator generateInfiniteTerrain() {
    // infinite world generation
    var delay = new WaitForSeconds(1f);

    while (true) {
      var playerPos = new Vector2(player.transform.position.x, player.transform.position.z);

      var curChunkX = (playerPos.x + Chunk.CHUNK_SIZE.x/2) / Chunk.CHUNK_SIZE.x;
      var curChunkY = (playerPos.y + Chunk.CHUNK_SIZE.y/2) / Chunk.CHUNK_SIZE.y;

      var curChunkOrigin = new Vector2((int)curChunkX, (int)curChunkY);

      foreach (var chunkOrigin in new List<Vector2>(renderedChunks.Keys)) { // iterating over new list of keys to prevent out of sync error
        // disable terrains that are too far
        if (Vector2.Distance(chunkOrigin, curChunkOrigin) >= lookAheadLimit) {
          var chunk = renderedChunks[chunkOrigin];
          chunk.gameObject.SetActive(false);
          renderedChunks.Remove(chunkOrigin);
          nonRenderedChunks[chunkOrigin] = chunk;
        }
      }

      for (int x = (int)curChunkOrigin.x - lookAhead; x <= curChunkOrigin.x + lookAhead; x++) {
        for (int y = (int)curChunkOrigin.y - lookAhead; y <= curChunkOrigin.y + lookAhead; y++) {
          if (!renderedChunks.ContainsKey(new Vector2(x, y))) {
            generateChunk(x, y);
          }
        }
      }

      yield return delay;
    }

  }
}
