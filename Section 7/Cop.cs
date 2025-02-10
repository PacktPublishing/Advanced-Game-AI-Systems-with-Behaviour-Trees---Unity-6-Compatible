using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BTAgent
{
    public GameObject[] patrolPoints;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Sequence selectPatrolPoint = new Sequence("Select Patrol Point");
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf gta = new Leaf("Go to " + patrolPoints[i].name, i, GoToPatrolPoint);
            selectPatrolPoint.AddChild(gta);
        }

        tree.AddChild(selectPatrolPoint);
    }

    public Node.Status GoToPatrolPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);
        return s;
    }

}