using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReBase;

public class SessionExample : MonoBehaviour
{
    private Session session;
    private int insertedCount = 0;

    void Awake()
    {
        Application.targetFrameRate = 30;
    }

    void Start()
    {
        Movement movement = new Movement(
            title: "Teste de Sessão",
            description: "Eu sou o primeiro movimento da Sessão",
            sessionId: "test1",
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

        StartCoroutine(RESTClient.Instance.InsertMovement(OnInserted, movement));
    }

    public void OnInserted(APIResponse response)
    {
        Debug.Log($"Inserted: {response}");
        insertedCount++;

        if (insertedCount == 2)
        {
            session = new Session(response.movement);
            session.title = "Atualizando a Sessão";
            StartCoroutine(RESTClient.Instance.UpdateSession(OnUpdated, session));
            return;
        }

        Movement movement = new Movement(
            title: "Teste de Sessão",
            description: "Eu sou o segundo movimento da Sessão",
            sessionId: "test1",
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

        StartCoroutine(RESTClient.Instance.InsertMovement(OnInserted, movement));
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
        StartCoroutine(RESTClient.Instance.FetchSessions(OnFetch, professionalId: session.professionalId, patientId: session.patientId));
    }

    public void OnFetch(APIResponse response)
    {
        Debug.Log($"Downloaded: {response}");
    }
}
