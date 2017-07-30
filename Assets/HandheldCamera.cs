using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandheldCamera : MonoBehaviour {

     [SerializeField]
     private int filmAmount = 8;
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

     public bool cameraReady = true;
 
     public static string ScreenShotName(int width, int height) 
     {
         return string.Format("screen_{0}x{1}_{2}.png", 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }
 
     void Start()
     {
        cooldownTimestamp = Time.time;
     }

     void Update()
     {

     }

     void LateUpdate() {
         if(Input.GetButton("Fire1") && Time.time > cooldownTimestamp && cameraReady)
         {
            TakePicture();
            cooldownTimestamp = Time.time + pictureCooldown;
         }
     }

     void TakePicture()
     {
             filmAmount -= 1;
             if(filmAmount <= 0)
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
                    TextMeshPro tmp = picture.GetComponentInChildren<TextMeshPro>();
                    tmp.text = points.ToString();
                    Debug.Log("You took a picture of " + hit.transform.name + "!");
                    enemy.Dissapear(0);
                }

            }
        }
     }
}


















    //                                   Jizzpoobu                                         
    //                               ggerfuckstickbugg                                     
    //                           ershitshitbloody.Arsetur                                  
    //                   djizzmopperpooj           izzmoppe                                
    //                rwankertwotba                  stardsh                               
    //              itfuck.Pissassh                   olebol                               
    //              locksbloodyballb                   agwee                               
    //              cumbubbletw ottwat    hole.Cockfu  ckthu                               
    //              ndercuntbuggertwatt urdfuckfuckerfu ckdi                               
    //              ck.Jizzmoppertwat  holebollixfrigmingewe                               
    //             ebast  ardshit.Jiz  zmopperbuggerbollocks                               
    //            thundercuntpissminge asshole.Wan kerpoowee                               
    //           fuckpoowankerpissmi   ngeshitcumbubbleblood                               
    //          y.Wankerjizzmoppertwotbuggercuntassh  olefu                                
    //         ckwan          ker.Wankertwotmoth     erfuck                                
    //        ertwo                      tfuckst     ickcum                                
    //       bubble                                 bloody                                 
    //      bastar                                 d.Cunt                                  
    //     mingeb                                  ollock                                  
    //     sfuck                      boll        ocksfu                                   
    //     ckst                      ick.J izz   poobug                                    
    //     gerf                      uckstickbu  ggers                         hitshitbl   
    //    oody.                      Arseturdj  izzmo                        pperpoojizzm  
    //    opper                     wankertwot basta                       rdshit    fuck  
    //    .Piss                     assholebo  llock                     sbloody    ballb  
    //    agwee                    cumbubble  twottw                   athole.     Cockf   
    //    uckth                    undercun   tbuggertwatturdfuckf   uckerfu     ckdic     
    //     k.Ji                   zzmopper    twatholebollixfrigmingeweeba      stard      
    //     shit                   .Jizzmo     pperb   ugger   bollocksth      underc       
    //     untp                  issminge      ass   hole.Wankerpoowee      fuckpo         
    //     owank               erpis sming         eshitcumbubblebloo     dy.Wank          
    //      erji             zzmop  pertwot         buggercuntassholefu   ckwanker         
    //      .Wank            ertwotmotherfu                     ckertwot    fuckstick      
    //       cumbu            bblebloodyba              star       d.Cunt  ming eboll      
    //       ocksfu              ckbo                   lloc        ksfuck  stick.Ji       
    //        zzpoob                                ugg              erfuc    ksti         
    //         ckbugger                            shit              shitb     lood        
    //            y.Arset                          urdj              izzmopperpooji        
    //  zzm        opperwanke                       rtwo           tbastardshitfuc         
    // k.Pissa    ssholebollocksblo                  ody         ballbag    w              
    // eecumbubbletwo ttwathole.Cockfuckthu           nder    cuntbug                      
    // gert watturdfuckfuc    kerfuckdick.Jiz zmoppertwatholebollix                        
    //  frig  mingeweeba         stardshit.J izzmopperbuggerboll                           
    //   ocks   thunde         rcuntpissmin geass hole.Wanker                              
    //    pooweefuck           poowankerpi  ssmi                                           
    //     ngeshit              cumbubbl   eblo                                            
    //       ody                .Wanke    rjiz                                             
    //                           zmoppe  rtwo                                              
    //                            tbuggercun                                               
    //                              tasshol                                                
    //                                efu 