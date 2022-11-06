using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    void Start()
    {
        Bake();
    }

    void Update()
    {
        
    }

    //IEnumerator Baker()
    //{
    //    yield return new WaitForSeconds(0);
    //    GetComponent<NavMeshSurface>().BuildNavMesh();
    //}

    public void Bake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
