using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.OrbitBlast
{
    public class PlayerSphere : MonoBehaviour
    {

        private int xDirection;
        private Vector3 startPosition;

        public float rotateSpeed;
        public float dashForce;
        public float dashParticleCoolDown;
        public float dashParticleDestroyTime;
        public float moveTime;
        public GameObject sphere;
        public GameObject arrow;
        public GameObject dashParticlePrefab;

        private bool clicked;
        private bool isClickble;
        private int clickedScore;
        private float dashParticleTimer;

        public static event Action OnMoveToForward;
        public static event Action GameOver;

        void Start()
        {
            OrbitBlastGameManager.OnGameStart += OrbitBlastGameManager_OnGameStart;
            startPosition = sphere.transform.localPosition;
            xDirection = 1;
            isClickble = true;
            dashParticleTimer = dashParticleCoolDown;
        }


        void Update()
        {
            if (!OrbitBlastGameManager.Instance.gameStarted) return;
            if (!clicked&& !OrbitBlastGameManager.Instance.gameOver)
            {
                transform.Rotate(new Vector3(0, 0, xDirection * rotateSpeed * Time.deltaTime));
            }
            if (Input.GetMouseButtonDown(0) && isClickble&&!OrbitBlastGameManager.Instance.gameOver)
            {
                OnMoveToForward?.Invoke();
                clickedScore = OrbitBlastScoreManager.Instance.GetScore();
                clicked = true;
                isClickble = false;
                arrow.SetActive(false);
                StartCoroutine(StopPressed());
            }
            if (clicked)
            {
                dashParticleTimer += Time.deltaTime;
                if (dashParticleTimer>dashParticleCoolDown)
                {
                    dashParticleTimer = 0;
                    Destroy(Instantiate(dashParticlePrefab, sphere.transform.position,Quaternion.identity),dashParticleDestroyTime);
                }
                sphere.transform.localPosition += new Vector3(0, dashForce * Time.deltaTime, 0);
            }
        }
        IEnumerator StopPressed()
        {
            yield return new WaitForSeconds(moveTime);
            clicked = false;
            if (OrbitBlastScoreManager.Instance.GetScore()==clickedScore)
            {
                GameOver?.Invoke();
                isClickble = false;
                yield return null;
            }
            sphere.transform.DOLocalMove(startPosition, moveTime).OnComplete(() =>
            {
                isClickble = true;
            });
            xDirection = -1 * xDirection;
            arrow.SetActive(true);
        }


        private void OrbitBlastGameManager_OnGameStart()
        {
            isClickble = true;
            clicked = false;
            arrow.SetActive(true);
        }

        private void BadSphere_OnPlayerSphereHit()
        {
            GameOver.Invoke();
        }
        private void OnDestroy()
        {
            OrbitBlastGameManager.OnGameStart -= OrbitBlastGameManager_OnGameStart;
        }

    }
}

