using System.Collections.Generic;
using UnityEngine;
namespace WebGLRescueArena { public sealed class SimpleObjectPool : MonoBehaviour { [SerializeField] private GameObject prefab; [SerializeField] private int initialSize = 16; private readonly Queue<GameObject> available = new Queue<GameObject>(); private void Awake() { for (int i = 0; i < initialSize; i++) Return(Instantiate(prefab)); } public GameObject Take() => available.Count == 0 ? Instantiate(prefab) : available.Dequeue(); public void Return(GameObject item) { item.SetActive(false); available.Enqueue(item); } } }
