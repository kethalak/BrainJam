using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TrialController : MonoBehaviour {

    [SerializeField] private Transform creatureSpawn;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform player;
    [SerializeField] private HandheldCamera handHeld;
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private OVRScreenFade screenFade;
    
    private GameObject currentCreature;

    private const string logPath = @"Assets\Shima\Trial Logs";
    private readonly StringBuilder trialLog = new StringBuilder();
    private float timeElapsed;
    private int phaseCount = 0;
    private List<PhaseData> phaseData = new List<PhaseData>();
    private bool logWritten = false;
    void Update() {
        timeElapsed += Time.deltaTime;
    }
    void Awake() {
        handHeld.OnFilmChanged += OnFilmAmountChanged;
    }

    void Start() {
        trialLog.AppendLine("Phase,Success,Distance,Time");
        StartCoroutine(ResetTrial());
    }
    
    void OnFilmAmountChanged(bool attacked) {
        
        trialLog.AppendLine(String.Format("{0},{1},{2:F2},{3:F2}", phaseCount, attacked ? "0" : "1", Vector3.Distance(handHeld.transform.position, creatureSpawn.transform.position), timeElapsed));
            
        if (handHeld.FilmAmount > 0) {
            StartCoroutine(ResetTrial());
        }
        else if (handHeld.FilmAmount == 0 && !logWritten)
            WriteToFile(trialLog, logPath);
    }

    private IEnumerator ResetTrial() {
        phaseCount += 1;
        timeElapsed = 0;
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

    private void WriteToFile(StringBuilder sb, string path) {
        path += "/" + System.DateTime.Now.ToString("yyyy-MM-dd -- HH-mm-ss");
        File.Create(path+".csv").Dispose();
        File.WriteAllText(path+".csv", sb.ToString());
        logWritten = true;
    }

    private struct PhaseData {
        private int phase;
        private bool success;
        private float distance;
        private float time;

        public PhaseData(int phase, bool success, float distance, float time) : this() {
            this.phase = phase;
            this.success = success;
            this.distance = distance;
            this.time = time;
        }
    }
}
