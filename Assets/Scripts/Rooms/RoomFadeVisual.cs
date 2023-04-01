using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FadeOut : MonoBehaviour
{
     
    Volume volume;
     
    VolumeProfile volprofile;
    UnityEngine.Rendering.Universal.LiftGammaGain gamma;
     
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volprofile = volume.profile;
         
        volprofile.TryGet(out gamma);
     
        Debug.Log(gamma);
    }
     
    public void doFadeOut()
    {
        StartCoroutine(doFade(-1));
    }
     
    public void doFadeIn()
    {
        StartCoroutine(doFade(0.1f));
    }
     
    IEnumerator doFade(float i)
    {
        for (float a = 0; a <= 1; a += Time.deltaTime*5)
        {
            float b;
            if (i < 0)
            {
                b = i * a;
                if (b < -0.85f) { b = i; }
            }

            else
            {
                b = -1+a;
                 
                if (b > -0.05f) { b = 0;}
                Debug.Log(b);
            }
            gamma.gain.Override(new Vector4(0, 0, 0, b));
            yield return null;
        }
    }
}