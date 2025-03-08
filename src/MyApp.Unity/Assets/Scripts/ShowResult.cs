using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowResult : MonoBehaviour
{
    [SerializeField] Text _resultText;
    [SerializeField] SampleScene _client;
    // Start is called before the first frame update
    void Start()
    {
        _client.OnResult += result =>
        {
            _resultText.text = result.ToString();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
