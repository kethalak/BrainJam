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
	[SerializeField]
	private float attackRange = 2.5f;
	[SerializeField]
	private float stepsPerCheck = 2;
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
	
	[Header("References")]
	[SerializeField]
	private GameObject warningMarker;
	[SerializeField]
	private Color markerStartColor = Color.yellow;
	[SerializeField]
	private Color markerEndColor = Color.red;
	
	public Animator anim;
	private GameObject player;
	private GameObject playerCam;
	private float awarenessTimestamp;
	private float despawnTimer= 2;
	private Material mat;
	private bool approachPlayer = false;
	private HandheldCamera handheldCam;
	private float distanceFromPlayer;

	NavMeshAgent agent;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		mat = GetComponentInChildren<Renderer>().material;
		currentRadius = maxRadius;
		if(anim == null)
			anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerCam = Camera.main.gameObject;
		handheldCam = player.GetComponentInChildren<HandheldCamera>();
    
	}

	void AssessThreats  () 
	{
		if(riskType == RiskType.Awareness)
		{
			if(distanceFromPlayer <= maxRadius && !handheldCam.beingAttacked)
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
						handheldCam.beingAttacked = true;
					}
				}
				if(threatType == ThreatType.Linear && !handheldCam.beingAttacked)
				{
					float chanceToAttack = ((maxRadius - distanceFromPlayer) / maxRadius) * threatCoefficient + threatConstant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						ApproachPlayer();
						handheldCam.beingAttacked = true;
					}
				}
			}
		}

		if(riskType == RiskType.Step)
		{
			if(distanceFromPlayer <= currentRadius + 1 && !handheldCam.beingAttacked)
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
						anim.SetTrigger("Move");
						handheldCam.beingAttacked = true;
					}
				}
				if(threatType == ThreatType.Linear && !handheldCam.beingAttacked)
				{
					float chanceToAttack = ((maxRadius - currentRadius) / maxRadius) * threatCoefficient + threatConstant;
					Debug.Log(string.Format("{0} attempted to attack with a {1}% chance to hit and rolled a {2}", transform.name, chanceToAttack * 100, randomNum * 100));

					if(randomNum <= chanceToAttack)
					{
						Debug.Log(string.Format("{0} hit!", transform.name));
						approachPlayer = true;
						anim.SetTrigger("Move");
						handheldCam.beingAttacked = true;
					}
				}

				currentRadius -= stepsPerCheck;
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
		warningMarker.SetActive(false);
		
		agent.SetDestination(playerCam.transform.position);
		if(Vector3.Distance(transform.position, player.transform.position) < attackRange)
		{
			Debug.Log("near player");
			AttackPlayer();
		}
	}

	void AttackPlayer()
	{
        agent.SetDestination(transform.position);
        agent.isStopped = true;
		approachPlayer = false;
		anim.SetTrigger("Attack");
		StartCoroutine(Dissapear(1));
		StartCoroutine(DestroyFilm(1));
	}

	void RunAway()
	{

	}

	public IEnumerator DestroyFilm(float time)
	{
		while(time >= 0)
		{
			time -= Time.deltaTime;
			if (time <= 0) {
				handheldCam.FilmAmount -= 1;
				yield return new WaitForEndOfFrame();
				handheldCam.beingAttacked = false;
			}

			yield return new WaitForEndOfFrame();
		}		
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
		while(time >= 0)
		{
			time -= Time.deltaTime;
			if(time <= 0)
			{
				handheldCam.beingAttacked = false;
				Destroy(this.gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	void Update () 
	{
		distanceFromPlayer = Vector3.Distance(transform.position, playerCam.transform.position);
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

		if(distanceFromPlayer <= maxRadius && !handheldCam.beingAttacked)
		{
			Material mat = warningMarker.GetComponentInChildren<Renderer>().material;
			mat.SetColor ("_EmissionColor", Color.Lerp(markerEndColor, markerStartColor, distanceFromPlayer / maxRadius));

			warningMarker.SetActive(true);
			warningMarker.transform.LookAt(playerCam.transform);

			Vector3 targetDir = playerCam.transform.position - transform.position;
			var rotation = Quaternion.LookRotation(targetDir);
   			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
		}
		else
		{
			warningMarker.SetActive(false);
		}
	}
}

