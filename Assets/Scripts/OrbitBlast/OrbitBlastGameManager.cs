using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.OrbitBlast
{
    public class OrbitBlastGameManager : MonoBehaviour
    {
        public static OrbitBlastGameManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        public int targetFrameRate = 60;

        public bool gameStarted;
        public bool gameOver;

        public Ease defaultEase;

        public static event Action OnGameStart;


        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            PlayerSphere.GameOver += PlayerSphere_GameOver;
            OnGameStart += OrbitBlastGameManager_OnGameStart;
        }
        private void OnDestroy()
        {
            PlayerSphere.GameOver -= PlayerSphere_GameOver;
            OnGameStart -= OrbitBlastGameManager_OnGameStart;
        }

        private void PlayerSphere_GameOver()
        {
            gameOver = true;
            gameStarted = false;
            Camera.main.transform.DOMove(new Vector3(0, 13, -10), 1.5f).SetEase(defaultEase);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)&&!gameStarted)
            {
                OnGameStart?.Invoke();
            }
        }

        private void OrbitBlastGameManager_OnGameStart()
        {
            Camera.main.transform.DOMove(new Vector3(0, 1, -10), 1.5f).SetEase(defaultEase).OnComplete(() =>
            {
                gameOver = false;
                gameStarted = true;
            });
        }
        public void ChangeScene(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}

