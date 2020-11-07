using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Porta : MonoBehaviour
{
    public bool IsCar { get; set; }
    public bool IsRevealed { get;  private set; }

    [SerializeField] Sprite[] sprites = null;
    [SerializeField] Image image = null;
    [SerializeField] Button button = null;
    [SerializeField] GameObject goat = null;
    [SerializeField] GameObject car = null;


    public void Reset()
    {
        IsRevealed = false;
        image.enabled = true;
        image.sprite = sprites[Random.Range(0, sprites.Length)];
        goat.SetActive(false);
        car.SetActive(false);
        button.enabled = true;
    }

    public void Reveal()
    {
        IsRevealed = true;
        image.enabled = false;
        goat.SetActive(!IsCar);
        car.SetActive(IsCar);
    }

    public void Disable()
    {
        button.enabled = false;
    }
}
