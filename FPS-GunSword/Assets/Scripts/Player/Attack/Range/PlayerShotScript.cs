using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotScript : MonoBehaviour
{
    public static PlayerShotScript instance;

    public float rayDistance;
    [SerializeField] private int shootInterval = 30;
    [SerializeField] private int reloadInterval = 120;

    private int shootIntervalTimer = 0;
    private int reloadTimer = 0;

    public void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        PlayerMagazineScript.instance.Start();
    }

    public void Update()
    {
        Transform trans = transform;
        shootIntervalTimer ++;
        reloadTimer ++;

        if (Input.GetMouseButton(0) && shootIntervalTimer >= shootInterval)
        {
            if(PlayerMagazineScript.instance.GetRemainingBullets()>0)
            {
                var direction = trans.forward;

               Vector3 rayPosition = trans.position + new Vector3(0.0f, 0.0f, 0.0f);
                Ray ray = Camera.main .ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(rayPosition, direction * -rayDistance, UnityEngine.Color.red);

                PlayerMagazineScript.instance.Shot();

                RaycastHit hit;
                if(Physics.Raycast(ray,out hit) )//&& gameObject.tag = "Enemy"
                {
                    Debug.Log("HIT");

                    if(hit.collider.tag == "OBJECT")
                    {
                       Destroy(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                PlayerMagazineScript.instance.Reload();
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && reloadTimer >= reloadInterval)
        {
            // �^�C�}�[�̏�����
            reloadTimer = 0;
            PlayerMagazineScript.instance.Reload();
        }
    }
}
