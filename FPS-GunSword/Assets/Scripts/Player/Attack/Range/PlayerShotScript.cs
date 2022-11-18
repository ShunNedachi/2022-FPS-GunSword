using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotScript : MonoBehaviour
{
    public static PlayerShotScript instance;

    public float rayDistance = 100;
    [SerializeField] private int shootInterval = 30;
    [SerializeField] private int reloadInterval = 120;
    [SerializeField] private float damage = 25;
    [SerializeField] private GameObject healItem;

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

        if (Input.GetMouseButton(0) && shootIntervalTimer > shootInterval)
        {
            PlayerSlashScript.instance.ModeChange();
            if(PlayerMagazineScript.instance.GetRemainingBullets()>0)
            {
                var direction = trans.forward;

               Vector3 rayPosition = trans.position + new Vector3(0.0f, 0.0f, 0.0f);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                PlayerMagazineScript.instance.Shot();

                RaycastHit hit;
                if(Physics.Raycast(ray,out hit) )//&& gameObject.tag = "Enemy"
                {
                    if(hit.collider.CompareTag("MeleeEnemy"))
                    {
                        //hit.collider.gameObject.GetComponent<DefaultEnemy>().GetDamage(damage);
                        Instantiate(healItem, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);

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
        public void ModeChange()
    {
        shootIntervalTimer = 0;
    }
}
