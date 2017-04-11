using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
  public GameObject cubePrefab;
  public Chunk chunkPrefab;

  public int xChunks;
  public int zChunks;
  public int maxHeight;
  public int waterHeight;
  public float scale;
  public HeightMap HeightMap { get; private set; }

  // Use this for initialization
  void Start () {
    var cubeWidth = cubePrefab.transform.lossyScale.x;
    var cubeHeight = cubePrefab.transform.lossyScale.y;
    var cubeDepth = cubePrefab.transform.lossyScale.z;
    var seed = Random.Range(100, 1000);
    var seed2 = Random.Range(100, 1000);

    var perlinNoise = new PerlinNoise(seed, scale);
    HeightMap = new HeightMap(perlinNoise, maxHeight);

    for (int i = -xChunks/2; i < xChunks/2; i++) {
      for (int j = -zChunks/2; j < zChunks/2; j++) {
        var chunk = Chunk.Instantiate(this, i, j) as Chunk;
      }
    }

    /*var noiseMap = perlinNoise.GetNoiseMap(0, x, 0, z);
		for (int i = 0; i < x; i++) {
      for (int k = 0; k < z; k++) {
        var temperature = Mathf.PerlinNoise((i+x+seed)/80f, (k+z+seed)/80f) * 10000;
        var height = Mathf.RoundToInt(noiseMap[i, k] * y);

        if (height <= waterHeight) {
          var cube = Instantiate(cubePrefab, new Vector3((i - x/2) * cubeWidth, waterHeight * cubeHeight, (k - z/2) * cubeDepth), Quaternion.identity, transform) as GameObject;
            cube.GetComponent<Renderer>().material.color = Color.blue;
        } else {
          var cube = Instantiate(cubePrefab, new Vector3((i - x/2) * cubeWidth, height * cubeHeight, (k - z/2) * cubeDepth), Quaternion.identity, transform) as GameObject;
          if (height <= waterHeight + 1) {
            cube.GetComponent<Renderer>().material.color = Color.yellow;
          } else {
            cube.GetComponent<Renderer>().material.color = new Color(0.13f, 0.55f, 0.13f);
          }
        }
      }
    }*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
