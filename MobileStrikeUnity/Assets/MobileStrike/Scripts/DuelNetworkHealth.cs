using Unity.Netcode;
using UnityEngine;

public class DuelNetworkHealth : NetworkBehaviour
{
    public float maxHealth = 100f;
    public NetworkVariable<float> Health = new NetworkVariable<float>(
        100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> IsEliminated = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Health.Value = maxHealth;
            IsEliminated.Value = false;
        }
    }

    // Call this from a server-validated hit check, not directly from an untrusted client.
    public void ApplyServerDamage(float amount, ulong attackerClientId)
    {
        if (!IsServer || IsEliminated.Value || amount <= 0f) return;
        Health.Value = Mathf.Max(0f, Health.Value - amount);
        if (Health.Value <= 0f)
        {
            IsEliminated.Value = true;
            DuelRoundManager.Instance?.ReportElimination(OwnerClientId, attackerClientId);
        }
    }
}
