using System.Collections.Generic;
using UnityEngine;
using ReBase;
using System;

public class MovementExample : MonoBehaviour
{
	private ReBaseClient client = new ReBaseClient("seu@email.com", "seuToken");
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
		APIResponse response = await client.InsertMovement(movement);
		Debug.Log($"Inserted: {response}");

		// Update
		movement.description = "Vamos atualizar pra ver o que acontece";
		response = await client.UpdateMovement(response.movement.id, movement);
		Debug.Log($"Updated: {response}");

		// Delete
		response = await client.DeleteMovement(response.movement.id);
		Debug.Log($"Deleted! {response}");

		// Find
		response = await client.FindMovement(response.deletedId);
		Debug.Log($"Found? {response}");

		// List
		response = await client.FetchMovements(professionalId: movement.professionalId, patientId: movement.patientId, page: 1, per: 3);
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
