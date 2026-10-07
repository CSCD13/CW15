using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombos : MonoBehaviour {
    private List<KeyCode> inputBuffer = new List<KeyCode>();
    private float inputTimer = 0.5f;

    private Coroutine timeoutCoroutine;

    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource audioSource;

    private KeyCode[][] combos = new KeyCode[][]
    {
        new KeyCode[] { KeyCode.W, KeyCode.W, KeyCode.S, KeyCode.S, KeyCode.Q, KeyCode.A },
        new KeyCode[] { KeyCode.W, KeyCode.W, KeyCode.W, KeyCode.S, KeyCode.Q, KeyCode.A },
        new KeyCode[] { KeyCode.W, KeyCode.W, KeyCode.W }
    };
    private float movementForce = 5f;

    [SerializeField] AudioClip[] soundEffects = new AudioClip[3];

    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            RegisterInput(KeyCode.W);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            RegisterInput(KeyCode.S);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            RegisterInput(KeyCode.Q);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            RegisterInput(KeyCode.A);
        }
    }

    private void RegisterInput(KeyCode key) {
        inputBuffer.Add(key);

        if (timeoutCoroutine != null) {
            StopCoroutine(timeoutCoroutine);
        }

        timeoutCoroutine = StartCoroutine(ManageInputs());
    }

    IEnumerator ManageInputs() {
        yield return new WaitForSeconds(inputTimer);

        executeCombo();
        inputBuffer.Clear();
    }

    private void executeCombo() {
        int comboIdentifier = 0;

        foreach (var combo in combos) {
            if (inputBuffer.Count == combo.Length) {
                int i = 0;
                bool correctCombo = true;

                foreach (var comboInp in combo) {
                    if (comboInp != inputBuffer[i]) {
                        correctCombo = false;
                        break; 
                    }
                    i++;
                }

                if (correctCombo) {
                    switch (comboIdentifier) {
                        case 0:
                            rb.AddForce(new Vector3(movementForce, movementForce, 0), ForceMode.Impulse);
                            audioSource.clip = soundEffects[0];
                            audioSource.Play();
                            break;
                        case 1:
                            rb.AddForce(new Vector3(0, movementForce, 0), ForceMode.Impulse);
                            audioSource.clip = soundEffects[1];
                            audioSource.Play();
                            break;
                        case 2:
                            rb.AddForce(new Vector3(-movementForce, movementForce, 0), ForceMode.Impulse);
                            audioSource.clip = soundEffects[2];
                            audioSource.Play();
                            break;
                    }
                    return; 
                }
            }
            comboIdentifier++;
        }
    }
}