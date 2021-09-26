using Contracts;

namespace ContractConfigurator.LRTR
{
    public class LRTRRendezvousFactory : ParameterFactory
    {
        protected double distance;
        protected double relativeSpeed;

        public override bool Load(ConfigNode configNode)
        {
            bool valid = base.Load(configNode);

            valid &= ConfigNodeUtil.ParseValue<double>(configNode, "distance", x => distance = x, this, 100);
            valid &= ConfigNodeUtil.ParseValue<double>(configNode, "relativeSpeed", x => relativeSpeed = x, this, 0);

            return valid;
        }

        public override ContractParameter Generate(Contract contract)
        {
            return new LRTRRendezvous(distance, relativeSpeed, title);
        }
    }
}
