using UnityEngine;
using System.Collections;

public class PerlinNoise {
  private long seed;
  private int octaves;
  private float persistence;
  private float lacunarity;

  public PerlinNoise(long seed){
    this.seed = seed;
    this.persistence = 0.5f;
    this.lacunarity = 2;
    this.octaves = 4;
  }

  public float[,] GetNoise(int x, int z, float scale){
    var noiseMap = new float[x, z];

    var greatestNoise = float.MinValue;
    var smallestNoise = float.MaxValue;

    for (int i = 0; i < x; i++) {
      for (int j = 0; j < z; j++) {

        var frequency = 1f;
        var amplitude = 1f;
        var noise = 0f;

        for (int octave = 0; octave < octaves; octave++) {
          var noiseValue = Mathf.PerlinNoise(i/scale * frequency + seed, j/scale * frequency + seed);
          noise += noiseValue * amplitude;

          amplitude *= persistence;
          frequency *= lacunarity;
        }

        noiseMap[i, j] = noise;
        
        if (noise > greatestNoise)
          greatestNoise = noise;
        if (noise < smallestNoise)
          smallestNoise = noise;
      }
    }

    // normalize
    for (int i = 0; i < x; i++) {
      for (int j = 0; j < z; j++) {
        noiseMap[i, j] = Mathf.InverseLerp(smallestNoise, greatestNoise, noiseMap[i, j]);
      }
    }

    return noiseMap;
  }
}
