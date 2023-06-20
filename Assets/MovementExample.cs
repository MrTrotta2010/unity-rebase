using System.Collections.Generic;
using UnityEngine;
using ReBase;
using System;

public class MovementExample : MonoBehaviour
{
	private Movement movement;

	public async void RunMovementExample()
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

		// Insert
		APIResponse response = await RESTClient.Instance.InsertMovement(movement);
		Debug.Log($"Inserted: {response}");

		// Update
		movement.description = "Vamos atualizar pra ver o que acontece";
		response = await RESTClient.Instance.UpdateMovement(response.movement.id, movement);
		Debug.Log($"Updated: {response}");

		// Delete
		response = await RESTClient.Instance.DeleteMovement(response.movement.id);
		Debug.Log($"Deleted! {response}");

		// Find
		response = await RESTClient.Instance.FindMovement(response.deletedId);
		Debug.Log($"Found? {response}");

		// List
		response = await RESTClient.Instance.FetchMovements(professionalId: movement.professionalId, patientId: movement.patientId);
		Debug.Log($"Downloaded: {response}");

		// Convert SerializableMovements into Movements
		if (response.movements != null)
		{
			foreach (SerializableMovement serializableMovement in response.movements)
			{
				try
				{
					Movement downloadedMovement = new Movement(serializableMovement);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
					Debug.LogError(serializableMovement);
				}
			}
		}
	}
}
