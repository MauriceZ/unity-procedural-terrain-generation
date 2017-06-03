using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
  public static Tree Instantiate(Chunk chunk, Block block) {
    var tree = Instantiate (chunk.treePrefab, chunk.transform) as Tree;
    tree.transform.position = new Vector3(block.x, block.Height + Chunk.BLOCK_SIZE, block.y);
    tree.height = Random.Range(4, 12);
    tree.Generate();
    return tree;
  }

  public GameObject treeTop;
  private int height;

  private void Generate() {
    var vertices = new List<Vector3>();
    var triangles = new List<int>();
    var uv = new List<Vector2>();

    // front
    vertices.Add(Vector3.zero);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.up * Chunk.BLOCK_SIZE * height);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.right * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.down * Chunk.BLOCK_SIZE * height);
    // vertices.Add(vertices[vertices.Count - 1] + Vector3.left * Chunk.BLOCK_SIZE);
    addTriangles(vertices, triangles, uv);

    // right
    vertices.Add(Vector3.right * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.up * Chunk.BLOCK_SIZE * height);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.forward * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.down * Chunk.BLOCK_SIZE * height);
    addTriangles(vertices, triangles, uv);

    // back
    vertices.Add(Vector3.right * Chunk.BLOCK_SIZE + Vector3.forward * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.up * Chunk.BLOCK_SIZE * height);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.left * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.down * Chunk.BLOCK_SIZE * height);
    addTriangles(vertices, triangles, uv);

    // left
    vertices.Add(Vector3.zero);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.forward * Chunk.BLOCK_SIZE);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.up * Chunk.BLOCK_SIZE * height);
    vertices.Add(vertices[vertices.Count - 1] + Vector3.back * Chunk.BLOCK_SIZE);
    addTriangles(vertices, triangles, uv);

    var mesh = GetComponent<MeshFilter>().mesh;
    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uv.ToArray();
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    GetComponent<MeshCollider>().sharedMesh = mesh;
    Instantiate(treeTop, transform.position + new Vector3(1f, height, 0.7f), Quaternion.identity, transform);
  }

  private void addTriangles(List<Vector3> vertices, List<int> triangles, List<Vector2> uv) {
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
}
