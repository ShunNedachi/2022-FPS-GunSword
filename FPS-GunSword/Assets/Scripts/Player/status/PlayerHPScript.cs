using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPScript : MonoBehaviour
{
    public static PlayerHPScript instance;
    [SerializeField] private int hpMax = 1000;
    public Image hpImage;
    [SerializeField] public AudioClip damageSound;
    AudioSource audioSource;

    private int hp = 0;
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
        hp = hpMax;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    public void Sethp(int damage)
    {
        hp -= damage;
        audioSource.PlayOneShot(damageSound);

    }

    public int GethP()
    {
        return hp;
    }
    public void Draw()
    {
        float hpPer = (float)hp / (float)hpMax;
        hpImage.fillAmount = hpPer;
    }
}
