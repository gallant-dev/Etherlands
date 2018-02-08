using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour {

	[SerializeField] GameObject floorBelow;
	public Transform ether;
	public float differenceInYPosition = -5.0f;

	// Use this for initialization
	void Start () {
        ether = GameObject.Find("Ether").transform;
	}

	// Update is called once per frame
	void Update () {
		if((ether.position.y + differenceInYPosition) <= floorBelow.transform.position.y){
			floorBelow.SetActive(true);
		}
	}
}
