using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
  public GameObject cubePrefab;
  public int x;
  public int y;
  public int z;
  public float scale;

  // Use this for initialization
  void Start () {
    var cubeWidth = cubePrefab.transform.lossyScale.x;
    var cubeHeight = cubePrefab.transform.lossyScale.y;
    var cubeDepth = cubePrefab.transform.lossyScale.z;
    var seed = Random.Range(100, 1000);
    var seed2 = Random.Range(100, 1000);

    int waterHeight = 8;

    var noiseMap = new PerlinNoise(seed).GetNoise(x, z, scale);

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
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
