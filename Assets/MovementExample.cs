using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReBase;

public class MovementExample : MonoBehaviour
{
    private Movement movement;

    void Awake()
    {
        Application.targetFrameRate = 30;

        movement = new Movement(
            title: "Eu sou um outro Movimento!",
            label: "NewAPITest",
            fps: Application.targetFrameRate,
            professionalId: "MrTrotta2010",
            patientId: "007",
            articulations: new int[] { 1, 2 }
        );

        movement.AddRegister(new Register(
            new Dictionary<int, Vector3>()
			{
                {  1, new Vector3(1f, 1f, 1f) },
                {  2, new Vector3(2f, 2f, 2f) }
			}
        ));
    }

	void Start()
	{
        StartCoroutine(RESTClient.Instance.InsertMovement(OnInserted, movement));
    }

    public void OnDownloadFinished(APIResponse response)
	{
        Debug.Log($"Downloaded: {response}");

        if (response.movements != null)
        {
            foreach (SerializableMovement serializableMovement in response.movements)
            {
                Movement downloadedMovement = new Movement(serializableMovement);
                Debug.Log($"Movement: {downloadedMovement.ToJson()}");
            }
        }
    }

    public void OnInserted(APIResponse response)
	{
        Debug.Log($"Inserted: {response}");

        movement.description = "Vamos atualizar pra ver o que acontece";
        StartCoroutine(RESTClient.Instance.UpdateMovement(OnUpdated, response.created.id, movement));
	}

    public void OnUpdated(APIResponse response)
	{
        Debug.Log($"Updated: {response}");
        StartCoroutine(RESTClient.Instance.DeleteMovement(OnDeleted, response.updated.id));
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
