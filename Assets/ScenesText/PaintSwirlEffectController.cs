using UnityEngine;

public class PaintSwirlEffectController : MonoBehaviour
{
    public Material paintSwirlMaterial;

    public float spinSpeed = 1.0f;    // SpinTime 增长速度
    public float timeSpeed = 1.0f;    // TimeParam 增长速度

    private float currentSpinTime = 0f;
    private float currentTimeParam = 0f;

    void Update()
    {
        if (paintSwirlMaterial == null)
            return;

        // 增加时间
        currentSpinTime += Time.deltaTime * spinSpeed;
        currentTimeParam += Time.deltaTime * timeSpeed;

        // 给 Shader 赋值
        paintSwirlMaterial.SetFloat("_SpinTime", currentSpinTime);
        paintSwirlMaterial.SetFloat("_TimeParam", currentTimeParam);
    }
}
