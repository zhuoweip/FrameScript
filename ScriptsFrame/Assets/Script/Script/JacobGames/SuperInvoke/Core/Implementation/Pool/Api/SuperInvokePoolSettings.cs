using JacobGames.SuperInvoke.Implementation.Runnable;

namespace JacobGames.SuperInvoke.Implementation.Pool {
    internal static class SuperInvokePoolSettings {

        private static readonly PoolParams DEFAULT_POOL_PARAMS = new PoolParams() {Size = -1};
        
        public static PoolManager PoolManager { get; set; }

        static SuperInvokePoolSettings() {
            PoolManager = new PoolManager();

            PoolManager.CreatePool<SingleTask>(DEFAULT_POOL_PARAMS);
            PoolManager.CreatePool<Sequence>(DEFAULT_POOL_PARAMS);
        }
    }
}
