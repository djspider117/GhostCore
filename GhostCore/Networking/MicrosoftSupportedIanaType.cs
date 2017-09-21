using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    /// <summary>
    /// Extracted from https://msdn.microsoft.com/en-us/library/windows/apps/windows.networking.connectivity.networkadapter.ianainterfacetype
    /// Can be used in conjunction with IANAifType enumereation
    /// </summary>
    public enum MicrosoftSupportedIanaType : uint
    {
        /// <summary>
        /// Some other type of network interface.
        /// </summary>
        Other = 1,
        /// <summary>
        /// An Ethernet network interface
        /// </summary>
        Ethernet = 6,
        /// <summary>
        /// An token ring network interface.
        /// </summary>
        TokenRingNetwork = 9,
        /// <summary>
        /// A PPP network interface
        /// </summary>
        PPPNetwork = 23,
        /// <summary>
        /// A software loopback network interface
        /// </summary>
        SoftwareLoopbackNetwork = 24,
        /// <summary>
        /// An ATM network interface.
        /// </summary>
        ATMNetwork = 37,
        /// <summary>
        /// An IEEE 802.11 wireless network interface.
        /// </summary>
        WirelessNetwork = 71,
        /// <summary>
        /// A tunnel type encapsulation network interface.
        /// </summary>
        TunnelEncapsultion = 131,
        /// <summary>
        /// An IEEE 1394 (Firewire) high performance serial bus network interface.
        /// </summary>
        Ieee1394Firewire = 144
    }
}
