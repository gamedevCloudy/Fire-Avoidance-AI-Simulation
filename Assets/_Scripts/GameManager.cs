using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Agent Props")]
    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private Transform agentContainer;
    [SerializeField] private int initalPopulaiton = 1000;

    [Header("Simulation Props")]
    private int generationCount = 1;

    [SerializeField] private int selectedAgentsCount = 100;
    [SerializeField] private List<AgentController> agents;

    [Header("Simulation Speed")]
    [Tooltip("Adjust time scale to speed up simulation.")]
    [Range(1f, 100f)]
    [SerializeField] private float timeScale = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text genText;
    [SerializeField] private TMP_Text genomeText;

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

    public void SelectFittestAgents()
    {
        // Sort the agents based on their fitness scores in descending order
        agents.Sort((a, b) => b.fitness.CompareTo(a.fitness));

        // Select the top agents with the highest fitness scores
        for (int i = selectedAgentsCount; i < agents.Count; i++)
        {
            agents[i].gameObject.SetActive(false); // Disable or destroy the least fit agents
            // agents.Remove(agents[i]);
        }

        // ReproduceAndMutate();

        generationCount += 1;
    }

    public void ReproduceAndMutate()
    {
        // Create new offspring from the selected fittest agents
        // for (int i = selectedAgentsCount; i < agents.Count; i++)
        for (int i = 0; i < selectedAgentsCount; i++)
        {
            AgentController parentA = agents[Random.Range(0, selectedAgentsCount)];
            AgentController parentB = agents[Random.Range(0, selectedAgentsCount)];

            // Create a new agent object as the offspring
            GameObject offspring = Instantiate(agentPrefab);
            offspring.transform.position = new Vector3(Random.Range(-48f, 48f), 1.5f, Random.Range(-48f, 48f));
            AgentController offspringController = offspring.GetComponent<AgentController>();

            //Add them to list.
            agents.Add(offspringController);

            // Combine the genomes of the parents and assign it to the offspring


            //Average out the genome of the parents. 
            for (int j = 0; j < offspringController.genome.Length; j++)
            {
                offspringController.genome[j] = (parentA.genome[j] + parentB.genome[j]) / 2;
            }

            // Apply mutation to the offspring's genome to introduce variation

            // Here, we'll simply mutate a single gene by adding a small random value
            int geneIndex = Random.Range(0, offspringController.genome.Length - 1);
            offspringController.genome[geneIndex] += Random.Range(-0.1f, 0.1f);
        }
        //Remove some parents from the genome
        for (int i = 0; i < (agents.Count - selectedAgentsCount); i++)
        {
            agents[i].gameObject.SetActive(false);
            agents.Remove(agents[i]);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        genText.text = "GEN: " + generationCount;
        genomeText.text = "best genome: " + "\n" + agents[0].genome[0] + "\n" + agents[0].genome[1];
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
