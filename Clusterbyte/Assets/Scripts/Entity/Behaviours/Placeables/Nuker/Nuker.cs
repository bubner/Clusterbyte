using System;
using UnityEngine;

namespace Entity.Behaviours.Placeables.Nuker
{
    public class Nuker : MonoBehaviour
    {
        /// <summary>
        /// The min number of enemies in Trigger radius to trigger the charge up and firing.
        /// </summary>
        public int minimumNukeEnemyCount = 3;
        /// <summary>
        /// Time to charge.
        /// </summary>
        public float chargeTimeSec = 5;

        [SerializeField] private GameObject head;
        [SerializeField] private GameObject nukePrefab;
        [SerializeField] private AudioSource alert;

        private Renderer headRenderer;
        private int enemyCount;
        private bool nuked;
        private float? startedCharging;

        internal void Awake()
        {
            head.TryGetComponent(out headRenderer);
            enemyCount = 0;
        }

        internal void Update()
        {
            if (enemyCount < minimumNukeEnemyCount)
            {
                headRenderer.material.color = Color.Lerp(headRenderer.material.color, Color.white, Time.deltaTime);
                transform.Rotate(Vector3.up, 45 * Time.deltaTime);
                alert.Stop();
                startedCharging = null;
                return;
            }

            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
            headRenderer.material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));

            if (!alert.isPlaying)
                alert.Play();

            startedCharging ??= Time.time;
            if (startedCharging + chargeTimeSec > Time.time)
                return;

            if (!nuked)
            {
                Instantiate(nukePrefab, transform.position + new Vector3(0, 30, 0), Quaternion.Euler(90, 180, 0));
                nuked = true;
            }

            Invoke(nameof(Cleanup), 3);
        }

        private void Cleanup()
        {
            Destroy(gameObject);
        }

        internal void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            enemyCount++;
        }

        internal void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            enemyCount--;
        }
    }
}