using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class RiverSound : MonoBehaviour
{
    [Header("Spline")]
    public SplineContainer splineContainer;

    [Header("Audio")]
    public AudioClip streamClip;
    public int audioPointCount;
    public float minDistance = 3f;
    public float maxDistance = 25f;
    public float maxVolumn =  1.0f;
    public float fadeSpeed = 3f;

    [Header("Player")]
    public Transform player;

    private AudioSource[] audioSources;

    private void Start()
    {
        audioSources = new AudioSource[audioPointCount];

        for (int i = 0; i < audioPointCount; i++)
        {
            // chia deu cac diem doc spline
            float t = (float)i / (audioPointCount - 1);

            //  Tao obj tu vi tri do
            float3 pos = splineContainer.EvaluatePosition(t);
            Vector3 worldpos = new Vector3(pos.x, pos.y, pos.z);

            GameObject audioObj = new GameObject($"StreamAudio_{i}");
            audioObj.transform.position = worldpos; 
            audioObj.transform.parent = this.transform;

            AudioSource src = audioObj.AddComponent<AudioSource>();
            src.clip = streamClip;
            src.loop = true;
            src.spatialBlend = 0f;
            src.volume = 0f;    
            src.Play();

            audioSources[i] = src;
        }
    }

    private void Update()
    {
        if (player == null) return;

        foreach(AudioSource src in audioSources)
        {
            float distance = Vector3.Distance(player.position, src.transform.position);
            
            float targetVolumn = Mathf.InverseLerp(maxDistance, minDistance, distance);

            src.volume = Mathf.Lerp(src.volume, targetVolumn, Time.deltaTime * fadeSpeed);
        }
    }

}
