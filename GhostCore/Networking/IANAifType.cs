using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    /// <summary>
    /// This data type is used as the syntax of the ifType
    /// object in the(updated) definition of MIB-II's ifTable.
    /// The definition of this textual convention with the
    /// addition of newly assigned values is published
    /// periodically by the IANA, in either the Assigned
    /// Numbers RFC, or some derivative of it specific to
    /// Internet Network Management number assignments.  (The
    /// latest arrangements can be obtained by contacting the IANA.)
    /// Requests for new values should be made to IANA via
    /// email(iana&iana.org).
    ///
    /// The relationship between the assignment of ifType
    /// values and of OIDs to particular media-specific MIBs
    /// is solely the purview of IANA and is subject to change
    /// without notice.  Quite often, a media-specific MIB's
    /// OID-subtree assignment within MIB-II's 'transmission'
    /// subtree will be the same as its ifType value.
    /// However, in some circumstances this will not be the
    /// case, and implementors must not pre-assume any
    /// specific relationship between ifType values and
    /// transmission subtree OIDs.
    /// 
    /// LAST-UPDATED "201409240000Z" -- September 24, 2014
    /// 
    /// Original docs found at: http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib
    /// </summary>
    public enum IANAifType : uint
    {
        /// None Of The Following
        Other = 1,
        Regular1822 = 2,
        Hdh1822 = 3,
        DdnX25 = 4,
        Rfc877x25 = 5,
        /// For All Ethernet-Like Interfaces,
        EthernetCsmacd = 6,
        /// Regardless of speed
        /// Deprecated Via https://tools.ietf.org/html/RFC3635
        /// Ethernetcsmacd (6) Should Be Used Instead
        Iso88023Csmacd = 7,
        Iso88024TokenBus = 8,
        Iso88025TokenRing = 9,
        Iso88026Man = 10,
        /// Deprecated Via https://tools.ietf.org/html/RFC3635
        /// Ethernetcsmacd (6) Should Be Used Instead
        StarLan = 11,
        Proteon10Mbit = 12,
        Proteon80Mbit = 13,
        Hyperchannel = 14,
        Fddi = 15,
        Lapb = 16,
        Sdlc = 17,
        /// DS1-MIB
        Ds1 = 18,
        /// Obsolete See DS1-MIB
        E1 = 19,
        /// No Longer Used
        /// See Also https://tools.ietf.org/html/RFC2127
        BasicISDN = 20,
        /// No Longer Used
        /// See Also https://tools.ietf.org/html/RFC2127
        PrimaryISDN = 21,
        /// Proprietary Serial
        PropPointToPointSerial = 22,
        Ppp = 23,
        SoftwareLoopback = 24,
        /// CLNP Over IP 
        Eon = 25,
        Ethernet3Mbit = 26,
        /// XNS Over IP
        Nsip = 27,
        /// Generic SLIP
        Slip = 28,
        /// ULTRA Technologies
        Ultra = 29,
        /// DS3-MIB
        Ds3 = 30,
        /// SMDS, Coffee
        Sip = 31,
        /// DTE Only. 
        FrameRelay = 32,
        Rs232 = 33,
        /// Parallel-Port
        Para = 34,
        /// Arcnet
        Arcnet = 35,
        /// Arcnet Plus
        ArcnetPlus = 36,
        /// ATM Cells
        Atm = 37,
        Miox25 = 38,
        /// SONET Or SDH 
        Sonet = 39,
        X25ple = 40,
        Iso88022llc = 41,
        LocalTalk = 42,
        SmdsDxi = 43,
        /// FRNETSERV-MIB
        FrameRelayService = 44,
        V35 = 45,
        Hssi = 46,
        Hippi = 47,
        /// Generic Modem
        Modem = 48,
        /// AAL5 Over ATM
        Aal5 = 49,
        SonetPath = 50,
        SonetVT = 51,
        /// SMDS Intercarrier Interface
        SmdsIcip = 52,
        /// Proprietary Virtual/Internal
        PropVirtual = 53,
        /// Proprietary Multiplexing
        PropMultiplexor = 54,
        /// 100Basevg
        Ieee80212 = 55,
        /// Fibre Channel
        FibreChannel = 56,
        /// HIPPI Interfaces     
        HippiInterface = 57,
        /// Obsolete, Use Either
        /// Framerelay(32) Or 
        /// Framerelayservice(44).
        FrameRelayInterconnect = 58,
        /// ATM Emulated LAN For 802.3
        Aflane8023 = 59,
        /// ATM Emulated LAN For 802.5
        Aflane8025 = 60,
        /// ATM Emulated Circuit          
        CctEmul = 61,
        /// Obsoleted Via https://tools.ietf.org/html/RFC3635
        /// Ethernetcsmacd (6) Should Be Used Instead
        FastEther = 62,
        /// ISDN And X.25           
        Isdn = 63,
        /// CCITT V.11/X.21             
        V11 = 64,
        /// CCITT V.36                  
        V36 = 65,
        /// CCITT G703 At 64Kbps
        G703at64k = 66,
        /// Obsolete See DS1-MIB
        G703at2mb = 67,
        /// SNA QLLC                    
        Qllc = 68,
        /// Obsoleted Via https://tools.ietf.org/html/RFC3635
        /// Ethernetcsmacd (6) Should Be Used Instead
        FastEtherFX = 69,
        /// Channel                     
        Channel = 70,
        /// Radio Spread Spectrum       
        Ieee80211 = 71,
        /// IBM System 360/370 OEMI Channel
        Ibm370parChan = 72,
        /// IBM Enterprise Systems Connection
        Escon = 73,
        /// Data Link Switching
        Dlsw = 74,
        /// ISDN S/T Interface
        Isdns = 75,
        /// ISDN U Interface
        Isdnu = 76,
        /// Link Access Protocol D
        Lapd = 77,
        /// IP Switching Objects
        IpSwitch = 78,
        /// Remote Source Route Bridging
        Rsrb = 79,
        /// ATM Logical Port
        AtmLogical = 80,
        /// Digital Signal Level 0
        Ds0 = 81,
        /// Group Of Ds0s On The Same Ds1
        Ds0Bundle = 82,
        /// Bisynchronous Protocol
        Bsc = 83,
        /// Asynchronous Protocol
        Async = 84,
        /// Combat Net Radio
        Cnr = 85,
        /// ISO 802.5R DTR
        Iso88025Dtr = 86,
        /// Ext Pos Loc Report Sys
        Eplrs = 87,
        /// Appletalk Remote Access Protocol
        Arap = 88,
        /// Proprietary Connectionless Protocol
        PropCnls = 89,
        /// CCITT-ITU X.29 PAD Protocol
        HostPad = 90,
        /// CCITT-ITU X.3 PAD Facility
        TermPad = 91,
        /// Multiproto Interconnect Over FR
        FrameRelayMPI = 92,
        /// CCITT-ITU X213
        X213 = 93,
        /// Asymmetric Digital Subscriber Loop
        Adsl = 94,
        /// Rate-Adapt. Digital Subscriber Loop
        Radsl = 95,
        /// Symmetric Digital Subscriber Loop
        Sdsl = 96,
        /// Very H-Speed Digital Subscrib. Loop
        Vdsl = 97,
        /// ISO 802.5 CRFP
        Iso88025CRFPInt = 98,
        /// Myricom Myrinet
        Myrinet = 99,
        /// Voice Receive And Transmit
        VoiceEM = 100,
        /// Voice Foreign Exchange Office
        VoiceFXO = 101,
        /// Voice Foreign Exchange Station
        VoiceFXS = 102,
        /// Voice Encapsulation
        VoiceEncap = 103,
        /// Voice Over IP Encapsulation
        VoiceOverIp = 104,
        /// ATM DXI
        AtmDxi = 105,
        /// ATM FUNI
        AtmFuni = 106,
        /// ATM IMA		   
        AtmIma = 107,
        /// PPP Multilink Bundle
        PppMultilinkBundle = 108,
        /// IBM Ipovercdlc
        IpOverCdlc = 109,
        /// IBM Common Link Access To Workstn
        IpOverClaw = 110,
        /// IBM Stacktostack
        StackToStack = 111,
        /// IBM VIPA
        VirtualIpAddress = 112,
        /// IBM Multi-Protocol Channel Support
        Mpc = 113,
        /// IBM Ipoveratm
        IpOverAtm = 114,
        /// ISO 802.5J Fiber Token Ring
        Iso88025Fiber = 115,
        /// IBM Twinaxial Data Link Control
        Tdlc = 116,
        /// Obsoleted Via https://tools.ietf.org/html/RFC3635
        /// Ethernetcsmacd (6) Should Be Used Instead
        GigabitEthernet = 117,
        /// HDLC
        Hdlc = 118,
        /// LAP F
        Lapf = 119,
        /// V.37
        V37 = 120,
        /// Multi-Link Protocol
        X25mlp = 121,
        /// X25 Hunt Group
        X25huntGroup = 122,
        /// Transp HDLC
        TranspHdlc = 123,
        /// Interleave Channel
        Interleave = 124,
        /// Fast Channel
        Fast = 125,
        /// IP (For APPN HPR In IP Networks)
        Ip = 126,
        /// CATV Mac Layer
        DocsCableMaclayer = 127,
        /// CATV Downstream Interface
        DocsCableDownstream = 128,
        /// CATV Upstream Interface
        DocsCableUpstream = 129,
        /// Avalon Parallel Processor
        A12MppSwitch = 130,
        /// Encapsulation Interface
        Tunnel = 131,
        /// Coffee Pot
        Coffee = 132,
        /// Circuit Emulation Service
        Ces = 133,
        /// ATM Sub Interface
        AtmSubInterface = 134,
        /// Layer 2 Virtual LAN Using 802.1Q
        L2vlan = 135,
        /// Layer 3 Virtual LAN Using IP
        L3ipvlan = 136,
        /// Layer 3 Virtual LAN Using IPX
        L3ipxvlan = 137,
        /// IP Over Power Lines	
        DigitalPowerline = 138,
        /// Multimedia Mail Over IP
        MediaMailOverIp = 139,
        /// Dynamic Syncronous Transfer Mode
        Dtm = 140,
        /// Data Communications Network
        Dcn = 141,
        /// IP Forwarding Interface
        IpForward = 142,
        /// Multi-Rate Symmetric DSL
        Msdsl = 143,
        /// IEEE1394 High Performance Serial Bus
        Ieee1394 = 144,
        /// HIPPI-6400 
        If_Gsn = 145,
        /// DVB-RCC MAC Layer
        DvbRccMacLayer = 146,
        /// DVB-RCC Downstream Channel
        DvbRccDownstream = 147,
        /// DVB-RCC Upstream Channel
        DvbRccUpstream = 148,
        /// ATM Virtual Interface
        AtmVirtual = 149,
        /// MPLS Tunnel Virtual Interface
        MplsTunnel = 150,
        /// Spatial Reuse Protocol	
        Srp = 151,
        /// Voice Over ATM
        VoiceOverAtm = 152,
        /// Voice Over Frame Relay 
        VoiceOverFrameRelay = 153,
        /// Digital Subscriber Loop Over ISDN
        Idsl = 154,
        /// Avici Composite Link Interface
        CompositeLink = 155,
        /// SS7 Signaling Link 
        Ss7SigLink = 156,
        /// Prop. P2P Wireless Interface
        PropWirelessP2P = 157,
        /// Frame Forward Interface
        FrForward = 158,
        /// Multiprotocol Over ATM AAL5
        Rfc1483 = 159,
        /// USB Interface
        Usb = 160,
        /// IEEE 802.3Ad Link Aggregate
        Ieee8023adLag = 161,
        /// BGP Policy Accounting
        Bgppolicyaccounting = 162,
        /// FRF .16 Multilink Frame Relay 
        Frf16MfrBundle = 163,
        /// H323 Gatekeeper
        H323Gatekeeper = 164,
        /// H323 Voice And Video Proxy
        H323Proxy = 165,
        /// MPLS                   
        Mpls = 166,
        /// Multi-Frequency Signaling Link
        MfSigLink = 167,
        /// High Bit-Rate DSL - 2Nd Generation
        Hdsl2 = 168,
        /// Multirate HDSL2
        Shdsl = 169,
        /// Facility Data Link 4Kbps On A DS1
        Ds1FDL = 170,
        /// Packet Over SONET/SDH Interface
        Pos = 171,
        /// DVB-ASI Input
        DvbAsiIn = 172,
        /// DVB-ASI Output 
        DvbAsiOut = 173,
        /// Power Line Communtications
        Plc = 174,
        /// Non Facility Associated Signaling
        Nfas = 175,
        /// TR008
        Tr008 = 176,
        /// Remote Digital Terminal
        Gr303RDT = 177,
        /// Integrated Digital Terminal
        Gr303IDT = 178,
        /// ISUP
        Isup = 179,
        /// Cisco Proprietary Maclayer
        PropDocsWirelessMaclayer = 180,
        /// Cisco Proprietary Downstream
        PropDocsWirelessDownstream = 181,
        /// Cisco Proprietary Upstream
        PropDocsWirelessUpstream = 182,
        /// HIPERLAN Type 2 Radio Interface
        Hiperlan2 = 183,
        /// Propbroadbandwirelessaccesspt2multipt
        /// Use Of This Iftype For IEEE 802.16 WMAN
        /// Interfaces As Per IEEE Std 802.16F Is
        /// Deprecated And Iftype 237 Should Be Used Instead.
        PropBWAp2Mp = 184,
        /// SONET Overhead Channel
        SonetOverheadChannel = 185,
        /// Digital Wrapper
        DigitalWrapperOverheadChannel = 186,
        /// ATM Adaptation Layer 2
        Aal2 = 187,
        /// MAC Layer Over Radio Links
        RadioMAC = 188,
        /// ATM Over Radio Links   
        AtmRadio = 189,
        /// Inter Machine Trunks
        Imt = 190,
        /// Multiple Virtual Lines DSL
        Mvl = 191,
        /// Long Reach DSL
        ReachDSL = 192,
        /// Frame Relay DLCI End Point
        FrDlciEndPt = 193,
        /// ATM VCI End Point
        AtmVciEndPt = 194,
        /// Optical Channel
        OpticalChannel = 195,
        /// Optical Transport
        OpticalTransport = 196,
        /// Proprietary ATM       
        PropAtm = 197,
        /// Voice Over Cable Interface
        VoiceOverCable = 198,
        /// Infiniband
        Infiniband = 199,
        /// TE Link
        TeLink = 200,
        /// Q.2931
        Q2931 = 201,
        /// Virtual Trunk Group
        VirtualTg = 202,
        /// SIP Trunk Group
        SipTg = 203,
        /// SIP Signaling   
        SipSig = 204,
        /// CATV Upstream Channel
        DocsCableUpstreamChannel = 205,
        /// Acorn Econet
        Econet = 206,
        /// FSAN 155Mb Symetrical PON Interface
        Pon155 = 207,
        /// Fsan622mb Symetrical PON Interface
        Pon622 = 208,
        /// Transparent Bridge Interface
        Bridge = 209,
        /// Interface Common To Multiple Lines   
        Linegroup = 210,
        /// Voice E&M Feature Group D
        VoiceEMFGD = 211,
        /// Voice FGD Exchange Access North American
        VoiceFGDEANA = 212,
        /// Voice Direct Inward Dialing
        VoiceDID = 213,
        /// MPEG Transport Interface
        MpegTransport = 214,
        /// 6To4 Interface (DEPRECATED)
        SixToFour = 215,
        /// GTP (GPRS Tunneling Protocol)
        Gtp = 216,
        /// Paradyne Etherloop 1
        PdnEtherLoop1 = 217,
        /// Paradyne Etherloop 2
        PdnEtherLoop2 = 218,
        /// Optical Channel Group
        OpticalChannelGroup = 219,
        /// Homepna ITU-T G.989
        Homepna = 220,
        /// Generic Framing Procedure (GFP)
        Gfp = 221,
        /// Layer 2 Virtual LAN Using Cisco ISL
        CiscoISLvlan = 222,
        /// Acteleis Proprietary Metaloop High Speed Link 
        ActelisMetaLOOP = 223,
        /// FCIP Link 
        FcipLink = 224,
        /// Resilient Packet Ring Interface Type
        Rpr = 225,
        /// RF Qam Interface
        Qam = 226,
        /// Link Management Protocol
        Lmp = 227,
        /// Cambridge Broadband Networks Limited Vectastar
        CblVectaStar = 228,
        /// CATV Modular CMTS Downstream Interface
        DocsCableMCmtsDownstream = 229,
        /// Asymmetric Digital Subscriber Loop Version 2 
        /// (DEPRECATED/OBSOLETED - Please Use Adsl2plus 238 Instead)
        Adsl2 = 230,
        /// Macseccontrolled 
        MacSecControlledIF = 231,
        /// Macsecuncontrolled
        MacSecUncontrolledIF = 232,
        /// Avici Optical Ethernet Aggregate
        AviciOpticalEther = 233,
        /// Atmbond
        Atmbond = 234,
        /// Voice FGD Operator Services
        VoiceFGDOS = 235,
        /// Multimedia Over Coax Alliance (Moca) Interface
        /// As Documented In Information Provided Privately To IANA
        MocaVersion1 = 236,
        /// IEEE 802.16 WMAN Interface
        Ieee80216WMAN = 237,
        /// Asymmetric Digital Subscriber Loop Version 2, 
        /// Version 2 Plus And All Variants
        Adsl2plus = 238,
        /// DVB-RCS MAC Layer
        DvbRcsMacLayer = 239,
        /// DVB Satellite TDM
        DvbTdm = 240,
        /// DVB-RCS TDMA
        DvbRcsTdma = 241,
        /// LAPS Based On ITU-T X.86/Y.1323
        X86Laps = 242,
        /// 3GPP WWAN
        WwanPP = 243,
        /// 3GPP2 WWAN
        WwanPP2 = 244,
        /// Voice P-Phone EBS Physical Interface
        VoiceEBS = 245,
        /// Pseudowire Interface Type
        IfPwType = 246,
        /// Internal LAN On A Bridge Per IEEE 802.1Ap
        Ilan = 247,
        /// Provider Instance Port On A Bridge Per IEEE 802.1Ah PBB
        Pip = 248,
        /// Alcatel-Lucent Ethernet Link Protection
        AluELP = 249,
        /// Gigabit-Capable Passive Optical Networks (G-PON) As Per ITU-T G.948
        Gpon = 250,
        /// Very High Speed Digital Subscriber Line Version 2 (As Per ITU-T Recommendation G.993.2)
        Vdsl2 = 251,
        /// WLAN Profile Interface
        CapwapDot11Profile = 252,
        /// WLAN BSS Interface
        CapwapDot11Bss = 253,
        /// WTP Virtual Radio Interface
        CapwapWtpVirtualRadio = 254,
        /// Bitsport
        Bits = 255,
        /// DOCSIS CATV Upstream RF Port
        DocsCableUpstreamRfPort = 256,
        /// CATV Downstream RF Port
        CableDownstreamRfPort = 257,
        /// Vmware Virtual Network Interface
        VmwareVirtualNic = 258,
        /// IEEE 802.15.4 WPAN Interface
        Ieee802154 = 259,
        /// OTN Optical Data Unit
        OtnOdu = 260,
        /// OTN Optical Channel Transport Unit
        OtnOtu = 261,
        /// VPLS Forwarding Instance Interface Type
        IfVfiType = 262,
        /// G.998.1 Bonded Interface
        G9981 = 263,
        /// G.998.2 Bonded Interface
        G9982 = 264,
        /// G.998.3 Bonded Interface
        G9983 = 265,
        /// Ethernet Passive Optical Networks (E-PON)
        AluEpon = 266,
        /// EPON Optical Network Unit
        AluEponOnu = 267,
        /// EPON Physical User To Network Interface
        AluEponPhysicalUni = 268,
        /// The Emulation Of A Point-To-Point Link Over The EPON Layer
        AluEponLogicalLink = 269,
        /// GPON Optical Network Unit
        AluGponOnu = 270,
        /// GPON Physical User To Network Interface
        AluGponPhysicalUni = 271,
        /// Vmware NIC Team
        VmwareNicTeam = 272,
        /// CATV Downstream OFDM Interface
        DocsOfdmDownstream = 277,
        /// CATV Upstream OFDMA Interface
        DocsOfdmaUpstream = 278,
        /// G.Fast Port
        /// Sdci (280)  SDCI (IO-Link)
        Gfast = 279
    }
}
