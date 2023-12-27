using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] public int CollectedItems = 0;
    [SerializeField] TextMeshProUGUI Label = null;
    // onTrigger collision for pickable objects?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickables"))
        {
            CollectedItems++;
            Label.text = CollectedItems.ToString();
            Destroy(collision.gameObject);
        }
    }
}
