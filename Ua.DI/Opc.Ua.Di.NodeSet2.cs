namespace Ua.DI
{
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6244")]
    public enum DeviceHealthEnumeration
    {
        NORMAL = 0,
        FAILURE = 1,
        CHECK_FUNCTION = 2,
        OFF_SPEC = 3,
        MAINTENANCE_REQUIRED = 4,
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6551")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6535")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6522")]
    public abstract class FetchResultDataType : Workstation.ServiceModel.Ua.Structure
    {
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15891")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15900")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=15888")]
    public class TransferResultErrorDataType : FetchResultDataType
    {
        public int Status { get; set; }
        public Workstation.ServiceModel.Ua.DiagnosticInfo Diagnostics { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            encoder.WriteInt32("Status", Status);
            encoder.WriteDiagnosticInfo("Diagnostics", Diagnostics);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            Status = decoder.ReadInt32("Status");
            Diagnostics = decoder.ReadDiagnosticInfo("Diagnostics");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15892")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15901")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=15889")]
    public class TransferResultDataDataType : FetchResultDataType
    {
        public int SequenceNumber { get; set; }
        public bool EndOfResults { get; set; }
        public ParameterResultDataType[] ParameterDefs { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            encoder.WriteInt32("SequenceNumber", SequenceNumber);
            encoder.WriteBoolean("EndOfResults", EndOfResults);
            encoder.WriteExtensionObjectArray<ParameterResultDataType>("ParameterDefs", ParameterDefs);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            SequenceNumber = decoder.ReadInt32("SequenceNumber");
            EndOfResults = decoder.ReadBoolean("EndOfResults");
            ParameterDefs = decoder.ReadExtensionObjectArray<ParameterResultDataType>("ParameterDefs");
            decoder.PopNamespace();
        }
    }
    
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6554")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6538")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6525")]
    public class ParameterResultDataType : Workstation.ServiceModel.Ua.Structure
    {
        public Workstation.ServiceModel.Ua.QualifiedName[] NodePath { get; set; }
        public Workstation.ServiceModel.Ua.StatusCode StatusCode { get; set; }
        public Workstation.ServiceModel.Ua.DiagnosticInfo Diagnostics { get; set; }
        
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            encoder.WriteQualifiedNameArray("NodePath", NodePath);
            encoder.WriteStatusCode("StatusCode", StatusCode);
            encoder.WriteDiagnosticInfo("Diagnostics", Diagnostics);
            encoder.PopNamespace();
        }
        
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            NodePath = decoder.ReadQualifiedNameArray("NodePath");
            StatusCode = decoder.ReadStatusCode("StatusCode");
            Diagnostics = decoder.ReadDiagnosticInfo("Diagnostics");
            decoder.PopNamespace();
        }
    }
    
}
