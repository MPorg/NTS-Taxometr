
namespace TaxometrMauiMvvm.Data.Json
{
    public class Request
    {

        public Request(Credentials credentials, OperationData operationData)
        {
            this.credentials = credentials;
            this.operationData = operationData;
        }

        public Credentials credentials;
        public OperationData operationData;

        public class Credentials
        {
            public Credentials(string authorizationToken)
            {
                this.authorizationToken = authorizationToken;
            }

            public string authorizationToken;
        }

        public class OperationData
        {
            public OperationData(string instrument, string operationExternalId, AmountData amountData, Goods goods)
            {
                this.instrument = instrument;
                this.operationExternalId = operationExternalId;
                this.amountData = amountData;
                this.goods = goods;
            }

            public string instrument;
            public string operationExternalId;
            public AmountData amountData;
            public Goods goods;

            public class AmountData
            {
                public AmountData(string currencyCode, string amount, string amountExponent)
                {
                    this.currencyCode = currencyCode;
                    this.amount = amount;
                    this.amountExponent = amountExponent;
                }

                public string currencyCode;
                public string amount;
                public string amountExponent;

            }

            public class Goods
            {
                public Goods(Product[] product)
                {
                    this.product = product;
                }

                public Product[] product;
                
                public class Product
                {
                    public Product(string name, string price, string quantity, string quantityExponent, string taxRate, string accountingSubject)
                    {
                        this.name = name;
                        this.price = price;
                        this.quantity = quantity;
                        this.quantityExponent = quantityExponent;
                        this.taxRate = taxRate;
                        this.accountingSubject = accountingSubject;
                    }

                    public string name;
                    public string price;
                    public string quantity;
                    public string quantityExponent;
                    public string taxRate;
                    public string accountingSubject;

                }
            }
        }
    }
}
