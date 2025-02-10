using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
	public Text clock;
	public float timeOfDay;

	static Blackboard instance;
	public static Blackboard Instance
	{
		get
		{
			if (!instance)
			{
				Blackboard[] managers = GameObject.FindObjectsOfType<Blackboard>();
				if (managers != null)
					if (managers.Length == 1)
					{
						instance = managers[0];
						return managers[0];
					}
				GameObject go = new GameObject("Blackboard", typeof(Blackboard));
				instance = go.GetComponent<Blackboard>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
		set
		{
			instance = value as Blackboard;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		timeOfDay = 0;
		StartCoroutine("UpdateClock");
    }

	IEnumerator UpdateClock()
	{
		while (true)
		{
			timeOfDay++;
			if (timeOfDay > 23) timeOfDay = 0;
			clock.text = timeOfDay + ":00";
			yield return new WaitForSeconds(1);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
