using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotScript : MonoBehaviour
{
    public float rayDistance;

    private void Update()
    {
        Transform trans = transform;

        if (Input.GetMouseButton(0))
        {
            var direction = trans.forward;

            Vector3 rayPosition = trans.position + new Vector3(0.0f, 0.0f, 0.0f);
            Ray ray = Camera.main .ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(rayPosition, direction * -rayDistance, UnityEngine.Color.red);

            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                Debug.Log("HIT‚µ‚Ü‚µ‚½‚æ");

                if(hit.collider.tag == "OBJECT")
                {
                    Destroy(hit.collider.gameObject);
                }
            }

        }
    }
}
