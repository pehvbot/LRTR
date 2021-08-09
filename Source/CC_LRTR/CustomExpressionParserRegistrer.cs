using ContractConfigurator.ExpressionParser;
using LRTR;
using System;
using System.Reflection;
using UnityEngine;

namespace ContractConfigurator.LRTR
{
    /// <summary>
    /// Used for registering some custom CC functions specific to LRTR.
    /// Needs to inherit from BaseParser just to have access to the protected static RegisterGlobalFunction() method.
    /// </summary>
    public class CustomExpressionParserRegistrer : BaseParser, IExpressionParserRegistrer
    {
        public override MethodInfo methodParseStatementInner => throw new NotImplementedException();

        public override MethodInfo methodGetRval => throw new NotImplementedException();

        public override MethodInfo methodApplyBooleanOperator => throw new NotImplementedException();

        public override MethodInfo methodParseStatement => throw new NotImplementedException();

        public override MethodInfo methodParseMethod => throw new NotImplementedException();

        public override MethodInfo methodCompleteIdentifierParsing => throw new NotImplementedException();

        public override MethodInfo method_ConvertType => throw new NotImplementedException();

        static CustomExpressionParserRegistrer()
        {
            RegisterMethods();
        }

        public override void ExecuteAndStoreExpression(string key, string expression, DataNode dataNode)
        {
            throw new NotImplementedException();
        }

        public override object ParseExpressionGeneric(string key, string expression, DataNode dataNode)
        {
            throw new NotImplementedException();
        }

        public void RegisterExpressionParsers()
        {
            // Nothing as of yet
        }

        private static void RegisterMethods()
        {
            Debug.Log("[LRTR] CustomExpressionParserRegistrer registering methods");
            RegisterGlobalFunction(new Function<float>("LRTRDeadlineMult", GetDeadlineMult, false));
            RegisterGlobalFunction(new Function<float>("LRTRDayInSeconds", GetDayInSeconds, false));
            RegisterGlobalFunction(new Function<float>("LRTRYearInDays", GetYearInDays, false));
        }

        private static float GetDeadlineMult()
        {
            return HighLogic.CurrentGame?.Parameters.CustomParams<LRTRSettings>()?.ContractDeadlineMult ?? 1;
        }
        private static float GetDayInSeconds()
        {
            ConfigNode paramsNode = null;

            foreach (ConfigNode lrtrConfig in GameDatabase.Instance.GetConfigNodes("LRTRCONFIG"))
                paramsNode = lrtrConfig;

            if (paramsNode == null)
            {
                Debug.LogError("[LRTRCONFIG] Could not find LRTRCONFIG node.");
                return 86400;
            }

            if (paramsNode.GetValue("hoursPerDay") != null)
            {
                return float.Parse(paramsNode.GetValue("hoursPerDay")) * 3600;
            }
            return 86400;
        }
        private static float GetYearInDays()
        {
            ConfigNode paramsNode = null;

            foreach (ConfigNode lrtrConfig in GameDatabase.Instance.GetConfigNodes("LRTRCONFIG"))
                paramsNode = lrtrConfig;

            if (paramsNode == null)
            {
                //Debug.LogError("[LRTRCONFIG] Could not find LRTRCONFIG node.");
                return 365;
            }

            if (paramsNode.GetValue("daysPerYear") != null)
            {
                return float.Parse(paramsNode.GetValue("daysPerYear"));
            }
            return 365;
        }
    }
}
