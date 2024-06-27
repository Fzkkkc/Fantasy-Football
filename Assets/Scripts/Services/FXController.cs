using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class FXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _moneyShowerFX;
        [SerializeField] private ParticleSystem _starShowerFX;
        [SerializeField] private ParticleSystem _hitFX;
        [SerializeField] private ParticleSystem _missFX;

        [SerializeField] private List<ParticleSystem> _particleSystems;
        
        public void PlayMoneyParticle()
        {
            _moneyShowerFX.gameObject.SetActive(true);
            _moneyShowerFX.Play();
        }
        
        public void PlayStarParticle()
        {
            _starShowerFX.gameObject.SetActive(true);
            _starShowerFX.Play();
        }
        
        public void PlayMissParticle()
        {
            _missFX.gameObject.SetActive(true);
            _missFX.Play();
        }
        
        public void PlayHitParticle()
        {
            _hitFX.gameObject.SetActive(true);
            _hitFX.Play();
        }

        private void DisableParticles()
        {
            foreach (var particle in _particleSystems)
            {
                particle.gameObject.SetActive(false);
            }
        }
        
        public void Init()
        {
            DisableParticles();
        }
    }
}