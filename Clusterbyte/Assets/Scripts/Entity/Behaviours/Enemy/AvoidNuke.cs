using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Behaviours.Placeables.Nuker;
using Entity.Factory;
using Entity.Factory.Info;
using UnityEngine;
using UnityEngine.AI;

namespace Entity.Behaviours.Enemy
{
    /// <summary>
    /// Avoids any nukes by speeding up. Very dubious.
    /// </summary>
    public class AvoidNuke : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        
        private bool run;

        internal void Start()
        {
            StartCoroutine(RunWatchdog());
        }

        private IEnumerator RunWatchdog()
        {
            while (GameManager.instance.state == GameManager.ACTIVE)
            {
                yield return new WaitForSeconds(0.33f);
                run = false;
            }
        } 
        
        internal void Update()
        {
            navMeshAgent.speed = run ? 12 : 0.5f;
        }

        internal void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Nuker _) && !run)
                EntityFactory.Get<Popup>().SendText("Big Boy has detected a Nuker and sped up!", Popup.Type.WARNING);
        }

        internal void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Nuker _) || run)
                return;
            run = true;
        }
    }
}