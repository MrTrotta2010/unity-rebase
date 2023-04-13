using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReBase;

public class PaginationExample : MonoBehaviour
{
    int currentPage = 1;
    bool useIdBasePagination = false;

    public void RunPaginationExample()
    {
        currentPage = 1;
        useIdBasePagination = false;

        RunMovementPagination();
    }

    void RunMovementPagination()
	{
        Debug.Log("Movement pagination!");
        Debug.Log("Pagination with 'page' and 'per'");
        StartCoroutine(RESTClient.Instance.FetchMovements(OnFetchMovements, professionalId: "MrTrotta2010", page: currentPage, per: 2));
    }

    void RunSessionsPagination()
	{
        currentPage = 1;
        useIdBasePagination = false;
        Debug.Log("Session pagination!");
        Debug.Log("Pagination with 'page' and 'per'");
        StartCoroutine(RESTClient.Instance.FetchSessions(OnFetchSessions, professionalId: "MrTrotta2010", page: currentPage, per: 2));
    }

    void OnFetchMovements(APIResponse response)
	{
        bool foundAnything = response.movements != null;

        Debug.Log($"Page {currentPage}: {(foundAnything ? response.movements.Length.ToString() : "no")} movements");

        if (useIdBasePagination)
		{
            if (currentPage < 2 && foundAnything)
			{
                string previousId = response.movements[response.movements.Length - 1].id;
                Debug.Log($"The last ID in the list is {previousId}");
                currentPage++;
                StartCoroutine(RESTClient.Instance.FetchMovements(OnFetchMovements, professionalId: "MrTrotta2010", previousId: previousId, per: 2));
            }
            else
            {
                RunSessionsPagination();
                return;
            }
        }
        else
		{
            if (currentPage < 2)
            {
                currentPage++;
                StartCoroutine(RESTClient.Instance.FetchMovements(OnFetchMovements, professionalId: "MrTrotta2010", page: currentPage, per: 2));
            }
            else
            {
                currentPage = 1;
                useIdBasePagination = true;
                Debug.Log("ID-based pagination");
                StartCoroutine(RESTClient.Instance.FetchMovements(OnFetchMovements, professionalId: "MrTrotta2010", per: 2));
            }
        }
	}

    void OnFetchSessions(APIResponse response)
    {
        bool foundAnything = response.sessions != null;

        Debug.Log($"Page {currentPage}: {(foundAnything ? response.sessions.Length.ToString() : "no")} sessions");

        if (useIdBasePagination)
        {
            if (currentPage == 2 || !foundAnything) return;

            currentPage++;
            string previousId = response.sessions[response.sessions.Length - 1].id;
            Debug.Log($"The last ID in the list is {previousId}");
            StartCoroutine(RESTClient.Instance.FetchSessions(OnFetchSessions, professionalId: "MrTrotta2010", previousId: previousId, per: 2));
        }
        else
        {
            if (currentPage < 2)
            {
                currentPage++;
                StartCoroutine(RESTClient.Instance.FetchSessions(OnFetchSessions, professionalId: "MrTrotta2010", page: currentPage, per: 2));
            }
            else
            {
                currentPage = 1;
                useIdBasePagination = true;
                Debug.Log("ID-based pagination");
                StartCoroutine(RESTClient.Instance.FetchSessions(OnFetchSessions, professionalId: "MrTrotta2010", per: 2));
            }
        }
    }
}
