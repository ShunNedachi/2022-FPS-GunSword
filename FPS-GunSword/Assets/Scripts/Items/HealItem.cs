using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public HealItem instance;
    [SerializeField] private float moveDistance = 10;
    [SerializeField] private float hitDistance = 1;
    [SerializeField] private float speed = 1;

    GameObject player;

    private bool distFlg= false;
    // Start is called before the first frame update

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    void Start()
    {
       
    }

    public void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
       player = GameObject.Find("Player");

    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        if(dist < moveDistance&&!distFlg)
        {
            distFlg = true;
        }
        if(distFlg)
        {
            Vector3 vector = player.transform.position - this.transform.position;
            vector.Normalize();
            vector*=speed;
            this.transform.position += vector;
        }
        if(dist<hitDistance)
        {
            PlayerEnergyScript.instance.SetEnergyItem();
            Destroy(transform.gameObject);
        }
    }
}
