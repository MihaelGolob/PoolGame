using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour {
    private IEnumerator Start() {
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().SetTrigger("Exit");
    }
}