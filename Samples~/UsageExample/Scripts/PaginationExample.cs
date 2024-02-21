using UnityEngine;
using ReBase;

public class PaginationExample : MonoBehaviour
{
	private ReBaseClient client = new ReBaseClient("seu@email.com", "seuToken");

	int currentPage = 1;
    bool useIdBasePagination = false;

    public void RunPaginationExample()
    {
        currentPage = 1;
        useIdBasePagination = false;

        RunMovementPagination();
    }

    async void RunMovementPagination()
    {
        Debug.Log("Movement pagination!");
        Debug.Log("Pagination with 'page' and 'per'");
        APIResponse response = await client.FetchMovements(professionalId: "MrTrotta2010", page: currentPage, per: 2);
        OnFetchMovements(response);
    }

    async void RunSessionsPagination()
    {
        currentPage = 1;
        useIdBasePagination = false;
        Debug.Log("Session pagination!");
        Debug.Log("Pagination with 'page' and 'per'");
		APIResponse response = await client.FetchSessions(professionalId: "MrTrotta2010", page: currentPage, per: 2);
        OnFetchSessions(response);
	}

    async void OnFetchMovements(APIResponse response)
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
				OnFetchMovements(await client.FetchMovements(professionalId: "MrTrotta2010", previousId: previousId, per: 2));
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
				OnFetchMovements(await client.FetchMovements(professionalId: "MrTrotta2010", page: currentPage, per: 2));
            }
            else
            {
                currentPage = 1;
                useIdBasePagination = true;
                Debug.Log("ID-based pagination");
                OnFetchMovements(await client.FetchMovements(professionalId: "MrTrotta2010", per: 2));
            }
        }
    }

    async void OnFetchSessions(APIResponse response)
    {
        bool foundAnything = response.sessions != null;

        Debug.Log($"Page {currentPage}: {(foundAnything ? response.sessions.Length.ToString() : "no")} sessions");

        if (useIdBasePagination)
        {
            if (currentPage == 2 || !foundAnything) return;

            currentPage++;
            string previousId = response.sessions[response.sessions.Length - 1].id;
            Debug.Log($"The last ID in the list is {previousId}");
			OnFetchSessions(await client.FetchSessions(professionalId: "MrTrotta2010", previousId: previousId, per: 2));
        }
        else
        {
            if (currentPage < 2)
            {
                currentPage++;
                OnFetchSessions(await client.FetchSessions(professionalId: "MrTrotta2010", page: currentPage, per: 2));
            }
            else
            {
                currentPage = 1;
                useIdBasePagination = true;
                Debug.Log("ID-based pagination");
                OnFetchSessions(await client.FetchSessions(professionalId: "MrTrotta2010", per: 2));
            }
        }
    }
}
