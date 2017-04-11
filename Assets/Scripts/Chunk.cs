using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
  private static Vector2 CHUNK_SIZE = new Vector2(21, 21);
  private static int BLOCK_SIZE = 1;

  private Block[,] blocks;
  private Vector2 origin;

  public int lowerX { get { return (int)origin.x - (int)CHUNK_SIZE.x/2; }}
  public int upperX { get { return (int)origin.x + (int)CHUNK_SIZE.x/2; }}
  public int lowerY { get { return (int)origin.y - (int)CHUNK_SIZE.y/2; }}
  public int upperY { get { return (int)origin.y + (int)CHUNK_SIZE.y/2; }}

  private List<Vector3> vertices;
  private List<Vector2> uv;
  private List<int> triangles;
  private Mesh mesh;

	// Use this for initialization
	void Start () {
    mesh = GetComponent<MeshFilter>().mesh;
    origin = Vector2.zero;
    Generate();
	}

  // Update is called once per frame
  void Update () {
    
  }
	
  public void Generate() {
    blocks = new Block[(int)CHUNK_SIZE.x, (int)CHUNK_SIZE.y];

    for (int x = 0; x < CHUNK_SIZE.x; x++) {
      for (int y = 0; y < CHUNK_SIZE.y; y++) {
        blocks[x, y] = new Block(lowerX + x, lowerY + y, Random.Range(1, 3));
      }
    }

    RenderChunk();
  }

  private void RenderChunk() {
    vertices = new List<Vector3>();
    uv = new List<Vector2>();
    triangles = new List<int>();

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

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uv.ToArray();
    mesh.RecalculateNormals();
    GetComponent<Renderer>().material.shader = Shader.Find("Standard");
    GetComponent<Renderer>().material.color = new Color(0.13f, 0.55f, 0.13f);
  }

  private void addFaceVertices(int x, int y, Vector3 face) {
    var block = blocks[x, y];

    if (face == Vector3.back) {
      vertices.Add(new Vector3(x, block.Height, y));
      vertices.Add(new Vector3(x, block.Height, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y));
      updateUV();
    } else if (face == Vector3.forward) {
      vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y));
      vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
      vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y));
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
          vertices.Add(new Vector3(x, block.Height, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y + BLOCK_SIZE));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y + BLOCK_SIZE));
        } else if (face == Vector3.down) {
          vertices.Add(new Vector3(x, block.Height, y));
          vertices.Add(new Vector3(x, block.Height + BLOCK_SIZE, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height + BLOCK_SIZE, y));
          vertices.Add(new Vector3(x + BLOCK_SIZE, block.Height, y));
        }

        updateUV();
      }
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
