using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Game.OrbitBlast
{
    public class RotatingSphere : MonoBehaviour
    {
        public GameObject spherePrefab;
        public GameObject boomParticlePrefab;
        private Collider _collider;

        [Header("Difficulty")]
        public float rotateMotionScore;
        [Range(0,7)]public float rotateWaitTime;

        public static event Action OnPlayerHit;

        private void Start()
        {
            transform.localScale = new Vector3(.1f, .1f, .1f);
            transform.DOScale(Vector3.one,.5f);
            float z = UnityEngine.Random.Range(0, 360);
            transform.parent.eulerAngles = new Vector3(0, 0, z);
            _collider = GetComponent<Collider>();
            OnPlayerHit += RotatingSphere_OnPlayerHit;

            StartCoroutine(GoRandomRotation());
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                OnPlayerHit?.Invoke();
            }
        }
        private void RotatingSphere_OnPlayerHit()
        {
            SpawnSphere();
            Instantiate(boomParticlePrefab, transform.position, Quaternion.identity);
            // particle oynat
        }
        public void SpawnSphere()
        {
            Instantiate(spherePrefab,this.transform.parent);
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            OnPlayerHit -= RotatingSphere_OnPlayerHit;
            StopCoroutine(GoRandomRotation());
        }

        public IEnumerator GoRandomRotation()
        {
            int score = OrbitBlastScoreManager.Instance.GetScore();
            float WaitTime = UnityEngine.Random.Range(2.5f, rotateWaitTime);
            yield return new WaitForSeconds(WaitTime);
            if (score< rotateMotionScore)
            {
                StartCoroutine(GoRandomRotation());
                yield break;
            }
            float z = UnityEngine.Random.Range(0, 360);
            transform.parent.DORotate(new Vector3(0, 0, z), 1f).SetEase(Ease.InCubic);
            StartCoroutine(GoRandomRotation());
        }


    }

}
