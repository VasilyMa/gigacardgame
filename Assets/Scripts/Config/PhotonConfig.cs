using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

[CreateAssetMenu(fileName = "PhotonConfig", menuName = "Config/Photon")]
public class PhotonConfig : Config, IConnectionCallbacks
{
    private int maxConnectionAttempts = 3;
    private int currentAttempt = 0;
    private bool isConnected = false;

    public override IEnumerator Init()
    {
        currentAttempt = 0;
        isConnected = false;

        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;

        while (currentAttempt < maxConnectionAttempts && !isConnected)
        {
            currentAttempt++;
            Debug.Log($"Attempting to connect to Photon (Attempt {currentAttempt}/{maxConnectionAttempts})");

            PhotonNetwork.ConnectUsingSettings();

            // Ждем соединения или таймаута
            float timeout = 10f; // секунд на попытку
            float elapsed = 0f;

            while (!isConnected && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!isConnected)
            {
                Debug.LogWarning($"Connection attempt {currentAttempt} timed out");
                PhotonNetwork.Disconnect();
                yield return new WaitForSeconds(1f); // Пауза между попытками
            }
        }

        if (!isConnected)
        {
            Debug.LogError($"Failed to connect to Photon after {maxConnectionAttempts} attempts");
            // Здесь можно продолжить работу в оффлайн-режиме
        }
        else
        {
            Debug.Log("Successfully connected to Photon");
        }

        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnConnected()
    {
        isConnected = true;
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
        isConnected = true;
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.LogError($"Custom authentication failed: {debugMessage}");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected: {cause}");
        isConnected = false;
    }

    public void OnRegionListReceived(RegionHandler regionHandler) { }
}