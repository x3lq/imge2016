using UnityEngine;
using System.Collections;

public class Kill : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector3(transform.position.x - 10, transform.position.y + 0.5f, transform.position.z), Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = new Vector3(transform.position.x - 10, transform.position.y + 0.5f, transform.position.z);
        Ray Ray = new Ray(Pos, Vector3.right);
        RaycastHit Hit;

        if (Physics.Raycast(Ray, out Hit, 20f))
        {
            if (!Hit.collider.gameObject.GetComponent<Event>().pressed) { EventHandler.Score -= 20; }
            Destroy(Hit.collider.gameObject);
        }
    }
}
