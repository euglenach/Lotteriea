using System;
using System.Collections.Generic;
using System.Linq;
using Random = Unity.Mathematics.Random;

namespace LotterySystem
{
    public struct LotterieaBox
    {
        private Random random;
        
        public LotterieaBox(uint seed)
        {
            random = new Random(seed);
        }

        public LotterieaBox(Random random)
        {
            this.random = random;
        }
        
        public static LotterieaBox CreateFromIndex(uint index)
        {
            return new LotterieaBox(Random.CreateFromIndex(index));
        }

        public void InitState(uint seed = 0x6E624EB7u)
        {
            random.InitState(seed);
        }

        public bool True(float trueProbability, RandomType randomType = RandomType.Ratio)
        {
            if(trueProbability <= 0) return false;

            return randomType switch
            {
                RandomType.Ratio => random.NextFloat() <= trueProbability,
                RandomType.Percent => random.NextFloat(0f, 100f) <= trueProbability,
                _ => throw new ArgumentOutOfRangeException(nameof(randomType), randomType, null)
            };
        }
    
        public bool False(float falseProbability, RandomType randomType = RandomType.Ratio)
        {
            return !True(falseProbability, randomType);
        }

        #region Select
        
        public T Select<T>(IEnumerable<T> source)
        {
            if(InternalUtility.TryAsSpan(source, out var span)) return Select((ReadOnlySpan<T>)span);
            if(source is IReadOnlyList<T> readOnlyList) return Select(readOnlyList);
            
            var buffer = source.ToArray();
            return Select(buffer);
        }
        
        public T Select<T>(ReadOnlySpan<T> source)
        {
            var count = source.Length;
            var index = random.NextInt(0, count);
            return source[index];
        }
        
        public T Select<T>(IReadOnlyList<T> source)
        {
            var count = source.Count;
            var index = random.NextInt(0, count);
            return source[index];
        }

        public T Select<T>(params T[] source)
        {
            var count = source.Length;
            var index = random.NextInt(0, count);
            return source[index];
        }
        
        public T Select<T>(T item1, T item2)
        {
            return random.NextBool()? item1 : item2;
        }
        
        public ref T Select<T>(ref T[] source)
        {
            var count = source.Length;
            var index = random.NextInt(0, count);
            return ref source[index];
        }

        public ref T Select<T>(ref T item1, ref T item2)
        {
            return ref random.NextBool()? ref item1 : ref item2;
        }

        #endregion
        
        #region WeightLotteryIndex
        
        public int WeightLotteryIndex(params int[] weightTable) => WeightLotteryIndex((ReadOnlySpan<int>)weightTable);
        
        public int WeightLotteryIndex(ReadOnlySpan<int> weightTable)
        {
            var totalWeight = SumWeight(weightTable);
            var value = random.NextInt(0, totalWeight);
            for (var i = 0; i < weightTable.Length; ++i)
            {
                if (weightTable[i] > value)
                {
                    return i;
                }
                value -= weightTable[i];
            }
            throw new InvalidOperationException("Failed to select a weighted index.");
        }
        
        public int WeightLotteryIndex(params IWeight[] weightTable) => WeightLotteryIndex((ReadOnlySpan<IWeight>)weightTable);
        
        public int WeightLotteryIndex<TWeight>(params TWeight[] weightTable) where TWeight : IWeight => WeightLotteryIndex((ReadOnlySpan<TWeight>)weightTable);
        
        public int WeightLotteryIndex(ReadOnlySpan<IWeight> weightTable)
        {
            var totalWeight = SumWeight(weightTable);
            var value = random.NextInt(0, totalWeight);
            for (var i = 0; i < weightTable.Length; ++i)
            {
                var weight = weightTable[i].Weight;
                if (weight > value)
                {
                    return i;
                }
                value -= weight;
            }
            throw new InvalidOperationException("Failed to select a weighted index.");
        }
        
        public int WeightLotteryIndex<TWeight>(ReadOnlySpan<TWeight> weightTable) where TWeight : IWeight
        {
            var totalWeight = SumWeight(weightTable);
            var value = random.NextInt(0, totalWeight);
            for (var i = 0; i < weightTable.Length; ++i)
            {
                var weight = weightTable[i].Weight;
                if (weight > value)
                {
                    return i;
                }
                value -= weight;
            }
            throw new InvalidOperationException("Failed to select a weighted index.");
        }
        
        #endregion LotteryIndex

        #region WeightLottery
        
        public IWeight WeightLottery(params IWeight[] weightTable) => WeightLottery((ReadOnlySpan<IWeight>)weightTable);
        public TWeight WeightLottery<TWeight>(params TWeight[] weightTable) where TWeight : IWeight => WeightLottery((ReadOnlySpan<TWeight>)weightTable);

        public IWeight WeightLottery(ReadOnlySpan<IWeight> weightTable)
        {
            var index = WeightLotteryIndex(weightTable);
            return weightTable[index];
        }
        
        public TWeight WeightLottery<TWeight>(ReadOnlySpan<TWeight> weightTable) where TWeight : IWeight
        {
            var index = WeightLotteryIndex(weightTable);
            return weightTable[index];
        }
        
        public ref IWeight WeightLotteryRef(params IWeight[] weightTable)
        {
            var index = WeightLotteryIndex(weightTable);
            return ref weightTable[index];
        }
        
        public ref TWeight WeightLotteryRef<TWeight>(params TWeight[] weightTable) where TWeight : IWeight
        {
            var index = WeightLotteryIndex(weightTable);
            return ref weightTable[index];
        }
        
        #endregion WeightLottery

        #region Shuffle

        public Span<T> Shuffle<T>(Span<T> source)
        {
            var count = source.Length;
            for (var i = 0; i < count; ++i)
            {
                var index = random.NextInt(i, count);
                (source[i], source[index]) = (source[index], source[i]);
            }
            return source;
        }
        
        public T[] Shuffle<T>(params T[] source)
        {
            var count = source.Length;
            for (var i = 0; i < count; ++i)
            {
                var index = random.NextInt(i, count);
                (source[i], source[index]) = (source[index], source[i]);
            }
            return source;
        }
        
        public List<T> Shuffle<T>(List<T> source)
        {
            var count = source.Count;
            for (var i = 0; i < count; ++i)
            {
                var index = random.NextInt(i, count);
                (source[i], source[index]) = (source[index], source[i]);
            }
            return source;
        }
        
        public IList<T> Shuffle<T>(IList<T> source)
        {
            var count = source.Count;
            for (var i = 0; i < count; ++i)
            {
                var index = random.NextInt(i, count);
                (source[i], source[index]) = (source[index], source[i]);
            }
            return source;
        }

        #endregion

        #region SumWeight

        private int SumWeight(ReadOnlySpan<IWeight> weightTable)
        {
            if(weightTable.Length == 0) throw new ArgumentException("Weight table must contain at least one element.", nameof(weightTable));

            long sum = 0;
            foreach(var item in weightTable)
            {
                if(item == null) throw new ArgumentException("Weight table must not contain null elements.", nameof(weightTable));
                sum += ValidateWeight(item.Weight, nameof(weightTable));
            }

            return ValidateTotalWeight(sum, nameof(weightTable));
        }
        
        private int SumWeight<TWeight>(ReadOnlySpan<TWeight> weightTable) where TWeight : IWeight
        {
            if(weightTable.Length == 0) throw new ArgumentException("Weight table must contain at least one element.", nameof(weightTable));

            long sum = 0;
            foreach(var item in weightTable)
            {
                if(item is null) throw new ArgumentException("Weight table must not contain null elements.", nameof(weightTable));
                sum += ValidateWeight(item.Weight, nameof(weightTable));
            }

            return ValidateTotalWeight(sum, nameof(weightTable));
        }
        
        private int SumWeight(ReadOnlySpan<int> weightTable)
        {
            if(weightTable.Length == 0) throw new ArgumentException("Weight table must contain at least one element.", nameof(weightTable));

            long sum = 0;
            foreach(var weight in weightTable)
            {
                sum += ValidateWeight(weight, nameof(weightTable));
            }

            return ValidateTotalWeight(sum, nameof(weightTable));
        }

        private int ValidateWeight(int weight, string paramName)
        {
            if(weight < 0) throw new ArgumentOutOfRangeException(paramName, weight, "Weight values must be zero or greater.");
            return weight;
        }

        private int ValidateTotalWeight(long totalWeight, string paramName)
        {
            if(totalWeight == 0) throw new ArgumentException("At least one weight must be greater than zero.", paramName);
            if(totalWeight > int.MaxValue) throw new ArgumentOutOfRangeException(paramName, totalWeight, "Total weight must be less than or equal to int.MaxValue.");
            return (int)totalWeight;
        }
        
        #endregion
    }
}
