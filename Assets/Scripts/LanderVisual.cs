using UnityEngine;

public class LanderVisual : MonoBehaviour
{

    [SerializeField] private ParticleSystem leftTrusterParticleSystem;
    [SerializeField] private ParticleSystem middleTrusterParticleSystem;
    [SerializeField] private ParticleSystem rightTrusterParticleSystem;
    [SerializeField] private GameObject landerExplosionVfx;

    void Start()
    {
        Lander.Instance.OnUpForce += Lander_OnUpForce;
        Lander.Instance.OnLeftForce += Lander_OnLeftForce;
        Lander.Instance.OnRightForce += Lander_OnRightForce;
        Lander.Instance.OnBeforeForce += Lander_OnBeforeForce;
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;

        SetEnabledThrusterParticleSystem(leftTrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleTrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightTrusterParticleSystem, false);
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftTrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleTrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightTrusterParticleSystem, false);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftTrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(middleTrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightTrusterParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightTrusterParticleSystem, true);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftTrusterParticleSystem, true);
    }
    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        if (e.landingType != Lander.LandingType.Success)
        {
            Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
