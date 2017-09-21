using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    /// The encapsulation method used by a tunnel. The value
    /// direct indicates that a packet is encapsulated
    /// directly within a normal IP header, with no
    /// intermediate header, and unicast to the remote tunnel
    /// endpoint(e.g., an RFC 2003 IP-in-IP tunnel, or an RFC
    /// 1933 IPv6-in-IPv4 tunnel). The value minimal indicates
    /// that a Minimal Forwarding Header(RFC 2004) is
    /// inserted between the outer header and the payload
    /// packet. The value UDP indicates that the payload
    /// packet is encapsulated within a normal UDP packet
    /// (e.g., RFC 1234).

    /// The values sixToFour, sixOverFour, and isatap
    /// indicates that an IPv6 packet is encapsulated directly
    /// within an IPv4 header, with no intermediate header,
    /// and unicast to the destination determined by the 6to4,
    /// 6over4, or ISATAP protocol.

    /// The remaining protocol-specific values indicate that a
    /// header of the protocol of that name is inserted
    /// between the outer header and the payload header.

    /// The assignment policy for IANAtunnelType values is
    /// identical to the policy for assigning IANAifType
    /// values.
    public enum IANAtunnelType : uint
    {
        /// None Of The Following
        Other = 1,
        /// No Intermediate Header
        Direct = 2,
        /// GRE Encapsulation
        Gre = 3,
        /// Minimal Encapsulation
        Minimal = 4,
        /// L2TP Encapsulation
        L2tp = 5,
        /// PPTP Encapsulation
        Pptp = 6,
        /// L2F Encapsulation
        L2f = 7,
        /// UDP Encapsulation
        Udp = 8,
        /// ATMP Encapsulation
        Atmp = 9,
        /// MSDP Encapsulation
        Msdp = 10,
        /// 6To4 Encapsulation
        SixToFour = 11,
        /// 6Over4 Encapsulation
        SixOverFour = 12,
        /// ISATAP Encapsulation
        Isatap = 13,
        /// Teredo Encapsulation
        /// Iphttps(15)       IPHTTPS
        /// Iphttps(15)       IPHTTPS
        Teredo = 14
    }
}
