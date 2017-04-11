using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMap {
  private PerlinNoise perlinNoise;
  private int maxHeight;
  private Dictionary<Chunk, float[,]> chunkNoiseMaps;

  public HeightMap(PerlinNoise perlinNoise, int maxHeight) {
    this.perlinNoise = perlinNoise;
    this.maxHeight = maxHeight;
    chunkNoiseMaps = new Dictionary<Chunk, float[,]>();
  }

  public int GetHeight(Chunk chunk, int x, int z) {
    if (!chunkNoiseMaps.ContainsKey(chunk)) {
      chunkNoiseMaps[chunk] = perlinNoise.GetNoiseMap(chunk.lowerX, chunk.upperX + 1, chunk.lowerY, chunk.upperY + 1);
    }

    return Mathf.RoundToInt(chunkNoiseMaps[chunk][x, z] * maxHeight);
  }
}
