using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ThreatType{ Linear, Quadratic}
public enum ValueType{ Linear, Quadratic}
public enum RiskType{ Awareness, Step}

public class Enemy : MonoBehaviour {

	[Header("Threat Settings")]
	[SerializeField]
	private ThreatType threatType = ThreatType.Linear;
	[SerializeField]
	private RiskType riskType = RiskType.Step;
	[SerializeField]
	private float awareness = 5;
	[SerializeField]
	private float maxRadius = 5;
	[SerializeField]
	private float threatCoefficient = 1;
	[SerializeField]
	private float threatConstant = 0;
	private float currentRadius;

	[Header("Value Settings")]
	[SerializeField]
	private ValueType valueType = ValueType.Linear;
	[SerializeField]
	private float maxValue = 1000;
	[SerializeField]
	private float valueRadius = 15;
	[SerializeField]
	private float valueCoefficient = 1;
	[SerializeField]
	private float valueConstant = 0;
	

	private Animator anim;
	private GameObject player;
	private float awarenessTimestamp;
	private float despawnTimer= 2;
	private Material mat;
	private bool approachPlayer = false;
	private HandheldCamera handheldCam;

	NavMeshAgent agent;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		mat = GetComponentInChildren<Renderer>().material;
		currentRadius = maxRadius;
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		handheldCam = player.GetComponentInChildren<HandheldCamera>();
	}

	void AssessThreats  () 
	{
		float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

		if(riskType == RiskType.Awareness)
		{
			if(distanceFromPlayer <= maxRadius)
			{
				float randomNum = Random.Range( 0.0f, 1.0f );

				if(threatType == ThreatType.Quadratic)
				{
					float chanceToAttack = Mathf.Pow((((maxRadius - distanceFromPlayer) / maxRadius)) * threatCoefficient + threatConstant, 2f);
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();						
						handheldCam.cameraReady = false;
					}
				}
				if(threatType == ThreatType.Linear)
				{
					float chanceToAttack = ((maxRadius - distanceFromPlayer) / maxRadius) * threatCoefficient + threatConstant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
						handheldCam.cameraReady = false;
					}
				}
			}
		}

		if(riskType == RiskType.Step)
		{
			if(distanceFromPlayer <= currentRadius + 1)
			{
				float randomNum = Random.Range( 0.0f, 1.0f );

				if(threatType == ThreatType.Quadratic)
				{
					float chanceToAttack = Mathf.Pow((((maxRadius - currentRadius) / maxRadius)) * threatCoefficient + threatConstant, 2f);
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						approachPlayer = true;
						handheldCam.cameraReady = false;
					}
				}
				if(threatType == ThreatType.Linear)
				{
					float chanceToAttack = ((maxRadius - currentRadius) / maxRadius) * threatCoefficient + threatConstant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						approachPlayer = true;
						handheldCam.cameraReady = false;
					}
				}

				currentRadius -= 1;
			}
		}
	}

	public float CalculateValue()
	{
		float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

		if(valueType == ValueType.Quadratic)
		{
			float pointModifier = Mathf.Pow((((valueRadius - distanceFromPlayer) / valueRadius)) * threatCoefficient + threatConstant, 2f);

			return pointModifier * maxValue;
		}

		if(valueType == ValueType.Linear)
		{
			float pointModifier = ((valueRadius - distanceFromPlayer) / valueRadius) * threatCoefficient + threatConstant;

			return pointModifier * maxValue;
		}

		else return 0;
	}

	void ApproachPlayer()
	{
		agent.SetDestination(player.transform.position);
		if(Vector3.Distance(transform.position, player.transform.position) < 2)
		{
			Debug.Log("near player");
			AttackPlayer();
		}
	}

	void AttackPlayer()
	{
		approachPlayer = false;
		anim.SetTrigger("Attack");
		StartCoroutine(Dissapear(1));
	}

	void RunAway()
	{

	}

	public IEnumerator Dissapear(float time)
	{

		// despawnTimer -= Time.deltaTime;
		// Color tempcolor = mat.color;
		// tempcolor.a = Mathf.MoveTowards(tempcolor.a, 0, Time.deltaTime);
		// mat.color = tempcolor;

		// if(despawnTimer <= 0)
		// {
		// 	Destroy(this.gameObject);
		// }
		while(time > 0)
		{
			Debug.Log(time);
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
			if(time <= 0)
			{
				handheldCam.cameraReady = true;
				Destroy(this.gameObject);
			}
		}
	}

	void Update () 
	{
		if(riskType == RiskType.Awareness && awarenessTimestamp <= Time.time)
		{
			AssessThreats();
			awarenessTimestamp = Time.time + awareness;
		}

		if(riskType == RiskType.Step)
		{
			AssessThreats();
		}

		if(approachPlayer)
		{
			ApproachPlayer();
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