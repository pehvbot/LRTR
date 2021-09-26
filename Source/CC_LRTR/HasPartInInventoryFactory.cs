using Contracts;

namespace ContractConfigurator.LRTR
{
    public class HasPartInInventoryFactory : ParameterFactory
    {
        protected string partName;
        protected double partCount;

        public override bool Load(ConfigNode configNode)
        {
            bool valid = base.Load(configNode);
            valid &= ConfigNodeUtil.ParseValue<double>(configNode, "partCount", x => partCount = x, this, 1);
            valid &= ConfigNodeUtil.ParseValue<string>(configNode, "partName", x => partName = x, this);

            return valid;
        }

        public override ContractParameter Generate(Contract contract)
        {
            return new HasPartInInventory(title, partName, partCount);
        }
    }
}
