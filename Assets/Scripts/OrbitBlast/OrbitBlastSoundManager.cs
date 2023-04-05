using Game.OrbitBlast;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.OrbitBlast
{
    public class OrbitBlastSoundManager : MonoBehaviour
    {
        public List<AudioClip> clips;

        private void Start()
        {
            PlayerSphere.OnMoveToForward += PlayerSphere_OnMoveToForward;
            RotatingSphere.OnPlayerHit += RotatingSphere_OnPlayerHit;
        }

        private void OnDestroy()
        {
            PlayerSphere.OnMoveToForward -= PlayerSphere_OnMoveToForward;
            RotatingSphere.OnPlayerHit -= RotatingSphere_OnPlayerHit;
        }
        private void PlayerSphere_OnMoveToForward()
        {
            float pitch = Random.Range(.9f, 1.1f);
            PlayClipAtPoint("throw", pitch);
        }

        private void RotatingSphere_OnPlayerHit()
        {
            string clipName = "pop";
            int r = Random.Range(1, 4);
            clipName = clipName + r.ToString();
            float pitch = Random.Range(.9f, 1.1f);
            PlayClipAtPoint(clipName, pitch);

        }

        private AudioClip GetClip(string clipName)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i].name == clipName)
                {
                    return clips[i];
                }
            }
            return null;
        }
        public void PlayClipAtPoint(string soundName, float pitch)
        {
            var clip = GetClip(soundName);
            if (clip == null) return;
            GameObject obj = new GameObject(soundName);
            var source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.clip = clip;
            source.pitch = pitch;
            source.Play();
            Destroy(obj, clip.length);
        }
    }
}

