using UnityEngine;
using System.Collections;

public class BlockDestroy : MonoBehaviour {

	public GameObject brokenBlock;
	private bool isQuitting;

	void OnApplicationQuit(){
		isQuitting = true;
	}

	void OnDestroy(){
		if (!isQuitting) {
			Instantiate (brokenBlock, transform.position, transform.rotation);
		}

	}
}
