using UnityEngine;

public class HelloClient : MonoBehaviour
{
    private HelloRequester _helloRequester;

    private void Start()
    {
        _helloRequester = new HelloRequester();
        _helloRequester.Start();
    }

    public void SendMessage(string message)
    {
        _helloRequester.SendMessage(message);

    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }
}