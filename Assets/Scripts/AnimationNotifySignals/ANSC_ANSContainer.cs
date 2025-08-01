using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANSC_ANSContainer : MonoBehaviour
{
    [Serializable] public struct ANS_Container
    {
        public string signalName;
        public ANS_ANSBase ansBase;
    }

    public List<ANS_Container> ansContainers;

    Dictionary<string, ANS_ANSBase> ansDictionary = new Dictionary<string, ANS_ANSBase>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the dictionary with the signal names and their corresponding ANS_ANSBase objects
        foreach (var container in ansContainers)
        {
            if (!ansDictionary.ContainsKey(container.signalName))
            {
                ansDictionary.Add(container.signalName, container.ansBase);
            }
            else
            {
                Debug.LogWarning($"Signal name '{container.signalName}' already exists in the dictionary.");
            }
        }
    }
    
    void OnDestroy()
    {
        // Clean up the dictionary to prevent memory leaks
        ansDictionary.Clear();
    }

    public void OnAnimationNotifySignal(string param)
    {
        string[] parameters = param.Split(',');
        string name = parameters[0];
        string[] args = parameters.Length > 1 ? parameters[1..] : null;
        if (ansDictionary.TryGetValue(name, out ANS_ANSBase ansBase))
        {
            ansBase.OnAnimationNotifySignal?.Invoke(this, args);
        }
        else
        {
            Debug.LogWarning($"No ANS_ANSBase found for signal name '{name}'.");
        }
    }
}
