using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReBase;

public class PaginationExample : MonoBehaviour
{
    int currentPage = 1;
    bool useIdBasePagination = false;

    void Start()
    {
        Debug.Log("Pagination with 'page' and 'per'");
        StartCoroutine(RESTClient.Instance.FetchMovements(OnFetch, professionalId: "MrTrotta2010", page: currentPage, per: 2));
    }

    void OnFetch(APIResponse response)
	{
        Debug.Log($"Page {currentPage}: {response.movements.Length} movements");
        Debug.Log(response);

        if (useIdBasePagination)
		{
            if (currentPage == 2) return;

            currentPage++;
            string previousId = response.movements[response.movements.Length - 1].id;
            Debug.Log($"The last ID in the list is {previousId}");
            StartCoroutine(RESTClient.Instance.FetchMovements(OnFetch, professionalId: "MrTrotta2010", previousId: previousId, per: 2));
        }
        else
		{
            if (currentPage < 2)
            {
                currentPage++;
                StartCoroutine(RESTClient.Instance.FetchMovements(OnFetch, professionalId: "MrTrotta2010", page: currentPage, per: 2));
            }
            else
            {
                currentPage = 1;
                useIdBasePagination = true;
                Debug.Log("ID-based pagination");
                StartCoroutine(RESTClient.Instance.FetchMovements(OnFetch, professionalId: "MrTrotta2010", per: 2));
            }
        }
	}
}
