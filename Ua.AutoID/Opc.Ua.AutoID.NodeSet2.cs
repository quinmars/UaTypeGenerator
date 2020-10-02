namespace Ua.AutoID
{
    // Ideally this should go into Workstation.UaClient
    public interface IOptionalFields
    {
        int OptionalFieldCount { get; }
        uint EncodingMask { get; }
    }
    
    /// <summary>
    /// AutoIdOperationStatusEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.1" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3013")]
    public enum AutoIdOperationStatusEnumeration
    {
        /// <summary>Successful operation</summary>
        SUCCESS = 0,
        /// <summary>The operation has not be executed in total.</summary>
        MISC_ERROR_TOTAL = 1,
        /// <summary>The operation has been executed only partial.</summary>
        MISC_ERROR_PARTIAL = 2,
        /// <summary>Password required</summary>
        PERMISSON_ERROR = 3,
        /// <summary>Password is wrong.</summary>
        PASSWORD_ERROR = 4,
        /// <summary>Memory region not available for the actual tag.</summary>
        REGION_NOT_FOUND_ERROR = 5,
        /// <summary>Operation not supported by the actual tag.</summary>
        OP_NOT_POSSIBLE_ERROR = 6,
        /// <summary>Addressed memory not available for the actual tag.</summary>
        OUT_OF_RANGE_ERROR = 7,
        /// <summary>The operation cannot be executed because no tag or code was inside the range of the AutoID Device or the tag or code has been moved out of the range during execution.</summary>
        NO_IDENTIFIER = 8,
        /// <summary>Multiple tags or codes have been selected, but the command can only be used with a single tag or code.</summary>
        MULTIPLE_IDENTIFIERS = 9,
        /// <summary>The tag or code exists and has a valid format, but there was a problem reading the data (e.g. still CRC error after maximum number of retries).</summary>
        READ_ERROR = 10,
        /// <summary>The (optical) code or plain text has too many failures and cannot be detected.</summary>
        DECODING_ERROR = 11,
        /// <summary>The code doesn’t match the given target value.</summary>
        MATCH_ERROR = 12,
        /// <summary>The code format is not supported by the AutoID Device.</summary>
        CODE_NOT_SUPPORTED = 13,
        /// <summary>The tag exists, but there was a problem writing the data.</summary>
        WRITE_ERROR = 14,
        /// <summary>The command or a parameter combination is not supported by the AutoID Device.</summary>
        NOT_SUPPORTED_BY_DEVICE = 15,
        /// <summary>The command or a parameter combination is not supported by the tag.</summary>
        NOT_SUPPORTED_BY_TAG = 16,
        /// <summary>The AutoID Device is in a state not ready to execute the command.</summary>
        DEVICE_NOT_READY = 17,
        /// <summary>The AutoID Device configuration is not valid.</summary>
        INVALID_CONFIGURATION = 18,
        /// <summary>This error indicates that there is a general error in the communication between the transponder and the reader.</summary>
        RF_COMMUNICATION_ERROR = 19,
        /// <summary>The AutoID Device has a hardware fault.</summary>
        DEVICE_FAULT = 20,
        /// <summary>The battery of the (active) tag is low.</summary>
        TAG_HAS_LOW_BATTERY = 21,
    }
    
    /// <summary>
    /// DeviceStatusEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.2" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3003")]
    public enum DeviceStatusEnumeration
    {
        Idle = 0,
        Error = 1,
        Scanning = 2,
        Busy = 3,
    }
    
    /// <summary>
    /// LocationTypeEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.3" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3009")]
    public enum LocationTypeEnumeration
    {
        NMEA = 0,
        LOCAL = 1,
        WGS84 = 2,
        NAME = 3,
    }
    
    /// <summary>
    /// RfidLockOperationEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.4" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3016")]
    public enum RfidLockOperationEnumeration
    {
        Lock = 0,
        Unlock = 1,
        PermanentLock = 2,
        PermanentUnlock = 3,
    }
    
    /// <summary>
    /// RfidLockRegionEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.5" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3015")]
    public enum RfidLockRegionEnumeration
    {
        Kill = 0,
        Access = 1,
        EPC = 2,
        TID = 3,
        User = 4,
    }
    
    /// <summary>
    /// RfidPasswordTypeEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.2.6" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3014")]
    public enum RfidPasswordTypeEnumeration
    {
        Access = 0,
        Kill = 1,
        Read = 2,
        Write = 3,
    }
    
    /// <summary>
    /// Result values of an AutoID Identifier access.
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.17" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5022")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5023")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3017")]
    public class AccessResult : Workstation.ServiceModel.Ua.Structure,
            IOptionalFields
    {
        public virtual int OptionalFieldCount => 3;
        public uint EncodingMask { get; protected set; }
        private string _codeType;
        public string CodeType
        {
            get => _codeType;
            set
            {
                uint flag = 1u << 0;
                
                _codeType = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private ScanData _identifier;
        public ScanData Identifier
        {
            get => _identifier;
            set
            {
                uint flag = 1u << 1;
                
                _identifier = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private System.DateTime? _timestamp;
        public System.DateTime? Timestamp
        {
            get => _timestamp;
            set
            {
                uint flag = 1u << 2;
                
                _timestamp = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt32("EncodingMask", EncodingMask);
            if (CodeType is {} opt0)
            {
                encoder.WriteString("CodeType", opt0);
            }
            if (Identifier is {} opt1)
            {
                encoder.WriteExtensionObject<ScanData>("Identifier", opt1);
            }
            if (Timestamp is {} opt2)
            {
                encoder.WriteDateTime("Timestamp", opt2);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            EncodingMask = decoder.ReadUInt32(null);
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            if ((EncodingMask & (1u << 0)) != 0)
            {
                CodeType = decoder.ReadString("CodeType");
            }
            else
            {
                CodeType = null;
            }
            
            if ((EncodingMask & (1u << 1)) != 0)
            {
                Identifier = decoder.ReadExtensionObject<ScanData>("Identifier");
            }
            else
            {
                Identifier = null;
            }
            
            if ((EncodingMask & (1u << 2)) != 0)
            {
                Timestamp = decoder.ReadDateTime("Timestamp");
            }
            else
            {
                Timestamp = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Additional result values of an Rfid Transponder access.
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.18" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5024")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5025")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3018")]
    public class RfidAccessResult : AccessResult
    {
        public override int OptionalFieldCount => base.OptionalFieldCount + 7;
        private string _codeTypeRWData;
        public string CodeTypeRWData
        {
            get => _codeTypeRWData;
            set
            {
                uint flag = 1u << (0 + base.OptionalFieldCount);
                
                _codeTypeRWData = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private ScanData _rWData;
        public ScanData RWData
        {
            get => _rWData;
            set
            {
                uint flag = 1u << (1 + base.OptionalFieldCount);
                
                _rWData = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private int? _antenna;
        public int? Antenna
        {
            get => _antenna;
            set
            {
                uint flag = 1u << (2 + base.OptionalFieldCount);
                
                _antenna = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private int? _currentPowerLevel;
        public int? CurrentPowerLevel
        {
            get => _currentPowerLevel;
            set
            {
                uint flag = 1u << (3 + base.OptionalFieldCount);
                
                _currentPowerLevel = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private ushort? _pC;
        public ushort? PC
        {
            get => _pC;
            set
            {
                uint flag = 1u << (4 + base.OptionalFieldCount);
                
                _pC = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private string _polarization;
        public string Polarization
        {
            get => _polarization;
            set
            {
                uint flag = 1u << (5 + base.OptionalFieldCount);
                
                _polarization = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private int? _strength;
        public int? Strength
        {
            get => _strength;
            set
            {
                uint flag = 1u << (6 + base.OptionalFieldCount);
                
                _strength = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            if (CodeTypeRWData is {} opt0)
            {
                encoder.WriteString("CodeTypeRWData", opt0);
            }
            if (RWData is {} opt1)
            {
                encoder.WriteExtensionObject<ScanData>("RWData", opt1);
            }
            if (Antenna is {} opt2)
            {
                encoder.WriteInt32("Antenna", opt2);
            }
            if (CurrentPowerLevel is {} opt3)
            {
                encoder.WriteInt32("CurrentPowerLevel", opt3);
            }
            if (PC is {} opt4)
            {
                encoder.WriteUInt16("PC", opt4);
            }
            if (Polarization is {} opt5)
            {
                encoder.WriteString("Polarization", opt5);
            }
            if (Strength is {} opt6)
            {
                encoder.WriteInt32("Strength", opt6);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            int offset = base.OptionalFieldCount;
            
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            if ((EncodingMask & (1u << (0 + offset))) != 0)
            {
                CodeTypeRWData = decoder.ReadString("CodeTypeRWData");
            }
            else
            {
                CodeTypeRWData = null;
            }
            
            if ((EncodingMask & (1u << (1 + offset))) != 0)
            {
                RWData = decoder.ReadExtensionObject<ScanData>("RWData");
            }
            else
            {
                RWData = null;
            }
            
            if ((EncodingMask & (1u << (2 + offset))) != 0)
            {
                Antenna = decoder.ReadInt32("Antenna");
            }
            else
            {
                Antenna = null;
            }
            
            if ((EncodingMask & (1u << (3 + offset))) != 0)
            {
                CurrentPowerLevel = decoder.ReadInt32("CurrentPowerLevel");
            }
            else
            {
                CurrentPowerLevel = null;
            }
            
            if ((EncodingMask & (1u << (4 + offset))) != 0)
            {
                PC = decoder.ReadUInt16("PC");
            }
            else
            {
                PC = null;
            }
            
            if ((EncodingMask & (1u << (5 + offset))) != 0)
            {
                Polarization = decoder.ReadString("Polarization");
            }
            else
            {
                Polarization = null;
            }
            
            if ((EncodingMask & (1u << (6 + offset))) != 0)
            {
                Strength = decoder.ReadInt32("Strength");
            }
            else
            {
                Strength = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for AntennaNameIdPair
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.3" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5017")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5018")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3011")]
    public class AntennaNameIdPair : Workstation.ServiceModel.Ua.Structure
    {
        public int AntennaId { get; set; }
        public string AntennaName { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteInt32("AntennaId", AntennaId);
            encoder.WriteString("AntennaName", AntennaName);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            AntennaId = decoder.ReadInt32("AntennaId");
            AntennaName = decoder.ReadString("AntennaName");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for DhcpGeoConfCoordinate
    /// </summary>
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5034")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5035")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3023")]
    public class DhcpGeoConfCoordinate : Workstation.ServiceModel.Ua.Structure
    {
        public byte LaRes { get; set; }
        public short LatitudeInteger { get; set; }
        public int LatitudeFraction { get; set; }
        public byte LoRes { get; set; }
        public short LongitudeInteger { get; set; }
        public int LongitudeFraction { get; set; }
        public byte AT { get; set; }
        public byte AltRes { get; set; }
        public int AltitudeInteger { get; set; }
        public short AltitudeFraction { get; set; }
        public byte Datum { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteByte("LaRes", LaRes);
            encoder.WriteInt16("LatitudeInteger", LatitudeInteger);
            encoder.WriteInt32("LatitudeFraction", LatitudeFraction);
            encoder.WriteByte("LoRes", LoRes);
            encoder.WriteInt16("LongitudeInteger", LongitudeInteger);
            encoder.WriteInt32("LongitudeFraction", LongitudeFraction);
            encoder.WriteByte("AT", AT);
            encoder.WriteByte("AltRes", AltRes);
            encoder.WriteInt32("AltitudeInteger", AltitudeInteger);
            encoder.WriteInt16("AltitudeFraction", AltitudeFraction);
            encoder.WriteByte("Datum", Datum);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            LaRes = decoder.ReadByte("LaRes");
            LatitudeInteger = decoder.ReadInt16("LatitudeInteger");
            LatitudeFraction = decoder.ReadInt32("LatitudeFraction");
            LoRes = decoder.ReadByte("LoRes");
            LongitudeInteger = decoder.ReadInt16("LongitudeInteger");
            LongitudeFraction = decoder.ReadInt32("LongitudeFraction");
            AT = decoder.ReadByte("AT");
            AltRes = decoder.ReadByte("AltRes");
            AltitudeInteger = decoder.ReadInt32("AltitudeInteger");
            AltitudeFraction = decoder.ReadInt16("AltitudeFraction");
            Datum = decoder.ReadByte("Datum");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for LocalCoordinate
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.4" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5028")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5029")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3019")]
    public class LocalCoordinate : Workstation.ServiceModel.Ua.Structure
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public System.DateTime Timestamp { get; set; }
        public double DilutionOfPrecision { get; set; }
        public int UsefulPrecision { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteDouble("X", X);
            encoder.WriteDouble("Y", Y);
            encoder.WriteDouble("Z", Z);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteDouble("DilutionOfPrecision", DilutionOfPrecision);
            encoder.WriteInt32("UsefulPrecision", UsefulPrecision);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            X = decoder.ReadDouble("X");
            Y = decoder.ReadDouble("Y");
            Z = decoder.ReadDouble("Z");
            Timestamp = decoder.ReadDateTime("Timestamp");
            DilutionOfPrecision = decoder.ReadDouble("DilutionOfPrecision");
            UsefulPrecision = decoder.ReadInt32("UsefulPrecision");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for Position
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.5" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5007")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5008")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3004")]
    public class Position : Workstation.ServiceModel.Ua.Structure
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int Rotation { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteInt32("PositionX", PositionX);
            encoder.WriteInt32("PositionY", PositionY);
            encoder.WriteInt32("SizeX", SizeX);
            encoder.WriteInt32("SizeY", SizeY);
            encoder.WriteInt32("Rotation", Rotation);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            PositionX = decoder.ReadInt32("PositionX");
            PositionY = decoder.ReadInt32("PositionY");
            SizeX = decoder.ReadInt32("SizeX");
            SizeY = decoder.ReadInt32("SizeY");
            Rotation = decoder.ReadInt32("Rotation");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for RfidSighting
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.13" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5009")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5010")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3006")]
    public class RfidSighting : Workstation.ServiceModel.Ua.Structure
    {
        public int Antenna { get; set; }
        public int Strength { get; set; }
        public System.DateTime Timestamp { get; set; }
        public int CurrentPowerLevel { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteInt32("Antenna", Antenna);
            encoder.WriteInt32("Strength", Strength);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteInt32("CurrentPowerLevel", CurrentPowerLevel);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            Antenna = decoder.ReadInt32("Antenna");
            Strength = decoder.ReadInt32("Strength");
            Timestamp = decoder.ReadDateTime("Timestamp");
            CurrentPowerLevel = decoder.ReadInt32("CurrentPowerLevel");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for Rotation
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.14" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5050")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5051")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3029")]
    public class Rotation : Workstation.ServiceModel.Ua.Structure
    {
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteDouble("Yaw", Yaw);
            encoder.WriteDouble("Pitch", Pitch);
            encoder.WriteDouble("Roll", Roll);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            Yaw = decoder.ReadDouble("Yaw");
            Pitch = decoder.ReadDouble("Pitch");
            Roll = decoder.ReadDouble("Roll");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for ScanDataEpc
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.6" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5036")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5037")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3024")]
    public class ScanDataEpc : Workstation.ServiceModel.Ua.Structure
    {
        public ushort PC { get; set; }
        public byte[] UId { get; set; }
        public ushort XPC_W1 { get; set; }
        public ushort XPC_W2 { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt16("PC", PC);
            encoder.WriteByteString("UId", UId);
            encoder.WriteUInt16("XPC_W1", XPC_W1);
            encoder.WriteUInt16("XPC_W2", XPC_W2);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            PC = decoder.ReadUInt16("PC");
            UId = decoder.ReadByteString("UId");
            XPC_W1 = decoder.ReadUInt16("XPC_W1");
            XPC_W2 = decoder.ReadUInt16("XPC_W2");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for ScanResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.8" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5002")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5003")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3001")]
    public abstract class ScanResult : Workstation.ServiceModel.Ua.Structure,
            IOptionalFields
    {
        public virtual int OptionalFieldCount => 1;
        public uint EncodingMask { get; protected set; }
        public string CodeType { get; set; }
        public ScanData ScanData { get; set; }
        public System.DateTime Timestamp { get; set; }
        private Location _location;
        public Location Location
        {
            get => _location;
            set
            {
                uint flag = 1u << 0;
                
                _location = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt32("EncodingMask", EncodingMask);
            encoder.WriteString("CodeType", CodeType);
            encoder.WriteExtensionObject<ScanData>("ScanData", ScanData);
            encoder.WriteDateTime("Timestamp", Timestamp);
            if (Location is {} opt0)
            {
                encoder.WriteExtensionObject<Location>("Location", opt0);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            EncodingMask = decoder.ReadUInt32(null);
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            CodeType = decoder.ReadString("CodeType");
            ScanData = decoder.ReadExtensionObject<ScanData>("ScanData");
            Timestamp = decoder.ReadDateTime("Timestamp");
            if ((EncodingMask & (1u << 0)) != 0)
            {
                Location = decoder.ReadExtensionObject<Location>("Location");
            }
            else
            {
                Location = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for OcrScanResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.9" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5004")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5005")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3002")]
    public class OcrScanResult : ScanResult
    {
        public override int OptionalFieldCount => base.OptionalFieldCount + 2;
        public Workstation.ServiceModel.Ua.NodeId ImageId { get; set; }
        public byte Quality { get; set; }
        public Position Position { get; set; }
        private string _font;
        public string Font
        {
            get => _font;
            set
            {
                uint flag = 1u << (0 + base.OptionalFieldCount);
                
                _font = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private System.DateTime? _decodingTime;
        public System.DateTime? DecodingTime
        {
            get => _decodingTime;
            set
            {
                uint flag = 1u << (1 + base.OptionalFieldCount);
                
                _decodingTime = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteNodeId("ImageId", ImageId);
            encoder.WriteByte("Quality", Quality);
            encoder.WriteExtensionObject<Position>("Position", Position);
            if (Font is {} opt0)
            {
                encoder.WriteString("Font", opt0);
            }
            if (DecodingTime is {} opt1)
            {
                encoder.WriteDateTime("DecodingTime", opt1);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            int offset = base.OptionalFieldCount;
            
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            ImageId = decoder.ReadNodeId("ImageId");
            Quality = decoder.ReadByte("Quality");
            Position = decoder.ReadExtensionObject<Position>("Position");
            if ((EncodingMask & (1u << (0 + offset))) != 0)
            {
                Font = decoder.ReadString("Font");
            }
            else
            {
                Font = null;
            }
            
            if ((EncodingMask & (1u << (1 + offset))) != 0)
            {
                DecodingTime = decoder.ReadDateTime("DecodingTime");
            }
            else
            {
                DecodingTime = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for OpticalScanResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.10" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5040")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5041")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3026")]
    public class OpticalScanResult : ScanResult
    {
        public override int OptionalFieldCount => base.OptionalFieldCount + 4;
        private float? _grade;
        public float? Grade
        {
            get => _grade;
            set
            {
                uint flag = 1u << (0 + base.OptionalFieldCount);
                
                _grade = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private Position _position;
        public Position Position
        {
            get => _position;
            set
            {
                uint flag = 1u << (1 + base.OptionalFieldCount);
                
                _position = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private string _symbology;
        public string Symbology
        {
            get => _symbology;
            set
            {
                uint flag = 1u << (2 + base.OptionalFieldCount);
                
                _symbology = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        private Workstation.ServiceModel.Ua.NodeId _imageId;
        public Workstation.ServiceModel.Ua.NodeId ImageId
        {
            get => _imageId;
            set
            {
                uint flag = 1u << (3 + base.OptionalFieldCount);
                
                _imageId = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            if (Grade is {} opt0)
            {
                encoder.WriteFloat("Grade", opt0);
            }
            if (Position is {} opt1)
            {
                encoder.WriteExtensionObject<Position>("Position", opt1);
            }
            if (Symbology is {} opt2)
            {
                encoder.WriteString("Symbology", opt2);
            }
            if (ImageId is {} opt3)
            {
                encoder.WriteNodeId("ImageId", opt3);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            int offset = base.OptionalFieldCount;
            
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            if ((EncodingMask & (1u << (0 + offset))) != 0)
            {
                Grade = decoder.ReadFloat("Grade");
            }
            else
            {
                Grade = null;
            }
            
            if ((EncodingMask & (1u << (1 + offset))) != 0)
            {
                Position = decoder.ReadExtensionObject<Position>("Position");
            }
            else
            {
                Position = null;
            }
            
            if ((EncodingMask & (1u << (2 + offset))) != 0)
            {
                Symbology = decoder.ReadString("Symbology");
            }
            else
            {
                Symbology = null;
            }
            
            if ((EncodingMask & (1u << (3 + offset))) != 0)
            {
                ImageId = decoder.ReadNodeId("ImageId");
            }
            else
            {
                ImageId = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for OpticalVerifierScanResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.11" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5052")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5053")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3030")]
    public class OpticalVerifierScanResult : OpticalScanResult
    {
        public string IsoGrade { get; set; }
        public short RMin { get; set; }
        public short SymbolContrast { get; set; }
        public short ECMin { get; set; }
        public short Modulation { get; set; }
        public short Defects { get; set; }
        public short Decodability { get; set; }
        public short Decode_ { get; set; }
        public short PrintGain { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteString("IsoGrade", IsoGrade);
            encoder.WriteInt16("RMin", RMin);
            encoder.WriteInt16("SymbolContrast", SymbolContrast);
            encoder.WriteInt16("ECMin", ECMin);
            encoder.WriteInt16("Modulation", Modulation);
            encoder.WriteInt16("Defects", Defects);
            encoder.WriteInt16("Decodability", Decodability);
            encoder.WriteInt16("Decode_", Decode_);
            encoder.WriteInt16("PrintGain", PrintGain);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            IsoGrade = decoder.ReadString("IsoGrade");
            RMin = decoder.ReadInt16("RMin");
            SymbolContrast = decoder.ReadInt16("SymbolContrast");
            ECMin = decoder.ReadInt16("ECMin");
            Modulation = decoder.ReadInt16("Modulation");
            Defects = decoder.ReadInt16("Defects");
            Decodability = decoder.ReadInt16("Decodability");
            Decode_ = decoder.ReadInt16("Decode_");
            PrintGain = decoder.ReadInt16("PrintGain");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for RfidScanResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.12" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5011")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5012")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3007")]
    public class RfidScanResult : ScanResult
    {
        public RfidSighting[] Sighting { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteExtensionObjectArray<RfidSighting>("Sighting", Sighting);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            Sighting = decoder.ReadExtensionObjectArray<RfidSighting>("Sighting");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for RtlsLocationResult
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.15" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5048")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5049")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3028")]
    public class RtlsLocationResult : ScanResult
    {
        public double Speed { get; set; }
        public double Heading { get; set; }
        public Rotation Rotation { get; set; }
        public System.DateTime ReceiveTime { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteDouble("Speed", Speed);
            encoder.WriteDouble("Heading", Heading);
            encoder.WriteExtensionObject<Rotation>("Rotation", Rotation);
            encoder.WriteDateTime("ReceiveTime", ReceiveTime);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            Speed = decoder.ReadDouble("Speed");
            Heading = decoder.ReadDouble("Heading");
            Rotation = decoder.ReadExtensionObject<Rotation>("Rotation");
            ReceiveTime = decoder.ReadDateTime("ReceiveTime");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for ScanSettings
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.7" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5015")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5016")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3010")]
    public class ScanSettings : Workstation.ServiceModel.Ua.Structure,
            IOptionalFields
    {
        public virtual int OptionalFieldCount => 1;
        public uint EncodingMask { get; protected set; }
        public double Duration { get; set; }
        public int Cycles { get; set; }
        public bool DataAvailable { get; set; }
        private LocationTypeEnumeration? _locationType;
        public LocationTypeEnumeration? LocationType
        {
            get => _locationType;
            set
            {
                uint flag = 1u << 0;
                
                _locationType = value;
                EncodingMask = value is null
                    ? EncodingMask | flag
                    : EncodingMask & ~flag;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt32("EncodingMask", EncodingMask);
            encoder.WriteDouble("Duration", Duration);
            encoder.WriteInt32("Cycles", Cycles);
            encoder.WriteBoolean("DataAvailable", DataAvailable);
            if (LocationType is {} opt0)
            {
                encoder.WriteEnumeration<LocationTypeEnumeration>("LocationType", opt0);
            }
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            EncodingMask = decoder.ReadUInt32(null);
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            Duration = decoder.ReadDouble("Duration");
            Cycles = decoder.ReadInt32("Cycles");
            DataAvailable = decoder.ReadBoolean("DataAvailable");
            if ((EncodingMask & (1u << 0)) != 0)
            {
                LocationType = decoder.ReadEnumeration<LocationTypeEnumeration>("LocationType");
            }
            else
            {
                LocationType = null;
            }
            
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for Location
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.4.1" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5013")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5014")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3008")]
    public sealed class Location : Workstation.ServiceModel.Ua.Union
    {
        public enum UnionField
        {
            Null = 0,
            NMEA = 1,
            Local = 2,
            WGS84 = 3,
            Name = 4,
        }
        
        private object _field;
        
        public UnionField SwitchField { get; private set; }
        
        public string NMEA
        {
            get => (string)_field;
            set
            {
                SwitchField = UnionField.NMEA;
                _field = value;
            }
        }
        
        public LocalCoordinate Local
        {
            get => (LocalCoordinate)_field;
            set
            {
                SwitchField = UnionField.Local;
                _field = value;
            }
        }
        
        public WGS84Coordinate WGS84
        {
            get => (WGS84Coordinate)_field;
            set
            {
                SwitchField = UnionField.WGS84;
                _field = value;
            }
        }
        
        public string Name
        {
            get => (string)_field;
            set
            {
                SwitchField = UnionField.Name;
                _field = value;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt32("SwitchField", (uint)SwitchField);
            switch (SwitchField)
            {
                case UnionField.Null:
                    break;
                case UnionField.NMEA:
                    encoder.WriteString("NMEA", NMEA);
                    break;
                case UnionField.Local:
                    encoder.WriteExtensionObject<LocalCoordinate>("Local", Local);
                    break;
                case UnionField.WGS84:
                    encoder.WriteExtensionObject<WGS84Coordinate>("WGS84", WGS84);
                    break;
                case UnionField.Name:
                    encoder.WriteString("Name", Name);
                    break;
                default:
                    throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);
            }
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            var switchField = (UnionField)decoder.ReadUInt32(null);
            switch (switchField)
            {
                case UnionField.Null:
                    _field = null;
                    break;
                case UnionField.NMEA:
                    NMEA = decoder.ReadString("NMEA");
                    break;
                case UnionField.Local:
                    Local = decoder.ReadExtensionObject<LocalCoordinate>("Local");
                    break;
                case UnionField.WGS84:
                    WGS84 = decoder.ReadExtensionObject<WGS84Coordinate>("WGS84");
                    break;
                case UnionField.Name:
                    Name = decoder.ReadString("Name");
                    break;
                default:
                    throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);
            }
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for ScanData
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.4.2" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5030")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5031")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3020")]
    public sealed class ScanData : Workstation.ServiceModel.Ua.Union
    {
        public enum UnionField
        {
            Null = 0,
            ByteString = 1,
            String = 2,
            Epc = 3,
            Custom = 4,
        }
        
        private object _field;
        
        public UnionField SwitchField { get; private set; }
        
        public byte[] ByteString
        {
            get => (byte[])_field;
            set
            {
                SwitchField = UnionField.ByteString;
                _field = value;
            }
        }
        
        public string String
        {
            get => (string)_field;
            set
            {
                SwitchField = UnionField.String;
                _field = value;
            }
        }
        
        public ScanDataEpc Epc
        {
            get => (ScanDataEpc)_field;
            set
            {
                SwitchField = UnionField.Epc;
                _field = value;
            }
        }
        
        public Workstation.ServiceModel.Ua.Variant Custom
        {
            get => (Workstation.ServiceModel.Ua.Variant)_field;
            set
            {
                SwitchField = UnionField.Custom;
                _field = value;
            }
        }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteUInt32("SwitchField", (uint)SwitchField);
            switch (SwitchField)
            {
                case UnionField.Null:
                    break;
                case UnionField.ByteString:
                    encoder.WriteByteString("ByteString", ByteString);
                    break;
                case UnionField.String:
                    encoder.WriteString("String", String);
                    break;
                case UnionField.Epc:
                    encoder.WriteExtensionObject<ScanDataEpc>("Epc", Epc);
                    break;
                case UnionField.Custom:
                    encoder.WriteVariant("Custom", Custom);
                    break;
                default:
                    throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);
            }
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            var switchField = (UnionField)decoder.ReadUInt32(null);
            switch (switchField)
            {
                case UnionField.Null:
                    _field = null;
                    break;
                case UnionField.ByteString:
                    ByteString = decoder.ReadByteString("ByteString");
                    break;
                case UnionField.String:
                    String = decoder.ReadString("String");
                    break;
                case UnionField.Epc:
                    Epc = decoder.ReadExtensionObject<ScanDataEpc>("Epc");
                    break;
                case UnionField.Custom:
                    Custom = decoder.ReadVariant("Custom");
                    break;
                default:
                    throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);
            }
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for WGS84Coordinate
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/AutoID/v101/docs/9.3.16" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5046")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5047")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3027")]
    public class WGS84Coordinate : Workstation.ServiceModel.Ua.Structure
    {
        public string N_S_Hemisphere { get; set; }
        public double Latitude { get; set; }
        public string E_W_Hemisphere { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public System.DateTime Timestamp { get; set; }
        public double DilutionOfPrecision { get; set; }
        public int UsefulPrecisionLatLon { get; set; }
        public int UsefulPrecisionAlt { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            encoder.WriteString("N_S_Hemisphere", N_S_Hemisphere);
            encoder.WriteDouble("Latitude", Latitude);
            encoder.WriteString("E_W_Hemisphere", E_W_Hemisphere);
            encoder.WriteDouble("Longitude", Longitude);
            encoder.WriteDouble("Altitude", Altitude);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteDouble("DilutionOfPrecision", DilutionOfPrecision);
            encoder.WriteInt32("UsefulPrecisionLatLon", UsefulPrecisionLatLon);
            encoder.WriteInt32("UsefulPrecisionAlt", UsefulPrecisionAlt);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            
            N_S_Hemisphere = decoder.ReadString("N_S_Hemisphere");
            Latitude = decoder.ReadDouble("Latitude");
            E_W_Hemisphere = decoder.ReadString("E_W_Hemisphere");
            Longitude = decoder.ReadDouble("Longitude");
            Altitude = decoder.ReadDouble("Altitude");
            Timestamp = decoder.ReadDateTime("Timestamp");
            DilutionOfPrecision = decoder.ReadDouble("DilutionOfPrecision");
            UsefulPrecisionLatLon = decoder.ReadInt32("UsefulPrecisionLatLon");
            UsefulPrecisionAlt = decoder.ReadInt32("UsefulPrecisionAlt");
            
            decoder.PopNamespace();
        }
    }
    
}
