using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float cooldown = 1f;
    [SerializeField] ParticleSystem particleSys;
    float timer = 0f;
    bool canDestroy = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (canDestroy == true) return;
        timer += Time.deltaTime;
        if (timer >= cooldown) 
        {
            canDestroy = true;
            timer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Stackable stackable = other.GetComponent<Stackable>();
        if (stackable!=null & canDestroy)
        {
            stackable.holder.GetComponent<Player>().ApplySlowdown(2f);
            stackable.holder.RemoveFromStack(stackable);
            particleSys.Play();
            Destroy(stackable.gameObject);
            canDestroy = false;
        }
    }
}
