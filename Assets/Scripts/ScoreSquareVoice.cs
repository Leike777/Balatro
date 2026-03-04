using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // 确保对象上有AudioSource组件
public class ScoreSquareVoice : MonoBehaviour
{
    [SerializeField] private AudioClip spawnSound; // 拖入要播放的音效文件
    [SerializeField] [Range(0, 1)] private float volume = 1f; // 音量控制
    //[SerializeField] private bool playOnAwake = true; // 是否在创建时立即播放

    private AudioSource audioSource;

    private void Awake()
    {
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        
        // 初始化AudioSource设置
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = volume;
        audioSource.clip = spawnSound;

        // if (playOnAwake && spawnSound != null)
        // {
        //     PlaySpawnSound();
        // }
        PlaySpawnSound();
    }

    // 公开方法可供其他脚本调用
    public void PlaySpawnSound()
    {
        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound, volume);
        }
        else
        {
            Debug.LogWarning("未分配生成音效！", this);
        }
    }
}
