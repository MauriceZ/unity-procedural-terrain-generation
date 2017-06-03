using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
  public static Vector2 CHUNK_SIZE = new Vector2(25, 25);
  public static int BLOCK_SIZE = 1;

  public static Chunk Instantiate(TerrainGenerator terrainGenerator, int chunkX, int chunkY) {
    var chunk = Instantiate (terrainGenerator.chunkPrefab, terrainGenerator.transform) as Chunk;
    chunk.terrainGenerator = terrainGenerator;
    chunk.origin = new Vector2(chunkX * CHUNK_SIZE.x, chunkY * CHUNK_SIZE.y);
    chunk.Generate();
    return chunk;
  }

  public Tree treePrefab;

  public Vector2 origin;
  private Block[,] blocks;
  private TerrainGenerator terrainGenerator;

  public int lowerX { get { return (int)origin.x - (int)CHUNK_SIZE.x/2; }}
  public int upperX { get { return (int)origin.x + (int)CHUNK_SIZE.x/2; }}
  public int lowerY { get { return (int)origin.y - (int)CHUNK_SIZE.y/2; }}
  public int upperY { get { return (int)origin.y + (int)CHUNK_SIZE.y/2; }}

  private List<Vector3> vertices;
  private List<Vector2> uv;
  private List<int> triangles;
  private List<Color> colors;

  public void Generate() {
    blocks = new Block[(int)CHUNK_SIZE.x, (int)CHUNK_SIZE.y];

    for (int x = 0; x < CHUNK_SIZE.x; x++) {
      for (int y = 0; y < CHUNK_SIZE.y; y++) {
        var height = terrainGenerator.HeightMap.GetHeight(this, x, y);
        blocks[x, y] = new Block(this, lowerX + x, lowerY + y, height, terrainGenerator.BiomeState);

        if (blocks[x, y].Biome.kind == BiomeState.BiomeKind.Forest && Random.value < 0.05f) {
          Tree.Instantiate(this, blocks[x, y]);
        }
      }
    }

    RenderChunk();
  }

  private void RenderChunk() {
    vertices = new List<Vector3>();
    uv = new List<Vector2>();
    triangles = new List<int>();
    colors = new List<Color>();

    for (int x = 0; x < CHUNK_SIZE.x; x++) {
      for (int y = 0; y < CHUNK_SIZE.y; y++) {
        addFaceVertices(x, y, Vector3.left);
        addFaceVertices(x, y, Vector3.right);
        addFaceVertices(x, y, Vector3.up);
        addFaceVertices(x, y, Vector3.down);
        addFaceVertices(x, y, Vector3.forward);
        // addFaceVertices(x, y, Vector3.back);
      }
    }

    var mesh = GetComponent<MeshFilter>().mesh;
    mesh.vertices = vertices.ToArray();
    mesh.SetColors(colors);
    mesh.colors = colors.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uv.ToArray();
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    GetComponent<MeshCollider>().sharedMesh = mesh;
  }

  private void addFaceVertices(int localX, int localY, Vector3 face) {
    var block = blocks[localX, localY];

    int x = block.x;
    int y = block.y;

    if (face == Vector3.back) {
      vertices.Add(new Vector3(x, block.Height, y));
      vertices.Add(new Vector3(x, block.Height, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y));

      updateColor(block);
      updateUV();
    } else if (face == Vector3.forward) {
      vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y));
      vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y));

      updateColor(block);
      updateUV();
    } else {
      int adjX = x + (int)face.x;
      int adjY = y + (int)face.y;

      if (!coordsWithinBounds(adjX, adjY) || blocks[adjX, adjY].Height != block.Height) {
        if (face == Vector3.left) {
          vertices.Add(new Vector3(x, block.Height, y));
          vertices.Add(new Vector3(x, block.Height, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y));
        } else if (face == Vector3.right) {
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y + BLOCK_SIZE));
        } else if (face == Vector3.up) {
          vertices.Add(new Vector3(x, block.Height, y));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y));
        } else if (face == Vector3.down) {
          vertices.Add(new Vector3(x, block.Height, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
        }

        updateColor(block);
        updateUV();
      }
    }
  }

  private void updateColor(Block block) {
    for (int i = 0; i < 4; i++) {
      colors.Add(block.Biome.color);
    }
  }

  private void updateUV() {
    triangles.Add(vertices.Count - 4);
    triangles.Add(vertices.Count - 3);
    triangles.Add(vertices.Count - 2);
   
    triangles.Add(vertices.Count - 2);
    triangles.Add(vertices.Count - 1);
    triangles.Add(vertices.Count - 4);
   
    uv.Add(new Vector2(0, 0));
    uv.Add(new Vector2(0, 1));
    uv.Add(new Vector2(1, 1));
    uv.Add(new Vector2(1, 0));
  }

  private bool coordsWithinBounds(int x, int y) {
    return 0 <= x && x < CHUNK_SIZE.x && 0 <= y && y < CHUNK_SIZE.y;
  }
}
