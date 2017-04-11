using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
  public int x { get; private set; }
  public int y { get; private set; }
  public int Height { get; private set; }
  public Color Color { get; private set; }

  public Block(int x, int y, int height) {
    this.x = x;
    this.y = y;
    Height = height;
    
    if (Height <= 17) {
      Color = Color.blue;
    } else if (Height == 18) {
      Color = Color.yellow;
    } else {
      Color = new Color(0.13f, 0.55f, 0.13f);
    }
  }
}
