using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class HandheldCamera : MonoBehaviour {


     [SerializeField]
     private int resWidth = 800;
     [SerializeField] 
     private int resHeight = 600;
     [SerializeField]
	 private Camera camera;
     [SerializeField]
     private GameObject testObj;
     [SerializeField]
     private AudioSource shutterSoundFX;
     [SerializeField]
     private GameObject picturePrefab;
     [SerializeField]
     private float pictureCooldown = 2;
     [SerializeField]
     private float calculatePointsRadius = 2;

     private float cooldownTimestamp;
     private Plane[] planes;
     private GameObject picture;
     private Animator anim;
     private bool gameover = false;
     
     public int pointSum;
     
     public List<GameObject> takenPictures = new List<GameObject>();

     [SerializeField] private int filmAmount = 8;
     private bool cameraReady = true;
     public bool beingAttacked = false;

    public Action<bool> OnFilmChanged;

    public int FilmAmount {
        get { return filmAmount; }
        set {
            filmAmount = value;
            if(OnFilmChanged != null)
                OnFilmChanged(beingAttacked);
        }
    }

    public static string ScreenShotName(int width, int height) 
     {
         return string.Format("screen_{0}x{1}_{2}.png", 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }
     
     void Awake()
     {
         anim = GetComponentInChildren<Animator>();
     }
     void Start()
     {
        cooldownTimestamp = Time.time;
     }

     void Update()
     {
        if(FilmAmount <= 0 && !gameover)
        {
            FilmAmount = 0;
            cameraReady = false;
            gameover = true;
            SceneManager.LoadScene("ScoreScreen");
        }

        if(Time.time > cooldownTimestamp)
        {
            cameraReady = true;
        }
     }

     void LateUpdate() {
         if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetButtonDown("Fire1"))
         {
            if(cameraReady && !beingAttacked && !gameover)
            {
                cameraReady = false;
                TakePicture();
                cooldownTimestamp = Time.time + pictureCooldown;
            }
         }
     }

     void TakePicture()
     {
             anim.SetTrigger("PressButton");
             FilmAmount -= 1;
             if(FilmAmount <= 0)
             {
                 cameraReady = false;
                 //rah rah rah Game OVAH
             }

             shutterSoundFX.Play();
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
            //  byte[] bytes = screenShot.EncodeToPNG();
            //  string filename = ScreenShotName(resWidth, resHeight);
            //  System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/" + filename, bytes);
            //  Debug.Log(string.Format("Took screenshot to: {0}", Application.dataPath + "/Resources/" + filename));
             screenShot.Apply();

             picture = Instantiate(picturePrefab, transform.position, camera.transform.rotation);
             picture.transform.SetParent(transform);
             picture.transform.localPosition = new Vector3(0, 0, -0.005f);
             picture.GetComponent<Renderer>().material.mainTexture = screenShot;
             takenPictures.Add(picture);

             CalculatePoints();
     }
     
     void CalculatePoints()
     {
        int layerMask = 1 << LayerMask.NameToLayer("Creature");
        RaycastHit[] hits;
        Vector3 p1 = transform.position;

        hits = Physics.SphereCastAll(p1, calculatePointsRadius, transform.forward, 1, layerMask);

        foreach(RaycastHit hit in hits)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            Bounds bounds = hit.transform.GetComponentInChildren<Renderer>().bounds;
            planes = GeometryUtility.CalculateFrustumPlanes(camera);

            if (GeometryUtility.TestPlanesAABB(planes, bounds))
            {   
                // RaycastHit[] directHits = Physics.RaycastAll(transform.position, hit.transform.GetComponent<Collider>().bounds.center - transform.position, Mathf.Infinity, LayerMask.NameToLayer("Player"));
                // Debug.DrawRay(transform.position, hit.transform.GetComponent<Collider>().bounds.center - transform.position, Color.red, 4);
                // foreach(RaycastHit directHit in directHits)
                // {
                //     Debug.Log(directHit.transform.name);
                //     if(directHit.transform == hit.transform)
                //     {
                //         Debug.Log("You took a picture of " + hit.transform.name + "!");
                //         TextMeshPro tmp = picture.GetComponentInChildren<TextMeshPro>();
                //         float points = Mathf.Ceil(enemy.CalculateValue());

                //         if( points > 0 )
                //             tmp.text = points.ToString();
                //     }
                // }

                float points = Mathf.Ceil(enemy.CalculateValue());

                if( points > 0 )
                {
                    pointSum += (int)points;
                    TextMeshPro tmp = picture.GetComponentInChildren<TextMeshPro>();
                    tmp.text = points.ToString();
                    Debug.Log("You took a picture of " + hit.transform.name + "!");
                    StartCoroutine(enemy.Dissapear(0));
                }

            }
        }
     }
}

