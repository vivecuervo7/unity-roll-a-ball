using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupControlledDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] List<GameObject> doorOpenIndicators;
    [SerializeField] List<GameObject> doorClosedIndicators;
    private List<Pickup> pickups;
    private AudioSource audioSource;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip unlockDoorSound;

    private void Awake()
    {
        pickups = GetComponentsInChildren<Pickup>().ToList();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetDoorLocked(pickups.Count > 0, true);
    }

    void PickupRetrieved(Pickup pickup)
    {
        if (pickups.Contains(pickup))
        {
            pickups.Remove(pickup);
            SetDoorLocked(pickups.Count > 0);
            audioSource.PlayOneShot(pickupSound);
        }
    }

    private void SetDoorLocked(bool locked, bool forceUpdate = false)
    {
        if (forceUpdate || door.activeSelf != locked)
        {
            door.SetActive(locked);
            doorClosedIndicators.ForEach(i => i.SetActive(locked));
            doorOpenIndicators.ForEach(i => i.SetActive(!locked));

            if (!locked)
            {
                audioSource.PlayOneShot(unlockDoorSound);
            }
        }
    }
}
