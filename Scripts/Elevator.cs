using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform elevatorModel;
    public GameObject collObject;

    public float elSpeed;

    private bool shouldOpen;
    private float buffer = 2f;
    private float elevate = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldOpen && elevatorModel.position.z != 1f)
        {
            elevatorModel.position = Vector3.MoveTowards(elevatorModel.position, new Vector3(elevatorModel.position.x, elevatorModel.position.y, 1f), elSpeed * Time.deltaTime);

            if (elevatorModel.position.z == 1f)
            {
                collObject.transform.position = Vector3.MoveTowards(elevatorModel.position, new Vector3(elevatorModel.position.x, elevatorModel.position.y, 1f), elSpeed * Time.deltaTime);
            }
        }
        else if (!shouldOpen && elevatorModel.position.z != 0f)
        {
            elevatorModel.position = Vector3.MoveTowards(elevatorModel.position, new Vector3(elevatorModel.position.x, elevatorModel.position.y, 0f), elSpeed * Time.deltaTime);

            if (elevatorModel.position.z == 0f)
            {
                collObject.transform.position = Vector3.MoveTowards(elevatorModel.position, new Vector3(elevatorModel.position.x, elevatorModel.position.y, 0f), elSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            shouldOpen = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time > elevate)
        {
            if (other.tag == "Player")
            {
                shouldOpen = false;
                elevate = Time.time + buffer;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            shouldOpen = false;
        }
    }

}
