using UnityEngine;
using System.Collections;

//[MenuItem("GameObject/Create Other/Custom Plane...")]
public class MiscLevelDetails : MonoBehaviour {
	
	public GameObject miniMapTerrain;
	public GameObject mainTerrain;
	
	// Use this for initialization
	void Start () {
		Terrain terrain = miniMapTerrain.GetComponent<Terrain>();
		terrain.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		/*
    float[,,] element = new float[1,1,2]; // create a temp 1x1x2 array
    splatmapData[y, x, 0] = element[0, 0, 0] = 0; // set the element and
    splatmapData[y, x, 1] = element[0, 0, 1] = 1; // update splatmapData
    terrain.terrainData.SetAlphamaps(y, x, element); // update only the selected terrain element
    	*/
		
		
		
		// Take a screen shot		
		/*
		public class example : MonoBehaviour {
    	void OnMouseDown() {
        Application.CaptureScreenshot("Screenshot.png");
    	}
		}
		*/
	
	}
}
