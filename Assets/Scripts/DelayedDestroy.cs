using UnityEngine;
using System.Collections;

public class DelayedDestroy : MonoBehaviour {

	public bool hasExplosion = false;
	public float explosionPower = 100f;

    public bool objectFades = false;
    public Color fadeRate = new Color (0f, 0f, 0f, 50f);
    public bool shouldObjectFade = false;

	public float secondsDelayed = 3.0f;

    ObjectPool_HG pool;

    public MeshRenderer meshRenderer;

    void Start () {
        pool = GameObject.Find("GameManager").GetComponent<ObjectPool_HG>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (hasExplosion == true)
        {
            Rigidbody[] rigidBody = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidBody)
            {
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionPower, transform.position, 0.7f);
                }
            }
        }

        StartCoroutine("DestroyObjectCoroutine");
	}

    private void Update()
    {
        if (shouldObjectFade)
        {
                meshRenderer.material.color -= fadeRate * Time.deltaTime;
                if (meshRenderer.material.color.a <= 0)
                {
                    shouldObjectFade = false;
                }
        }
    }

    IEnumerator DestroyObjectCoroutine()
    {
        if (objectFades)
        {
            shouldObjectFade = true;
        }
        yield return new WaitForSeconds(secondsDelayed);
        pool.DestroyGameObject(gameObject, true, false, 0);
    }
}
