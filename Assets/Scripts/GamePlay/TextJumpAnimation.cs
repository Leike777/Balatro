using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextJumpAnimation : MonoBehaviour
{
    [Header("跳跃设置")]
    public float jumpHeight = 10f;          // 跳跃高度
    public float jumpDuration = 0.5f;       // 单次跳跃持续时间
    public float delayBetweenChars = 0.1f;  // 字符间跳跃延迟
    public float delayBetweenLoops = 1f;    // 新增：循环之间的等待时间
    public AnimationCurve jumpCurve;        // 跳跃运动曲线

    private TextMeshProUGUI tmpText;
    private Coroutine animationCoroutine;
    private Vector3[][] originalVertices;   // 存储每个字符的原始顶点位置

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        
        // 设置默认跳跃曲线（抛物线）
        if(jumpCurve == null || jumpCurve.keys.Length == 0)
        {
            jumpCurve = new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1),  // 顶点
                new Keyframe(1, 0)
            );
        }
    }

    void OnEnable()
    {
        StartAnimation();
    }

    void OnDisable()
    {
        StopAnimation();
    }

    public void StartAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        
        animationCoroutine = StartCoroutine(AnimateCharacters());
    }

    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        ResetTextMesh();
    }

    private IEnumerator AnimateCharacters()
    {
        while (true)
        {
            // 等待文本布局完成
            yield return null;
            
            // 强制更新文本信息
            tmpText.ForceMeshUpdate();
            
            TMP_TextInfo textInfo = tmpText.textInfo;
            int characterCount = textInfo.characterCount;
            
            if (characterCount == 0) 
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // 存储原始顶点位置
            StoreOriginalVertices(textInfo);
            
            // 逐个字符动画
            for (int i = 0; i < characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) 
                    continue;
                
                // 单个字符跳跃动画
                yield return StartCoroutine(AnimateSingleCharacter(i, textInfo));
                
                // 字符间延迟
                yield return new WaitForSeconds(delayBetweenChars);
            }
            
            // 重置所有字符位置
            ResetTextMesh();
            
            // 新增：循环之间的等待时间
            yield return new WaitForSeconds(delayBetweenLoops);
        }
    }

    private void StoreOriginalVertices(TMP_TextInfo textInfo)
    {
        originalVertices = new Vector3[textInfo.characterCount][];
        
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            
            originalVertices[i] = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                originalVertices[i][j] = textInfo.meshInfo[materialIndex].vertices[vertexIndex + j];
            }
        }
    }

    private IEnumerator AnimateSingleCharacter(int charIndex, TMP_TextInfo textInfo)
    {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        if (!charInfo.isVisible) yield break;
        
        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / jumpDuration;
            float height = jumpCurve.Evaluate(progress) * jumpHeight;
            
            // 更新四个顶点位置
            for (int j = 0; j < 4; j++)
            {
                Vector3 originalPos = originalVertices[charIndex][j];
                textInfo.meshInfo[materialIndex].vertices[vertexIndex + j] = 
                    originalPos + new Vector3(0, height, 0);
            }
            
            // 更新网格
            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            yield return null;
        }
        
        // 确保回到原始位置
        for (int j = 0; j < 4; j++)
        {
            textInfo.meshInfo[materialIndex].vertices[vertexIndex + j] = originalVertices[charIndex][j];
        }
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    private void ResetTextMesh()
    {
        if (originalVertices == null) return;
        
        TMP_TextInfo textInfo = tmpText.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible || originalVertices[i] == null) continue;
            
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            
            for (int j = 0; j < 4; j++)
            {
                textInfo.meshInfo[materialIndex].vertices[vertexIndex + j] = originalVertices[i][j];
            }
        }
        
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    // 新增：动态调整参数的方法
    public void SetAnimationParameters(float height, float charDuration, float charDelay, float loopDelay)
    {
        jumpHeight = height;
        jumpDuration = charDuration;
        delayBetweenChars = charDelay;
        delayBetweenLoops = loopDelay;
    }
}
