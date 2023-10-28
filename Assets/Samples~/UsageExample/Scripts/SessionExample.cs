using System.Collections.Generic;
using UnityEngine;
using ReBase;

public class SessionExample : MonoBehaviour
{
    private Session session;
    private int insertedCount = 0;
    private string professionalId = "MrTrotta2010";
    private string patientId = "007";
    private string firstSessionId = "";

    public async void RunSessionExample()
    {
        Application.targetFrameRate = 30;
        insertedCount = 0;
        firstSessionId = "";

        session = new Session(
            title: "Teste de Sess�o",
            description: "Eu sou a primeira Sess�o",
            professionalId: professionalId,
            patientId: patientId
        );

        // Insert Session
        APIResponse response;
        response = await ReBaseClient.Instance.InsertSession(session);
        firstSessionId = response.session?.id;
        Debug.Log($"Inserted: {response}");

        // Insert Movements into Session
        for (insertedCount = 0; insertedCount < 2; insertedCount++)
        {
            Movement movement = new Movement(
                description: "Eu sou um movimento da primeira Sess�o",
                sessionId: firstSessionId,
                label: "NewAPITest",
                fps: Application.targetFrameRate,
                professionalId: professionalId,
                patientId: patientId,
                articulations: new string[] { "1", "2" }
            );

            movement.AddRegister(new Register(
                new Dictionary<string, Rotation>()
                {
                    {  "1", new Rotation(1f, 1f, 1f) },
                    {  "2", new Rotation(2f, 2f, 2f) }
                }
            ));

            response = await ReBaseClient.Instance.InsertMovement(movement);
			Debug.Log($"Inserted: {response}");
		}

        // List Sessions
        response = await ReBaseClient.Instance.FetchSessions(professionalId: professionalId, patientId: patientId);
		Debug.Log($"Downloaded: {response}");

        // Find previously inserted Session
		foreach (SerializableSession serializableSession in response.sessions)
		{
			if (serializableSession.id == firstSessionId)
			{
				session = new Session(serializableSession);
				break;
			}
		}

		// Update Session
		if (session.id == default) response = null;
		else
		{
			session.title = "Atualizando a Sess�o";
			response = await ReBaseClient.Instance.UpdateSession(session);
		}
		Debug.Log($"Updated: {response}");

        // Delete Session
		string id = response.session.id ?? session.id;
        response = await ReBaseClient.Instance.DeleteSession(id);
		Debug.Log($"Deleted: {response}");

        // Insert Session with Movements
		session = new Session(
			title: "Teste de Sess�o 2",
			description: "Todos os movimentos da Sess�o ser�o inseridos de uma vez",
			professionalId: professionalId,
			patientId: patientId,
			movements: new Movement[2]
			{
				new Movement(
					label: "firstMovement",
					fps: Application.targetFrameRate,
					articulations: new string[] { "1", "2" },
					registers: new Register[1] {
						new Register(
							new Dictionary<string, Rotation>()
							{
								{ "1", new Rotation(1f, 1f, 1f) },
								{ "2", new Rotation(2f, 2f, 2f) }
							}
						)
					}
				),
				new Movement(
					label: "secondMovement",
					fps: Application.targetFrameRate,
					articulations: new string[] { "1", "2" },
					registers: new Register[1] {
						new Register(
							new Dictionary<string, Rotation>()
							{
								{ "1", new Rotation(1f, 1f, 1f) },
								{ "2", new Rotation(2f, 2f, 2f) }
							}
						)
					}
				)
			}
		);

		response = await ReBaseClient.Instance.InsertSession(session);

		if (response.session == null) Debug.Log("Couldn't insert Session");
		else
		{
			Debug.Log($"Inserted: {response}");

			// Find Session
			response = await ReBaseClient.Instance.FindSession(response.session.id);
			Debug.Log($"Found: {response}");

			// Delete Session
			response = await ReBaseClient.Instance.DeleteSession(response.session.id);
			Debug.Log($"Deleted: {response}");
		}
	}
}
