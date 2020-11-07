using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Jogo : MonoBehaviour
{
    [SerializeField] private TMP_Text statsText = null;
    [SerializeField] private Porta[] portas = null;
    [SerializeField] private GameObject botoes = null;
    [SerializeField] private GameObject dedo = null;

    private int singlePlays = 0;
    private int singleWins = 0;
    private int singleLosses = 0;
    private int secondPlays = 0;
    private int secondWins = 0;
    private int secondLosses = 0;

    private int escolha = 0;
    private bool wait1 = false;
    private bool wait2 = false;

    void Start()
    {
        Load();
        RefreshStatsText();
        StartCoroutine(IStartGame());
    }

    void Load()
    {
        singlePlays = PlayerPrefs.GetInt("singlePlays", 0);
        singleWins = PlayerPrefs.GetInt("singleWins", 0);
        singleLosses = PlayerPrefs.GetInt("singleLosses", 0);
        secondPlays = PlayerPrefs.GetInt("secondPlays", 0);
        secondWins = PlayerPrefs.GetInt("secondWins", 0);
        secondLosses = PlayerPrefs.GetInt("secondLosses", 0);
    }

    void Save()
    {
        PlayerPrefs.SetInt("singlePlays", singlePlays);
        PlayerPrefs.SetInt("singleWins", singleWins);
        PlayerPrefs.SetInt("singleLosses", singleLosses);
        PlayerPrefs.SetInt("secondPlays", secondPlays);
        PlayerPrefs.SetInt("secondWins", secondWins);
        PlayerPrefs.SetInt("secondLosses", secondLosses);
    }

    public void RefreshStatsText()
    {
        string text =
@"Jogos jogados: {0}
---------------------
Não Troca {1}%:
ganhos: {2}
percas: {3}
---------------------
Troca {4}%:
ganhos: {5}
percas: {6}
";
        int totalGames = singlePlays + secondPlays;
        float singleT = singlePlays == 0 ? 0f : (float)singleWins / (float)singlePlays;
        float secondT = secondPlays == 0 ? 0f : (float)secondWins / (float)secondPlays;
        text = string.Format(text, totalGames, singleT * 100f, singleWins, singleLosses, secondT * 100f, secondWins, secondLosses);

        statsText.text = text;
    }

    IEnumerator IStartGame()
    {
        wait1 = false;
        wait2 = false;

        int r = Random.Range(0, portas.Length);
        for(int i = 0; i < portas.Length; i++)
        {
            portas[i].Reset();
            portas[i].IsCar = i == r;
        }
        botoes.SetActive(false);
        dedo.SetActive(false);

        while (!wait1) yield return null;

        for (int i = 0; i < portas.Length; i++)
        {
            portas[i].Disable();
        }

        while(true)
        {
            int rr = Random.Range(0, portas.Length);
            if(escolha != rr && !portas[rr].IsCar)
            {
                portas[rr].Reveal();
                break;
            }
        }
        botoes.SetActive(true);

        while (!wait2) yield return null;

        botoes.SetActive(false);
        RefreshStatsText();
        Save();

        yield return new WaitForSeconds(2f);

        StartCoroutine(IStartGame());

    }

    public void OnPortaClicked(int portaIndex)
    {
        escolha = portaIndex;
        wait1 = true;

        dedo.SetActive(true);
        dedo.transform.position = portas[portaIndex].transform.position;
    }

    public void OnSimClicked()
    {
        if (wait2) return;
        wait2 = true;
        secondPlays++;
        if(!portas[escolha].IsCar)
        {
            secondWins++;
        }
        else
        {
            secondLosses++;
        }

        for (int i = 0; i < portas.Length; i++)
        {
            var p = portas[i];
            if(i != escolha && !p.IsRevealed)
            {
                dedo.transform.position = p.transform.position;
            }
            p.Reveal();
        }
    }

    public void OnNaoClicked()
    {
        if (wait2) return;
        wait2 = true;
        singlePlays++;


        if (portas[escolha].IsCar)
        {
            singleWins++;
        }
        else
        {
            singleLosses++;
        }

        for (int i = 0; i < portas.Length; i++)
        {
            portas[i].Reveal();
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        StopAllCoroutines();
        Start();
    }
}
