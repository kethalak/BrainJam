using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialController : MonoBehaviour {

    [SerializeField] private Transform creatureSpawn;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform player;
    [SerializeField] private HandheldCamera handHeld;
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private OVRScreenFade screenFade;

    private GameObject currentCreature;
    
    void Awake() {
        currentCreature = GameObject.Instantiate(creaturePrefab, creatureSpawn.position, creatureSpawn.rotation);
        handHeld.OnFilmChanged += OnFilmAmountChanged;
    }

    void OnFilmAmountChanged() {
        StartCoroutine(ResetTrial());
    }

    private IEnumerator ResetTrial() {
        yield return new WaitForSeconds(1);
        StartCoroutine(screenFade.FadeOut());
        yield return new WaitForSeconds(.5f);
        if (currentCreature != null) {
            Destroy(currentCreature);
        }

        currentCreature = Instantiate(creaturePrefab, creatureSpawn.position, creatureSpawn.rotation);
        player.position = playerSpawn.position;
        yield return new WaitForSeconds(.1f);
        StartCoroutine(screenFade.FadeIn());
    }

}
