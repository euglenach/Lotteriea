using System;
using System.Collections.Generic;

namespace LotterySystem
{
    public static class Lotteriea
    {
        private static LotterieaBox core;

        static Lotteriea()
        {
            var ticks = DateTime.UtcNow.Ticks;
            var seed = (uint)((ticks >> 32) ^ ticks);
            if (seed == 0) seed = 1;
            core = new(seed);
        }

        public static void InitState(uint seed = 0x6E624EB7u)
        {
            core.InitState(seed);
        }

        public static bool True(float trueProbability, RandomType randomType = RandomType.Ratio) => core.True(trueProbability, randomType);

        public static bool False(float falseProbability, RandomType randomType = RandomType.Ratio) => core.False(falseProbability, randomType);
        
        public static T Select<T>(IEnumerable<T> source) => core.Select(source);

        public static T Select<T>(params T[] source) => core.Select(source);
        
        public static T Select<T>(ReadOnlySpan<T> source) => core.Select(source);
        public static T Select<T>(IReadOnlyList<T> source) => core.Select(source);

        public static T Select<T>(T item1, T item2) => core.Select(item1, item2);

        public static ref T Select<T>(ref T[] source) => ref core.Select(ref source);

        public static ref T Select<T>(ref T item1, ref T item2) => ref core.Select(ref item1, ref item2);

        public static int WeightLotteryIndex(params int[] weightTable) => core.WeightLotteryIndex(weightTable);
        
        public static int WeightLotteryIndex(ReadOnlySpan<int> weightTable) => core.WeightLotteryIndex(weightTable);

        public static int WeightLotteryIndex(params IWeight[] weightTable) => core.WeightLotteryIndex(weightTable);
        
        public static int WeightLotteryIndex<TWeight>(params TWeight[] weightTable) where TWeight : IWeight => core.WeightLotteryIndex(weightTable);
        
        public static int WeightLotteryIndex(ReadOnlySpan<IWeight> weightTable) => core.WeightLotteryIndex(weightTable);
        public static int WeightLotteryIndex<TWeight>(ReadOnlySpan<TWeight> weightTable) where TWeight : IWeight => core.WeightLotteryIndex(weightTable);
        
        public static IWeight WeightLottery(params IWeight[] weightTable) => core.WeightLottery(weightTable);
        
        public static TWeight WeightLottery<TWeight>(params TWeight[] weightTable) where TWeight : IWeight => core.WeightLottery(weightTable);
        
        public static IWeight WeightLottery(ReadOnlySpan<IWeight> weightTable) => core.WeightLottery(weightTable);
        
        public static TWeight WeightLottery<TWeight>(ReadOnlySpan<TWeight> weightTable) where TWeight : IWeight => core.WeightLottery(weightTable);
        
        public static ref IWeight WeightLotteryRef(IWeight[] weightTable) => ref core.WeightLotteryRef(weightTable);
        
        public static ref TWeight WeightLotteryRef<TWeight>(TWeight[] weightTable) where TWeight : IWeight => ref core.WeightLotteryRef(weightTable);
        
        public static Span<T> Shuffle<T>(Span<T> source) => core.Shuffle(source);
        public static T[] Shuffle<T>(params T[] source) => core.Shuffle(source);
        public static List<T> Shuffle<T>(List<T> source) => core.Shuffle(source);
        public static IList<T> Shuffle<T>(IList<T> source) => core.Shuffle(source);
    }
}
