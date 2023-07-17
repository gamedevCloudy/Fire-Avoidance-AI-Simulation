using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    private float[] genome;
    //Genome will define direction: +- Horizontal and Vertical

    //health and fitness 
    [SerializeField] private float health = 100f;
    private bool dealDamage = false;
    [SerializeField] private float fitness = 0f;

    void Start()
    {
        genome = new float[2];
        genome[0] = Random.Range(-1f, 1f);
        genome[1] = Random.Range(-1f, 1f);
    }

    void Update()
    {
        //Move within bounds. 
        if (transform.position.x > -49.5f && transform.position.x < 49.5f)
        {
            if (transform.position.z > -49.5f && transform.position.z < 49.5f)
            {
                Vector3 movementDir = new Vector3(genome[0], 0f, genome[1]).normalized;
                transform.position += movementDir * speed * Time.deltaTime;
            }
        }

        if (dealDamage) health -= 5f * Time.deltaTime;

        CalculateFitness();

        if (health <= 0) gameObject.SetActive(false);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "deadzone")
            dealDamage = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "deadzone")
            dealDamage = false;
    }

    private void CalculateFitness()
    {
        fitness = Mathf.Max(0f, health) + Mathf.Max(0f, Time.timeSinceLevelLoad - 10f) * 10f; // Fitness based on remaining health and survival time

        if (!dealDamage)
        {
            fitness += 100f; // Bonus fitness for being inside the blue zone
        }
    }

}
