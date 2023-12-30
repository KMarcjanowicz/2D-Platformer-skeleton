using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("Collected iterator")]
    [Space]

    [SerializeField] public int CollectedItems = 0;

    [Header("GameObject variables")]
    [Space]

    [SerializeField] private TextMeshProUGUI Label = null;

    [Header("Audio")]
    [Space]

    [SerializeField] private AudioSource CollectAudio = null;

    // onTrigger collision for pickable objects?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickables"))
        {
            CollectedItems++;
            Label.text = CollectedItems.ToString();


            if (CollectAudio != null )
            {
                CollectAudio.Play();
            }

            Destroy(collision.gameObject);
        }
    }
}
