using System.Collections;
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

    public void RunSessionExample()
    {
        Application.targetFrameRate = 30;
        insertedCount = 0;
        firstSessionId = "";

        session = new Session(
            title: "Teste de Sessão",
            description: "Eu sou a primeira Sessão",
            professionalId: professionalId,
            patientId: patientId
        );

        StartCoroutine(RESTClient.Instance.InsertSession(OnInserted, session));
    }

    public void OnInserted(APIResponse response)
    {
        Debug.Log($"Inserted: {response}");
        insertedCount++;

        if (insertedCount >= 2)
        {
            StartCoroutine(RESTClient.Instance.FetchSessions(OnFetch, professionalId: professionalId, patientId: patientId));
            return;
        }

        firstSessionId = response.session.id;

        Movement movement = new Movement(
            description: "Eu sou um movimento da primeira Sessão",
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

        StartCoroutine(RESTClient.Instance.InsertMovement(OnInserted, movement));
    }

    public void OnFetch(APIResponse response)
    {
        Debug.Log($"Downloaded: {response}");

        foreach (SerializableSession serializableSession in response.sessions)
		{
            if (serializableSession.id == firstSessionId)
			{
                session = new Session(serializableSession);
                break;
            }
		}

        if (session.id != default(string))
		{
            session.title = "Atualizando a Sessão";
            StartCoroutine(RESTClient.Instance.UpdateSession(OnUpdated, session));
		}
        else
		{
            OnUpdated(null);
		}
    }

    public void OnUpdated(APIResponse response)
    {
        Debug.Log($"Updated: {response}");
        string id = response.session.id ?? session.id;
        StartCoroutine(RESTClient.Instance.DeleteSession(OnDeleted, id));
    }

    public void OnDeleted(APIResponse response)
    {
        Debug.Log($"Deleted: {response}");

        session = new Session(
            title: "Teste de Sessão 2",
            description: "Todos os movimentos da Sessão serão inseridos de uma vez",
            professionalId: professionalId,
            patientId: patientId,
            movements: new Movement[2]
            {
                new Movement(
                    label: "firstMovement",
                    fps: Application.targetFrameRate,
                    articulations: new string[] { "1", "2" },
                    articulationData: new Register[1] {
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
                    articulationData: new Register[1] {
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

        StartCoroutine(RESTClient.Instance.InsertSession(OnBulkInsert, session));
    }

    public void OnBulkInsert(APIResponse response)
	{
        Debug.Log($"Inserted: {response}");
        StartCoroutine(RESTClient.Instance.FindSession(OnFind, response.session.id));
    }

    public void OnFind(APIResponse response)
    {
        Debug.Log($"Found: {response}");
        StartCoroutine(RESTClient.Instance.DeleteSession(End, response.session.id));
    }

    public void End(APIResponse response)
    {
        Debug.Log($"Deleted: {response}");
    }
}
