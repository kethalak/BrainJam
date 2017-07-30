using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiResScreenshot : MonoBehaviour {

     public int resWidth = 800; 
     public int resHeight = 600;
	 
	 public Camera camera;

     public GameObject testObj;
     public AudioSource shutterSoundFX;
     public GameObject picturePrefab;

     public int filmAmount = 8;
     private float pictureCooldown = 2;
     private float cooldownTimestamp;

     private bool cameraReady = true;

     private Plane[] planes;

     private bool takeHiResShot = false;
 
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

     public void TakeHiResShot() {
         takeHiResShot = true;
     }

     void Update()
     {
        // Bounds bounds = testObj.GetComponent<Renderer>().bounds;
        // planes = GeometryUtility.CalculateFrustumPlanes(camera);
        // if (GeometryUtility.TestPlanesAABB(planes, bounds))
        //     Debug.Log(testObj.name + " has been detected!");
        // else
        //     Debug.Log("Nothing has been detected");
     }

     void LateUpdate() {
         takeHiResShot |= Input.GetKeyDown("k") && Time.time > cooldownTimestamp && cameraReady;
         if (takeHiResShot) {
             cooldownTimestamp = Time.time + pictureCooldown;

             filmAmount -= 1;
             if(filmAmount <= 0)
             {
                 cameraReady = false;
                 //GAME OVAH BITCHES
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

             GameObject picture = Instantiate(picturePrefab, transform.position, camera.transform.rotation);
             picture.transform.SetParent(transform);
             picture.transform.localPosition = new Vector3(0, 0, -0.005f);
             picture.GetComponent<Renderer>().material.mainTexture = screenShot;
             takeHiResShot = false;
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