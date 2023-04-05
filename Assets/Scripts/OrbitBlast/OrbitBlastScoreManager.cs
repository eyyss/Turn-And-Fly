using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.OrbitBlast
{
    public class OrbitBlastScoreManager : MonoBehaviour
    {
        public static OrbitBlastScoreManager Instance;

        private int score;
        public TextMeshPro scoreText;
        public TextMeshPro highScoreText;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            GetHighScore();
            RotatingSphere.OnPlayerHit += RotatingSphere_OnPlayerHit;
            OrbitBlastGameManager.OnGameStart += OrbitBlastGameManager_OnGameStart;
        }



        private void OnDestroy()
        {
            RotatingSphere.OnPlayerHit -= RotatingSphere_OnPlayerHit;
            OrbitBlastGameManager.OnGameStart -= OrbitBlastGameManager_OnGameStart;

        }
        private void RotatingSphere_OnPlayerHit()
        {
            score += UnityEngine.Random.Range(5, 10);
            scoreText.text = score.ToString();
            scoreText.transform.DOPunchScale(new Vector3(.15f, .15f, .15f), .5f, 1);
            SaveScoreHighScore();
        }

        private void SaveScoreHighScore()
        {
            if (PlayerPrefs.HasKey("OrbitBlastHighScore"))
            {
                if (score>PlayerPrefs.GetInt("OrbitBlastHighScore"))
                {
                    PlayerPrefs.SetInt("OrbitBlastHighScore", score);
                    highScoreText.text = "New High Score: " + Environment.NewLine + score;
                }
            }
            else
            {
                PlayerPrefs.SetInt("OrbitBlastHighScore", score);
                highScoreText.text = "High Score: " + Environment.NewLine + score;
            }
        }
        private void GetHighScore()
        {
            highScoreText.text = "High Score:" + Environment.NewLine + PlayerPrefs.GetInt("OrbitBlastHighScore").ToString();
            highScoreText.transform.DOPunchScale(new Vector3(.15f, .15f, .15f), .5f, 1);
        }
        public int GetScore()
        {
            return score;
        }
        private void OrbitBlastGameManager_OnGameStart()
        {
            score = 0;
            scoreText.text = score.ToString();
        }
    }
}

