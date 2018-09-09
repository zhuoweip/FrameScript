
namespace JacobGames.SuperInvoke.Implementation.Pool {

    internal class SingleTypePool<T> : ISingleTypePool
        where T : class, IPoolable, new() {

        private readonly PoolParams poolParams;
        private readonly EmptyNodesPool emptyNodesPool;
        private PoolNode head;

        private int count;
        
        public SingleTypePool(PoolParams poolParams) {

            this.poolParams = poolParams;
            emptyNodesPool = new EmptyNodesPool(poolParams);

            for (int i = 0; i < poolParams.PreloadedInstances; i++) {
                Push(Create());
            }
        }

        public object GetPooledInstance() {
            return count > 0
                ? Pop()
                : new T();
        }

        public void ReturnPooledInstance(object instance) {
            if (count < poolParams.Size || poolParams.Size < 0) {
                PoolNode node = emptyNodesPool.GetNode();
                node.Value = instance;
                Push(node);
            }
        }

        public void Clear() {
            head = null;
            emptyNodesPool.Clear();
        }

        private PoolNode Create() {
            PoolNode node = emptyNodesPool.GetNode();
            node.Value = new T();
            return node;
        }

        private void Push(PoolNode node) {

            node.Next = head;
            head = node;
            count++;
        }

        private T Pop() {

            PoolNode node = head;
            head = head.Next;
            node.Next = null;
            T value = (T) node.Value;
            emptyNodesPool.ReturnNode(node);
            count--;
            return value;
        }
    }
}