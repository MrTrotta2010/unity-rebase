using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReBase;
using System;

public class MovementExample : MonoBehaviour
{
    private Movement movement;

    public void RunMovementExample()
    {
        Application.targetFrameRate = 30;

        movement = new Movement(
            label: "NewAPITest",
            fps: Application.targetFrameRate,
            professionalId: "MrTrotta2010",
            articulations: new string[] { "1", "2" }
        );

        movement.AddRegister(new Register(
            new Dictionary<string, Rotation>()
			{
                { "1", new Rotation(1f, 1f, 1f) },
                { "2", new Rotation(2f, 2f, 2f) }
			}
        ));

        StartCoroutine(RESTClient.Instance.InsertMovement(OnInserted, movement));
    }

    public void OnDownloadFinished(APIResponse response)
	{
        Debug.Log($"Downloaded: {response}");

        if (response.movements != null)
        {
            foreach (SerializableMovement serializableMovement in response.movements)
            {
                try {
                    Movement downloadedMovement = new Movement(serializableMovement);
				}
                catch(Exception e)
				{
                    Debug.LogError(e);
                    Debug.LogError(serializableMovement);
				}
            }
        }
    }

    public void OnInserted(APIResponse response)
	{
        Debug.Log($"Inserted: {response}");

        movement.description = "Vamos atualizar pra ver o que acontece";
        StartCoroutine(RESTClient.Instance.UpdateMovement(OnUpdated, response.movement.id, movement));
	}

    public void OnUpdated(APIResponse response)
	{
        Debug.Log($"Updated: {response}");
        StartCoroutine(RESTClient.Instance.DeleteMovement(OnDeleted, response.movement.id));
	}

    public void OnDeleted(APIResponse response)
	{
        Debug.Log($"Deleted! {response}");
        StartCoroutine(RESTClient.Instance.FindMovement(OnFound, response.deletedId));
	}

    public void OnFound(APIResponse response)
	{
        Debug.Log($"Found? {response}");
        StartCoroutine(RESTClient.Instance.FetchMovements(OnDownloadFinished, professionalId: movement.professionalId, patientId: movement.patientId));
    }
}
