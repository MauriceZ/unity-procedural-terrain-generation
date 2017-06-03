using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
  public int x { get; private set; }
  public int y { get; private set; }
  public int Height { get; private set; }
  public Chunk Chunk { get; private set; }
  public BiomeState.Biome Biome { get; set; }

  public Block(Chunk chunk, int x, int y, int height, BiomeState biomeState) {
    this.x = x;
    this.y = y;
    Height = height;
    Chunk = chunk;

    biomeState.AssignBlockBiome(this);
  }
}
