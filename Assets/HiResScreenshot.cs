using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiResScreenshot : MonoBehaviour {

     public int resWidth = 800; 
     public int resHeight = 600;
	 
	 public Camera camera;

     public GameObject testObj;

     private Plane[] planes;

     private bool takeHiResShot = false;
 
     public static string ScreenShotName(int width, int height) {
         return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
                              Application.dataPath, 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }
 
     public void TakeHiResShot() {
         takeHiResShot = true;
     }

     void Update()
     {
        Bounds bounds = testObj.GetComponent<Renderer>().bounds;
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (GeometryUtility.TestPlanesAABB(planes, bounds))
            Debug.Log(testObj.name + " has been detected!");
        else
            Debug.Log("Nothing has been detected");
     }

     void LateUpdate() {
         takeHiResShot |= Input.GetKeyDown("k");
         if (takeHiResShot) {
             RenderTexture original = camera.targetTexture;
             RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
             camera.targetTexture = rt;
             Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
             camera.Render();
             RenderTexture.active = rt;
             screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
             camera.targetTexture = original;
             RenderTexture.active = null; // JC: added to avoid errors
             Destroy(rt);
             byte[] bytes = screenShot.EncodeToPNG();
             string filename = ScreenShotName(resWidth, resHeight);
             System.IO.File.WriteAllBytes(filename, bytes);
             Debug.Log(string.Format("Took screenshot to: {0}", filename));
             takeHiResShot = false;
         }
     }
}
