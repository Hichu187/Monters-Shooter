using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float UpdatedSpeed = 0.1f;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdatedSpeed);
        while (enabled)
        {
            agent.SetDestination(target.transform.position);
            yield return wait;
        }
    }
}
