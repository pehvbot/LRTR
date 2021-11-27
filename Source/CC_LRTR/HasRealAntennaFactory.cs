using Contracts;

namespace ContractConfigurator.LRTR
{
    public class HasRealAntennaFactory : ParameterFactory
    {
        protected double minGain;
        protected double minAntennaDiameter;
        protected string RFBand;

        public override bool Load(ConfigNode configNode)
        {
            bool valid = base.Load(configNode);
            valid &= ConfigNodeUtil.ParseValue<double>(configNode, "minGain", x => minGain = x, this, 0);
            valid &= ConfigNodeUtil.ParseValue<double>(configNode, "minAntennaDiameter", x => minAntennaDiameter = x, this, 0);
            valid &= ConfigNodeUtil.ParseValue<string>(configNode, "RFBand", x => RFBand = x, this, "");

            return valid;
        }

        public override ContractParameter Generate(Contract contract)
        {
            return new HasRealAntenna(title, minGain, minAntennaDiameter, RFBand);
        }
    }
}
