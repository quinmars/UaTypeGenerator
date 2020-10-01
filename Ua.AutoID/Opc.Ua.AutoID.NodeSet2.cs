namespace Ua.AutoID
{
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3013")]
    public enum AutoIdOperationStatusEnumeration
    {
        SUCCESS = 0,
        MISC_ERROR_TOTAL = 1,
        MISC_ERROR_PARTIAL = 2,
        PERMISSON_ERROR = 3,
        PASSWORD_ERROR = 4,
        REGION_NOT_FOUND_ERROR = 5,
        OP_NOT_POSSIBLE_ERROR = 6,
        OUT_OF_RANGE_ERROR = 7,
        NO_IDENTIFIER = 8,
        MULTIPLE_IDENTIFIERS = 9,
        READ_ERROR = 10,
        DECODING_ERROR = 11,
        MATCH_ERROR = 12,
        CODE_NOT_SUPPORTED = 13,
        WRITE_ERROR = 14,
        NOT_SUPPORTED_BY_DEVICE = 15,
        NOT_SUPPORTED_BY_TAG = 16,
        DEVICE_NOT_READY = 17,
        INVALID_CONFIGURATION = 18,
        RF_COMMUNICATION_ERROR = 19,
        DEVICE_FAULT = 20,
        TAG_HAS_LOW_BATTERY = 21,
    }
    
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3003")]
    public enum DeviceStatusEnumeration
    {
        Idle = 0,
        Error = 1,
        Scanning = 2,
        Busy = 3,
    }
    
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3009")]
    public enum LocationTypeEnumeration
    {
        NMEA = 0,
        LOCAL = 1,
        WGS84 = 2,
        NAME = 3,
    }
    
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3016")]
    public enum RfidLockOperationEnumeration
    {
        Lock = 0,
        Unlock = 1,
        PermanentLock = 2,
        PermanentUnlock = 3,
    }
    
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3015")]
    public enum RfidLockRegionEnumeration
    {
        Kill = 0,
        Access = 1,
        EPC = 2,
        TID = 3,
        User = 4,
    }
    
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3014")]
    public enum RfidPasswordTypeEnumeration
    {
        Access = 0,
        Kill = 1,
        Read = 2,
        Write = 3,
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5022")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5023")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3017")]
    public class AccessResult : Workstation.ServiceModel.Ua.Structure
    {
        public string CodeType { get; set; }
        public ScanData Identifier { get; set; }
        public System.DateTime Timestamp { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteString("CodeType", CodeType);
            encoder.WriteExtensionObject<ScanData>("Identifier", Identifier);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            CodeType = decoder.ReadString("CodeType");
            Identifier = decoder.ReadExtensionObject<ScanData>("Identifier");
            Timestamp = decoder.ReadDateTime("Timestamp");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5024")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5025")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3018")]
    public class RfidAccessResult : AccessResult
    {
        public string CodeTypeRWData { get; set; }
        public ScanData RWData { get; set; }
        public int Antenna { get; set; }
        public int CurrentPowerLevel { get; set; }
        public ushort PC { get; set; }
        public string Polarization { get; set; }
        public int Strength { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteString("CodeTypeRWData", CodeTypeRWData);
            encoder.WriteExtensionObject<ScanData>("RWData", RWData);
            encoder.WriteInt32("Antenna", Antenna);
            encoder.WriteInt32("CurrentPowerLevel", CurrentPowerLevel);
            encoder.WriteUInt16("PC", PC);
            encoder.WriteString("Polarization", Polarization);
            encoder.WriteInt32("Strength", Strength);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            CodeTypeRWData = decoder.ReadString("CodeTypeRWData");
            RWData = decoder.ReadExtensionObject<ScanData>("RWData");
            Antenna = decoder.ReadInt32("Antenna");
            CurrentPowerLevel = decoder.ReadInt32("CurrentPowerLevel");
            PC = decoder.ReadUInt16("PC");
            Polarization = decoder.ReadString("Polarization");
            Strength = decoder.ReadInt32("Strength");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5017")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5018")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3011")]
    public class AntennaNameIdPair : Workstation.ServiceModel.Ua.Structure
    {
        public int AntennaId { get; set; }
        public string AntennaName { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteInt32("AntennaId", AntennaId);
            encoder.WriteString("AntennaName", AntennaName);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            AntennaId = decoder.ReadInt32("AntennaId");
            AntennaName = decoder.ReadString("AntennaName");
            decoder.PopNamespace();
        }
    }
    
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5009")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5010")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3006")]
    public class RfidSighting : Workstation.ServiceModel.Ua.Structure
    {
        public int Antenna { get; set; }
        public int Strength { get; set; }
        public System.DateTime Timestamp { get; set; }
        public int CurrentPowerLevel { get; set; }
        
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5050")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5051")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3029")]
    public class Rotation : Workstation.ServiceModel.Ua.Structure
    {
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteDouble("Yaw", Yaw);
            encoder.WriteDouble("Pitch", Pitch);
            encoder.WriteDouble("Roll", Roll);
            encoder.PopNamespace();
        }
        
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5036")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5037")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3024")]
    public class ScanDataEpc : Workstation.ServiceModel.Ua.Structure
    {
        public ushort PC { get; set; }
        public byte[] UId { get; set; }
        public ushort XPC_W1 { get; set; }
        public ushort XPC_W2 { get; set; }
        
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5002")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5003")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3001")]
    public abstract class ScanResult : Workstation.ServiceModel.Ua.Structure
    {
        public string CodeType { get; set; }
        public ScanData ScanData { get; set; }
        public System.DateTime Timestamp { get; set; }
        public Location Location { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteString("CodeType", CodeType);
            encoder.WriteExtensionObject<ScanData>("ScanData", ScanData);
            encoder.WriteDateTime("Timestamp", Timestamp);
            encoder.WriteExtensionObject<Location>("Location", Location);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            CodeType = decoder.ReadString("CodeType");
            ScanData = decoder.ReadExtensionObject<ScanData>("ScanData");
            Timestamp = decoder.ReadDateTime("Timestamp");
            Location = decoder.ReadExtensionObject<Location>("Location");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5004")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5005")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3002")]
    public class OcrScanResult : ScanResult
    {
        public Workstation.ServiceModel.Ua.NodeId ImageId { get; set; }
        public byte Quality { get; set; }
        public Position Position { get; set; }
        public string Font { get; set; }
        public System.DateTime DecodingTime { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteNodeId("ImageId", ImageId);
            encoder.WriteByte("Quality", Quality);
            encoder.WriteExtensionObject<Position>("Position", Position);
            encoder.WriteString("Font", Font);
            encoder.WriteDateTime("DecodingTime", DecodingTime);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            ImageId = decoder.ReadNodeId("ImageId");
            Quality = decoder.ReadByte("Quality");
            Position = decoder.ReadExtensionObject<Position>("Position");
            Font = decoder.ReadString("Font");
            DecodingTime = decoder.ReadDateTime("DecodingTime");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5040")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5041")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3026")]
    public class OpticalScanResult : ScanResult
    {
        public float Grade { get; set; }
        public Position Position { get; set; }
        public string Symbology { get; set; }
        public Workstation.ServiceModel.Ua.NodeId ImageId { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteFloat("Grade", Grade);
            encoder.WriteExtensionObject<Position>("Position", Position);
            encoder.WriteString("Symbology", Symbology);
            encoder.WriteNodeId("ImageId", ImageId);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            Grade = decoder.ReadFloat("Grade");
            Position = decoder.ReadExtensionObject<Position>("Position");
            Symbology = decoder.ReadString("Symbology");
            ImageId = decoder.ReadNodeId("ImageId");
            decoder.PopNamespace();
        }
    }
    
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5011")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5012")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3007")]
    public class RfidScanResult : ScanResult
    {
        public RfidSighting[] Sighting { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteExtensionObjectArray<RfidSighting>("Sighting", Sighting);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            Sighting = decoder.ReadExtensionObjectArray<RfidSighting>("Sighting");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5048")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5049")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3028")]
    public class RtlsLocationResult : ScanResult
    {
        public double Speed { get; set; }
        public double Heading { get; set; }
        public Rotation Rotation { get; set; }
        public System.DateTime ReceiveTime { get; set; }
        
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
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5015")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/AutoID/;i=5016")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/AutoID/;i=3010")]
    public class ScanSettings : Workstation.ServiceModel.Ua.Structure
    {
        public double Duration { get; set; }
        public int Cycles { get; set; }
        public bool DataAvailable { get; set; }
        public LocationTypeEnumeration LocationType { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            encoder.WriteDouble("Duration", Duration);
            encoder.WriteInt32("Cycles", Cycles);
            encoder.WriteBoolean("DataAvailable", DataAvailable);
            encoder.WriteEnumeration<LocationTypeEnumeration>("LocationType", LocationType);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/AutoID/");
            Duration = decoder.ReadDouble("Duration");
            Cycles = decoder.ReadInt32("Cycles");
            DataAvailable = decoder.ReadBoolean("DataAvailable");
            LocationType = decoder.ReadEnumeration<LocationTypeEnumeration>("LocationType");
            decoder.PopNamespace();
        }
    }
    
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