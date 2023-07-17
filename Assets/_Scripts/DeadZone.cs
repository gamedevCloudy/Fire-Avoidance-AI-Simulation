using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private int speed;
    // Update is called once per frame

    [SerializeField] private GameManager gm;
    void Update()
    {
        if (transform.position.x < -5) //max X position - leaves some space for agents
        {
            Vector3 target = new Vector3(1, 0, 0);
            transform.position += target * speed * Time.deltaTime;
        }
        else
        {
            //Generate new generation
            Reset();
        }
    }

    void Reset()
    {
        gm.SelectFittestAgents();
        transform.position = new Vector3(-90f, -0.9f, 0f);
        gm.ReproduceAndMutate();
    }
}
