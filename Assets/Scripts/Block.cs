using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
  public Vector2 position { get; private set; }
  public int Height { get; private set; }

  public Block(int x, int y, int height) {
    position = new Vector2(x, y);
    Height = height;
  }
}
