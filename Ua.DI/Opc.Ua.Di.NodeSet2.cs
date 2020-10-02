namespace Ua.DI
{
    // Ideally this should go into Workstation.UaClient
    public interface IOptionalFields
    {
        int OptionalFieldCount { get; }
        uint EncodingMask { get; }
    }
    
    /// <summary>
    /// DeviceHealthEnumeration enumeration
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/DI/v102/docs/5.5.4" />
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6244")]
    public enum DeviceHealthEnumeration
    {
        /// <summary>This device functions normally.</summary>
        NORMAL = 0,
        /// <summary>Malfunction of the device or any of its peripherals.</summary>
        FAILURE = 1,
        /// <summary>Functional checks are currently performed.</summary>
        CHECK_FUNCTION = 2,
        /// <summary>The device is currently working outside of its specified range or that internal diagnoses indicate deviations from measured or set values.</summary>
        OFF_SPEC = 3,
        /// <summary>This element is working, but a maintenance operation is required.</summary>
        MAINTENANCE_REQUIRED = 4,
    }
    
    /// <summary>
    /// Class for FetchResultDataType
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/DI/v102/docs/8.2.6" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6551")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6535")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6522")]
    public abstract class FetchResultDataType : Workstation.ServiceModel.Ua.Structure
    {
    }
    
    /// <summary>
    /// Class for TransferResultErrorDataType
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/DI/v102/docs/8.2.6" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15891")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15900")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=15888")]
    public class TransferResultErrorDataType : FetchResultDataType
    {
        public int Status { get; set; }
        public Workstation.ServiceModel.Ua.DiagnosticInfo Diagnostics { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            
            encoder.WriteInt32("Status", Status);
            encoder.WriteDiagnosticInfo("Diagnostics", Diagnostics);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
        public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)
        {
            base.Decode(decoder);
            decoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            
            Status = decoder.ReadInt32("Status");
            Diagnostics = decoder.ReadDiagnosticInfo("Diagnostics");
            
            decoder.PopNamespace();
        }
    }
    
    /// <summary>
    /// Class for TransferResultDataDataType
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/DI/v102/docs/8.2.6" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15892")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=15901")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=15889")]
    public class TransferResultDataDataType : FetchResultDataType
    {
        public int SequenceNumber { get; set; }
        public bool EndOfResults { get; set; }
        public ParameterResultDataType[] ParameterDefs { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            
            encoder.WriteInt32("SequenceNumber", SequenceNumber);
            encoder.WriteBoolean("EndOfResults", EndOfResults);
            encoder.WriteExtensionObjectArray<ParameterResultDataType>("ParameterDefs", ParameterDefs);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
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
    
    /// <summary>
    /// Class for ParameterResultDataType
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/DI/v102/docs/8.2.6" />
    [Workstation.ServiceModel.Ua.BinaryEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6554")]
    [Workstation.ServiceModel.Ua.XmlEncodingId("nsu=http://opcfoundation.org/UA/DI/;i=6538")]
    [Workstation.ServiceModel.Ua.DataTypeId("nsu=http://opcfoundation.org/UA/DI/;i=6525")]
    public class ParameterResultDataType : Workstation.ServiceModel.Ua.Structure
    {
        public Workstation.ServiceModel.Ua.QualifiedName[] NodePath { get; set; }
        public Workstation.ServiceModel.Ua.StatusCode StatusCode { get; set; }
        public Workstation.ServiceModel.Ua.DiagnosticInfo Diagnostics { get; set; }
        
        /// <<inheritdoc/>
        public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.PushNamespace("http://opcfoundation.org/UA/DI/");
            
            encoder.WriteQualifiedNameArray("NodePath", NodePath);
            encoder.WriteStatusCode("StatusCode", StatusCode);
            encoder.WriteDiagnosticInfo("Diagnostics", Diagnostics);
            
            encoder.PopNamespace();
        }
        
        /// <<inheritdoc/>
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
