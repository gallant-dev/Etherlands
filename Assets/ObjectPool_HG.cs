using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_HG : MonoBehaviour {

    List<PooledObject> pooledObjectList = new List<PooledObject>();
    List<PooledObject> objectsToRemoveList = new List<PooledObject>();

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
        RemoveObjectsFromList();
    }

    public void DestroyGameObject(GameObject gO, bool shoudDestroyGameObject, bool doesObjectRespawn, float respawnTime)
    {
        pooledObjectList.Add(new PooledObject(gO, shoudDestroyGameObject, doesObjectRespawn, respawnTime));
    }

    void RemoveObjectsFromList()
    {
        objectsToRemoveList = pooledObjectList.FindAll(obj => obj.destroyObject == true);

        if (objectsToRemoveList.Count >= 1)
        {
            Destroy(objectsToRemoveList[0].pooledGameObject);
            pooledObjectList.Remove(objectsToRemoveList[0]);
            objectsToRemoveList.RemoveAt(0);
        }
    }


}

public class PooledObject
{
    public GameObject pooledGameObject;
    public bool destroyObject;
    public bool doesObjectRespawn;
    public float respawnTime;

    public PooledObject (GameObject newPooledGameObject, bool newDestroyObject, bool newDoesObjectRespawn, float newRespawnTime)
    {
        pooledGameObject = newPooledGameObject;
        destroyObject = newDestroyObject;
        doesObjectRespawn = newDoesObjectRespawn;
        respawnTime = newRespawnTime;
    }

}
