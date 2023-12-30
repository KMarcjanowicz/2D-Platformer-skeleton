using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private AudioSource DeathAudio = null;
    private Rigidbody2D RB2D = null;
    private Animator Anim = null;
    private bool alive = true;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Traps" && alive)
        {
            Die();
            alive = false;
        }
    }

    private void Die() {
        RB2D.bodyType = RigidbodyType2D.Static;
        Anim.SetTrigger("death");   
        if(DeathAudio != null)
        {
            DeathAudio.Play();
        }
    }

    //used in animation
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
