using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeState {
  public static int UPPER_TEMPERATURE = 256;
  public static Color COLOR_GREEN = new Color(0.13f, 0.55f, 0.13f);

  public enum BiomeKind { Water, Snow, Forest, Grass, Sand, Rock };

  public struct Biome {
    public BiomeKind kind;
    public Color color;

    public Biome(BiomeKind kind, Color color) {
      this.kind = kind;
      this.color = color;
    }
  }

  public static Biome BIOME_WATER = new Biome(BiomeKind.Water, Color.blue);
  public static Biome BIOME_SNOW = new Biome(BiomeKind.Snow, Color.white);
  public static Biome BIOME_GRASS = new Biome(BiomeKind.Grass, COLOR_GREEN);
  public static Biome BIOME_FOREST = new Biome(BiomeKind.Forest, COLOR_GREEN);
  public static Biome BIOME_SAND = new Biome(BiomeKind.Sand, Color.yellow);
  public static Biome BIOME_ROCK = new Biome(BiomeKind.Sand, Color.gray);

  private PerlinNoise perlinNoise;
  private Dictionary<Chunk, float[,]> chunkNoiseMaps;
  private int waterHeight;
  private int maxHeight;

  public BiomeState(float scale, int waterHeight, int maxHeight) {
    var seed = Random.Range(100, 1000);
    this.perlinNoise = new PerlinNoise(seed, scale);
    this.waterHeight = waterHeight;
    this.maxHeight = maxHeight;
    chunkNoiseMaps = new Dictionary<Chunk, float[,]>();
  }

  public int GetTemperature(Chunk chunk, int x, int z) {
    if (!chunkNoiseMaps.ContainsKey(chunk)) {
      chunkNoiseMaps[chunk] = perlinNoise.GetNoiseMap(chunk.lowerX, chunk.upperX + 1, chunk.lowerY, chunk.upperY + 1);
    }

    return Mathf.RoundToInt(chunkNoiseMaps[chunk][x, z] * UPPER_TEMPERATURE);
  }

  private Biome GetBiome(Block block) {
    var temperature = GetTemperature(block.Chunk, block.x - block.Chunk.lowerX, block.y - block.Chunk.lowerY);

    if (temperature < 50 && block.Height > maxHeight * 0.6f) {
      return BIOME_SNOW;
    } else if (temperature <= 250 && temperature >= 235 && block.Height < maxHeight * 0.7f) {
      return BIOME_SAND;
    } else if (temperature < 150 && temperature >= 100 && block.Height > waterHeight + 6) {
      return BIOME_FOREST;
    }

    return BIOME_GRASS;
  }

  public Color GetColor(int height) {
    if (height <= waterHeight)
      return Color.blue;
    else if (height == waterHeight + 1)
      return Color.yellow;
    else
      return COLOR_GREEN;
  }

  public void AssignBlockBiome(Block block) {
    if (block.Height <= waterHeight + 1) {
      if (block.Height == waterHeight + 1) {
        block.Biome = BIOME_SAND;
      } else {
        block.Biome = BIOME_WATER;
      }
    } else if (block.Height >= maxHeight - 10) {
      block.Biome = BIOME_SNOW;
    } else if (block.Height >= maxHeight - 15) {
      block.Biome = BIOME_ROCK;
    } else {
      block.Biome = GetBiome(block);
    }
  }
}
