using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(30.0f, 360.0f, 0.0f), 1.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}
