using Unity.Netcode;
using UnityEngine;

public class DuelRoundManager : NetworkBehaviour
{
    public static DuelRoundManager Instance { get; private set; }

    public NetworkVariable<ulong> WinnerClientId = new NetworkVariable<ulong>(
        ulong.MaxValue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> RoundFinished = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            WinnerClientId.Value = ulong.MaxValue;
            RoundFinished.Value = false;
        }
    }

    public void ReportElimination(ulong eliminatedClientId, ulong attackerClientId)
    {
        if (!IsServer || RoundFinished.Value) return;
        if (attackerClientId == eliminatedClientId || attackerClientId == ulong.MaxValue) return;
        WinnerClientId.Value = attackerClientId;
        RoundFinished.Value = true;
        ShowRoundResultClientRpc(attackerClientId, eliminatedClientId);
    }

    [ClientRpc]
    private void ShowRoundResultClientRpc(ulong winnerClientId, ulong loserClientId)
    {
        string result = NetworkManager.Singleton != null &&
                        NetworkManager.Singleton.LocalClientId == winnerClientId
            ? "YOU WIN!"
            : NetworkManager.Singleton != null &&
              NetworkManager.Singleton.LocalClientId == loserClientId
                ? "YOU LOSE!"
                : "ROUND FINISHED";
        Debug.Log(result);
    }
}
