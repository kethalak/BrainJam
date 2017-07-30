using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThreatType{ Linear, Quadratic}
public enum ValueType{ Linear, Quadratic}
public enum RiskType{ Awareness, Step}

public class Enemy : MonoBehaviour {

	[Header("Threat and Value Settings")]
	[SerializeField]
	private ThreatType threatType = ThreatType.Linear;

	public ValueType valueType = ValueType.Linear;

	[SerializeField]
	private RiskType riskType = RiskType.Step;

	[SerializeField]
	private float awareness = 5;
	[SerializeField]
	private float maxValue = 1000;
	[SerializeField]
	private float maxRadius = 5;

	private float currentRadius;

	[Header("Additional Threat Settings")]
	[SerializeField]
	private float coefficient = 1;
	[SerializeField]
	private float constant = 0;

	private Animator anim;

	private GameObject player;

	private float awarenessTimestamp;

	void Awake()
	{
		currentRadius = maxRadius;
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
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
					float chanceToAttack = Mathf.Pow((((maxRadius - distanceFromPlayer) / maxRadius)) * coefficient + constant, 2f);
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
					}
				}
				if(threatType == ThreatType.Linear)
				{
					float chanceToAttack = ((maxRadius - distanceFromPlayer) / maxRadius) * coefficient + constant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
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
					float chanceToAttack = Mathf.Pow((((maxRadius - currentRadius) / maxRadius)) * coefficient + constant, 2f);
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
					}
				}
				if(threatType == ThreatType.Linear)
				{
					float chanceToAttack = ((maxRadius - currentRadius) / maxRadius) * coefficient + constant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
					}
				}

				currentRadius -= 1;
			}
		}
	}

	void ApproachPlayer()
	{
		anim.SetTrigger("Attack");
	}

	void AttackPlayer()
	{

	}

	void RunAway()
	{

	}

	void Dissapear()
	{

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