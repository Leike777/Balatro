using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr : MonoBehaviour
{
    public static MusicMgr Instance; // 单例实例
    
    public AudioSource audioSource;
    public AudioClip[] backgroundMusicClips;
    public float volume = 0.5f; // 默认音量
    
    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景时不销毁
        }
        else
        {
            Destroy(gameObject); // 避免重复创建
            return;
        }
        
        // 初始化AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // 循环播放
        audioSource.volume = volume;
        PlayMusic(0);
        // 场景加载回调
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     // 可以在这里根据场景切换不同音乐
    //     PlayMusic(0); // 播放第一个音乐
    // }
    
    // 播放指定音乐
    public void PlayMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < backgroundMusicClips.Length)
        {
            audioSource.clip = backgroundMusicClips[clipIndex];
            audioSource.Play();
        }
    }
    
    // 设置音量
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume); // 限制在0-1之间
        audioSource.volume = volume;
    }
    
    // 切换音乐时平滑过渡
    public void CrossfadeMusic(int newClipIndex, float fadeDuration = 1.0f)
    {
        StartCoroutine(CrossfadeCoroutine(newClipIndex, fadeDuration));
    }
    
    private IEnumerator CrossfadeCoroutine(int newClipIndex, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        
        // 淡出当前音乐
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t/fadeDuration);
            yield return null;
        }
        
        // 切换音乐
        PlayMusic(newClipIndex);
        
        // 淡入新音乐
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, volume, t/fadeDuration);
            yield return null;
        }
    }
}
