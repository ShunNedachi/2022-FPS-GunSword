using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;

    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // ’e‚ÌˆÚ“®—p
        rigidBody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // íœˆ—‚ª“ü‚Á‚Ä‚¢‚È‚¢‚Ì‚ÅŒã‚Åì¬
    }
}
