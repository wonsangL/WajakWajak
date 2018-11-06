using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wajak : MonoBehaviour {
    public Sprite[] wajaks;
    private Image img;

    private void Start()
    {
        img = transform.gameObject.GetComponent<Image>();
        StartCoroutine(Wak());
    }

    IEnumerator Wak()
    {
        while (true)
        {
            int index = (int)(Random.value * 13);
            img.sprite = wajaks[index];
            yield return new WaitForSeconds(1);
        }
    }
}
