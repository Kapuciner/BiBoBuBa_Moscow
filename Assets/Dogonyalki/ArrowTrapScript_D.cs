using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapScript_D : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject arrow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerStepped() {
        StartCoroutine(ShootArrowCoroutine());
    }

    IEnumerator ShootArrowCoroutine(int arrowAmount = 3)
    {
        for(int i = 0; i < arrowAmount; i++) {
            Instantiate(arrow, transform);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
