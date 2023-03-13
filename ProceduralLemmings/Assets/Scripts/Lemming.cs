using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lemming : MonoBehaviour
{
    private Animator _animator;
    private int _biome = -1;
    private List<Vector3> _path;
    private int PathIndex = 0;
    private float Speed = .5f;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetBiome(int biome, List<Vector3> path)
    {
        _biome = biome;
        _animator.SetInteger("Biome", biome);
        _animator.SetBool("Static", path.Count == 0);
        if (path.Count > 0)
        {
            _path = path;
            StartCoroutine(MoveCoroutine());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            Vector3 initPos = transform.position;
            Vector3 destination = _path[PathIndex];
            float Distance = Vector3.Distance(initPos, destination) / Speed;
            float timer = 0f;
            // Look At destination
            transform.LookAt(destination);
            while (Vector3.Distance(transform.position, destination) > 0.05f)
            {
                transform.position = Vector3.Lerp(initPos, destination, timer / (Distance * 5));
                timer++;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            PathIndex++;
            if (PathIndex >= _path.Count) PathIndex = 0;
            if (_biome != (int)Biome.Desert && _biome != (int)Biome.Snow)
            {
                _animator.SetBool("Static", true);
                yield return new WaitForSeconds(1f);
                _animator.SetBool("Static", false);
            }
        }

        yield return null;
    }
}
