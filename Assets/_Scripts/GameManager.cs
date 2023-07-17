using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Agent Props")]
    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private Transform agentContainer;
    [SerializeField] private int initalPopulaiton = 1000;

    [Header("Simulation Props")]
    private int generationCount = 1;

    [SerializeField] private int agentsToSelect = 100;
    [SerializeField] private List<AgentController> agents;

    [Header("Simulation Speed")]
    [Tooltip("Adjust time scale to speed up simulation.")]
    [Range(1f, 100f)]
    [SerializeField] private float timeScale = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 30;
        GenerateInitalPopulation(initalPopulaiton);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
    }

    void GenerateInitalPopulation(int initalSize)
    {
        for (int i = 0; i < initalSize; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-48f, 48f), 1.5f, Random.Range(-48f, 48f));

            GameObject ag = Instantiate(agentPrefab, spawnPos, Quaternion.identity);

            ag.transform.SetParent(agentContainer);

            agents.Add(ag.GetComponent<AgentController>());
        }
    }
}
