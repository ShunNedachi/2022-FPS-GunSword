using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotScript : MonoBehaviour
{
    public float rayDistance;
    [SerializeField] private int magazineMax;
    [SerializeField] private float shootInterval = 3.0f;
    [SerializeField] private float reloadInterval = 3.0f;

    private float shootIntervalTimer = 0;
    private float reloadTimer = 0;
    private int magazine;

    // Start is called before the first frame update
    void Start()
    {
        magazine = magazineMax;
    }

    private void Update()
    {
        Transform trans = transform;
        timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && magazine>0 &&shootIntervalTimer >= shootInterval)
        {
            var direction = trans.forward;

            Vector3 rayPosition = trans.position + new Vector3(0.0f, 0.0f, 0.0f);
            Ray ray = Camera.main .ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(rayPosition, direction * -rayDistance, UnityEngine.Color.red);

            magazine--;

            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                Debug.Log("HITしましたよ");

                if(hit.collider.tag == "OBJECT")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && reloadTimer >= reloadInterval)
        {
            // タイマーの初期化
            timer = 0;
            Reload();
        }
    }

    private void Reload()
    {
        magazine = magazineMax;
    }
}
