using UnityEngine;

public class LanderVisual : MonoBehaviour
{

    [SerializeField] private ParticleSystem leftTrusterParticleSystem;
    [SerializeField] private ParticleSystem middleTrusterParticleSystem;
    [SerializeField] private ParticleSystem rightTrusterParticleSystem;

    void Start()
    {
        Lander lander = GetComponent<Lander>();
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

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

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }

    void Update()
    {

    }
}
