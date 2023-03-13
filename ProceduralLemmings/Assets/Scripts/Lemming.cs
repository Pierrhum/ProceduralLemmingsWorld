using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lemming : MonoBehaviour
{
    private Animator _animator;
    private List<Vector3> _path;
    private int PathIndex = 0;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetBiome(int biome, List<Vector3> path=null)
    {
        _animator.SetInteger("Biome", biome);
        _animator.SetBool("Static", path==null);
        if (path != null)
        {
            _path = path;
            //StartCoroutine(MoveCoroutine());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        Vector3 initPos = transform.position;
        Vector3 destination = _path[PathIndex];
        float timer = 0f;
        while (transform.position != destination)
        {
            //transform.position = Vector3.Lerp(initPos, destination,);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        PathIndex++;
        yield return StartCoroutine(MoveCoroutine());
    }
}
