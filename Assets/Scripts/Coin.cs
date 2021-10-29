using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip hitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "car" || other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            GameManager.Instance.AddScore(10);

            if (hitSound != null)
                GameManager.Instance.PlaySound(hitSound);
        }
    }
}