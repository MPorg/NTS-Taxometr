namespace TaxometrMauiMvvm.Data.Json
{
    public class Response
    {
        public Result result;
        public Transaction transaction;

        public class Result
        {
            public string code;
            public string description;
            public HostResponse hostResponse;
            public ErrorData errorData;

            public class HostResponse
            {
                public string code;
                public string description;
            }

            public class ErrorData
            {
                public string code;
                public string description;
            }
        }

        public class Transaction
        {
            public string terminalId;
            public string operationDay;
            public string transactionNumber;
            public string reversible;
            public string refundable;
            public string externalTerminalId;
            public string dateTime;
            public string operationExternalId;
            public InstrumentSpecificData instrumentSpecificData;
            public Company company;
            public Merchant merchant;

            public class InstrumentSpecificData
            {
                public string authorizationCode;
                public string rrn;
                public string cardholderName;
                public string maskedPan;
                public string cardType;
                public string applicationId;
            }

            public class Company
            {
                public string name;
            }

            public class Merchant
            {
                public string name;
                public string address;
                public string phoneNumber;
                public string merchantId;
            }
        }
    }
}
