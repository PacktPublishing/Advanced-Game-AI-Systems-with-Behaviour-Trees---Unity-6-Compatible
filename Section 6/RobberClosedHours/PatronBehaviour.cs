using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronBehaviour : BTAgent
{
    public GameObject[] art;
    public GameObject frontdoor;
    public GameObject home;

    [Range(0, 1000)]
    public int boredom = 0;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        RSelector selectObject = new RSelector("Select Art To View");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }

        Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor);
        Leaf goToHome = new Leaf("Go Home", GoToHome);
        Leaf isBored = new Leaf("Is Bored?", IsBored);
        Leaf isOpen = new Leaf("Is Open?", IsOpen);

        Sequence viewArt = new Sequence("View Art");
        viewArt.AddChild(isOpen);
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);


        BehaviourTree whileBored = new BehaviourTree();
        whileBored.AddChild(isBored);

        Loop lookAtPaintings = new Loop("Look", whileBored);
        lookAtPaintings.AddChild(selectObject);

        viewArt.AddChild(lookAtPaintings);
        viewArt.AddChild(goToHome);

        BehaviourTree galleryOpenCondition = new BehaviourTree();
        galleryOpenCondition.AddChild(isOpen);
        DepSequence bePatron = new DepSequence("Be An Art Patron", galleryOpenCondition, agent);
        bePatron.AddChild(viewArt);

        Selector viewArtWithFallback = new Selector("View Art with Fallback");
        viewArtWithFallback.AddChild(bePatron);
        viewArtWithFallback.AddChild(goToHome);

        tree.AddChild(viewArtWithFallback);

        StartCoroutine("IncreaseBoredom");
    }

    IEnumerator IncreaseBoredom()
    {
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 150, 0, 1000);
        }
        return s;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontdoor);
        return s;
    }

    public Node.Status GoToHome()
    {
        Node.Status s = GoToLocation(home.transform.position);
        return s;
    }

    public Node.Status IsBored()
    {
        if (boredom < 100)
            return Node.Status.FAILURE;
        else
            return Node.Status.SUCCESS;
    }


}
