using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject diamond;
    public GameObject painting;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;

    GameObject pickup;

    [Range(0, 1000)]
    public int money = 800;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector opendoor = new Selector("Open Door");
        Selector selectObject = new Selector("Select Object to Steal");

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);

        steal.AddChild(invertMoney);
        steal.AddChild(opendoor);

        selectObject.AddChild(goToDiamond);
        selectObject.AddChild(goToPainting);

        steal.AddChild(selectObject);

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
        if (!diamond.activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
            pickup = diamond;
        }
        return s;
    }

    public Node.Status GoToPainting()
    {
        if (!painting.activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(painting.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            painting.transform.parent = this.gameObject.transform;
            pickup = painting;
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
            pickup.SetActive(false);
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
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }

}
