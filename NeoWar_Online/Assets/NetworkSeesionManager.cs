using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkSeesionManager : NetworkManager
{
    public static NetworkSeesionManager Instance => singleton as NetworkSeesionManager;

    public bool IsServer => (mode == NetworkManagerMode.Host || mode == NetworkManagerMode.ServerOnly);
    public bool IsClient => (mode == NetworkManagerMode.Host || mode == NetworkManagerMode.ClientOnly);
}
