﻿// Copyright (c) 2012-2016 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

namespace Dicom.Network
{
    using System.Text;

    public class DicomNActionRequest : DicomRequest
    {
        public DicomNActionRequest(DicomDataset command)
            : base(command)
        {
        }

        public DicomNActionRequest(
            DicomUID requestedClassUid,
            DicomUID requestedInstanceUid,
            ushort actionTypeId)
            : base(DicomCommandField.NActionRequest, requestedClassUid)
        {
            SOPInstanceUID = requestedInstanceUid;
            ActionTypeID = actionTypeId;
        }

        public DicomUID SOPInstanceUID
        {
            get
            {
                return Command.Get<DicomUID>(DicomTag.RequestedSOPInstanceUID);
            }
            private set
            {
                Command.Add(DicomTag.RequestedSOPInstanceUID, value);
            }
        }

        public ushort ActionTypeID
        {
            get
            {
                return Command.Get<ushort>(DicomTag.ActionTypeID);
            }
            private set
            {
                Command.Add(DicomTag.ActionTypeID, value);
            }
        }

        public delegate void ResponseDelegate(DicomNActionRequest request, DicomNActionResponse response);

        public ResponseDelegate OnResponseReceived;

        internal override void PostResponse(DicomService service, DicomResponse response)
        {
            try
            {
                if (OnResponseReceived != null) OnResponseReceived(this, (DicomNActionResponse)response);
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} [{1}]", ToString(Type), MessageID);
            sb.AppendFormat("\n\t\tAction Type:	{0:x4}", ActionTypeID);
            return sb.ToString();
        }
    }
}
