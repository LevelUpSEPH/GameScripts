using System.Collections;
using UnityEngine;

public class FootprintBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem _footprintGenerator;

    private void OnEnable()
    {
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable()
    {
        LevelBehaviour.Started -= OnLevelStarted;
        StopAllCoroutines();
    }

    private void OnLevelStarted()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DisableAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }

    public void StartLifetime(float lifetime)
    {
        _footprintGenerator.Play();
        StartCoroutine(DisableAfterLifetime(lifetime));
    }

}
