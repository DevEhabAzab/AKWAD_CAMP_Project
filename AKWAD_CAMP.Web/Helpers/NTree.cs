namespace AKWAD_CAMP.Web.Helpers
{
    public delegate void TreeVisitor<T>(T nodeData);

    public delegate void TreeChildPrinter<T>(List<T> nodeData,T parent);
    
    public delegate bool IsNode<T>(NTree<T> nodeData);

    public class NTree<T>
    {
        private T data;
        private List<NTree<T>> children;
        public T Data { get
            {
                return data;
            } }  
        public List<NTree<T>> Children
        { get
            {
                return children;
            } }

        public NTree(T data)
        {
            this.data = data;
            children = new List<NTree<T>>();
        }

        public void AddChild(T data)
        {
            children.Add(new NTree<T>(data));
        }
        public NTree<T> ReplaceChild(NTree<T> node ,IsNode<T> action)
        {
            foreach(var n in children)
            {
                if (action(n))
                {
                    n.data = node.Data;
                    return n;
                }
            }
           
                
                
            return null;
        }
        public NTree<T> GetChild(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0)
                    return n;
            return null;
        }

        public void Traverse(TreeVisitor<T> visitor , TreeChildPrinter<T> printer)
        {
            visitor(this.data);
            foreach (NTree<T> kid in this.children)
                kid.Traverse(visitor,printer);
        if(this.children is not null &&  this.children.Any())
           printer( this.children.Select(x => x.data).ToList(),this.data);

        }
       
    }
}
