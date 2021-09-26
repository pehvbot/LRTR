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
    ///

    public class CustomExpressionParserRegistrer : BaseParser, IExpressionParserRegistrer
    {
        public override MethodInfo methodParseStatementInner => throw new NotImplementedException();

        public override MethodInfo methodGetRval => throw new NotImplementedException();

        public override MethodInfo methodApplyBooleanOperator => throw new NotImplementedException();

        public override MethodInfo methodParseStatement => throw new NotImplementedException();

        public override MethodInfo methodParseMethod => throw new NotImplementedException();

        public override MethodInfo methodCompleteIdentifierParsing => throw new NotImplementedException();

        public override MethodInfo method_ConvertType => throw new NotImplementedException();

        public static LRTRHomeWorldParameters Config { get; private set; } = null;

        static CustomExpressionParserRegistrer()
        {
            RegisterMethods();
            if (Config == null)
            {
                Config = new LRTRHomeWorldParameters();
                foreach (ConfigNode stg in GameDatabase.Instance.GetConfigNodes("HOMEWORLDPARAMETERS"))
                    Config.Load(stg);
            }
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
            RegisterGlobalFunction(new Function<bool>("LRTRYesPlanes", GetPlaneContractsEnabled, false));
            RegisterGlobalFunction(new Function<float>("LRTRDaysPerYear", GetDaysPerYear, false));
            RegisterGlobalFunction(new Function<float>("LRTRHoursPerDay", GetHoursPerDay, false));
            RegisterGlobalFunction(new Function<float>("LRTRKarmanLine", GetKarmanLine, false));
        }

        private static float GetDeadlineMult()
        {
            return HighLogic.CurrentGame?.Parameters.CustomParams<LRTRSettings>()?.ContractDeadlineMult ?? 1;
        }

        private static bool GetPlaneContractsEnabled()
        {
            return HighLogic.CurrentGame?.Parameters.CustomParams<LRTRSettings>()?.PlaneContractsEnabled ?? true;
        }

        private static float GetDaysPerYear()
        {
            return (float)Config.daysPerYear;
        }

        private static float GetHoursPerDay()
        {
            return (float)Config.hoursPerDay;
        }

        private static float GetKarmanLine()
        {
            return (float)Config.karmanAltitude;
        }

        private static bool WithdrawContract(string contractName)
        {
            if (ContractPreLoader.Instance == null) return false;
            foreach (ConfiguredContract cc in ContractPreLoader.Instance.PendingContracts())
            {
                string name = cc?.contractType?.name;
                if (name == contractName)
                {
                    cc.Withdraw();
                    return true;
                }
            }

            return false;
        }
    }
}
