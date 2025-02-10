using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject diamond;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;

    [Range(0, 1000)]
    public int money = 800;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector opendoor = new Selector("Open Door");

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);

        steal.AddChild(invertMoney);
        steal.AddChild(opendoor);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();

    }

    public Node.Status HasMoney()
    {
        if(money < 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backdoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontdoor);
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            money += 300;
            diamond.SetActive(false);
        }
        return s;
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }
}
