using BreakInfinity;
using Datas.DataModels.Generators;
using Enums;
using UnityEngine;

namespace Helpers
{
    public static class Calculator
    {
        public static ulong CalculateAmountToBuy(MultiBuyOption buyOption, GeneratorCostDataModel costData,
            ulong generatorLevel, BigDouble currentCurrency)
        {
            ulong n = 1; // The number of generators to buy
            var b = costData.baseCost; // The base price (base cost)
            var r = costData.costCoefficient; // The price growth rate exponent (price coefficient)
            var k = generatorLevel; // The number of generators currently owned (level)
            var c = currentCurrency; // The amount of currency owned (money)

            switch (buyOption)
            {
                case MultiBuyOption.One:
                    n = 1;
                    break;
                case MultiBuyOption.Ten:
                    n = 10;
                    break;
                case MultiBuyOption.TwentyFive:
                    break;
                case MultiBuyOption.Fifty:
                    n = 50;
                    break;
                case MultiBuyOption.Next:
                    n = 1; // _nextBonus - _level;
                    break;
                case MultiBuyOption.Max:
                    var amount = BigDouble.Floor(BigDouble.Log((c * (r - 1)) / (b * BigDouble.Pow(r, k)) + 1, r));
                    n = (ulong)amount.ToDouble();
                    break;
                default:
                    n = 1;
                    Debug.LogWarning($"You didn't handle the calculation of {buyOption.ToString()} amount.");
                    break;
            }

            if (n < 1)
            {
                n = 1;
            }

            return n;
        }

        public static BigDouble GetGeneratorCost(GeneratorCostDataModel costData, ulong amountToBuy,
            ulong generatorLevel)
        {
            if (amountToBuy <= 1)
            {
                return GetGeneratorCostOfOne(costData, generatorLevel);
            }

            BigDouble n = amountToBuy; // The number of generators to buy
            var b = costData.baseCost; // The base price (base cost)
            var r = costData.costCoefficient; // The price growth rate exponent (price coefficient)
            var k = generatorLevel; // The number of generators currently owned (level)

            var result = b * (BigDouble.Pow(r, k) * (BigDouble.Pow(r, n) - 1) / (r - 1));

            return result;
        }

        private static BigDouble GetGeneratorCostOfOne(GeneratorCostDataModel costData, ulong generatorLevel)
        {
            return costData.baseCost * BigDouble.Pow(costData.costCoefficient, generatorLevel);
        }
    }
}