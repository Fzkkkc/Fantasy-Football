using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class FXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _moneyShowerFX;
        [SerializeField] private ParticleSystem _starShowerFX;

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