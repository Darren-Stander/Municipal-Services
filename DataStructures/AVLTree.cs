using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
  /// <summary>
    /// AVL Tree node with height tracking
    /// </summary>
    public class AVLNode
    {
   public ServiceRequest Data { get; set; }
  public AVLNode? Left { get; set; }
        public AVLNode? Right { get; set; }
        public int Height { get; set; }

        public AVLNode(ServiceRequest data)
        {
      Data = data;
       Height = 1;
        }
    }

    /// <summary>
    /// Self-balancing AVL Tree for efficient operations
    /// Balanced by priority for quick priority-based retrieval
    /// </summary>
    public class AVLTree
    {
        public AVLNode? Root { get; private set; }
        private int _count;

        public int Count => _count;

        private int GetHeight(AVLNode? node)
     {
     return node?.Height ?? 0;
      }

        private int GetBalance(AVLNode? node)
        {
          return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

  private AVLNode RightRotate(AVLNode y)
        {
            AVLNode x = y.Left!;
AVLNode? T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

 return x;
        }

        private AVLNode LeftRotate(AVLNode x)
        {
    AVLNode y = x.Right!;
            AVLNode? T2 = y.Left;

       y.Left = x;
    x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
 y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

    /// <summary>
  /// Insert a service request (ordered by priority then by ID)
        /// </summary>
        public void Insert(ServiceRequest request)
     {
        Root = InsertRec(Root, request);
      _count++;
}

        private AVLNode InsertRec(AVLNode? node, ServiceRequest request)
 {
            if (node == null)
          return new AVLNode(request);

       // Compare by priority first, then by ID
            int comparison = CompareRequests(request, node.Data);

       if (comparison < 0)
  node.Left = InsertRec(node.Left, request);
            else if (comparison > 0)
    node.Right = InsertRec(node.Right, request);
       else
  return node; // Duplicate

// Update height
 node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

    // Get balance factor
         int balance = GetBalance(node);

            // Left Left Case
   if (balance > 1 && CompareRequests(request, node.Left!.Data) < 0)
        return RightRotate(node);

      // Right Right Case
         if (balance < -1 && CompareRequests(request, node.Right!.Data) > 0)
   return LeftRotate(node);

      // Left Right Case
            if (balance > 1 && CompareRequests(request, node.Left!.Data) > 0)
          {
           node.Left = LeftRotate(node.Left);
          return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && CompareRequests(request, node.Right!.Data) < 0)
{
                node.Right = RightRotate(node.Right);
     return LeftRotate(node);
            }

    return node;
      }

        private int CompareRequests(ServiceRequest a, ServiceRequest b)
        {
       // Higher priority comes first (descending)
            int priorityCompare = b.Priority.CompareTo(a.Priority);
      if (priorityCompare != 0)
        return priorityCompare;

     // Then by ID (ascending)
       return a.Id.CompareTo(b.Id);
      }

        /// <summary>
        /// Get all requests in priority order
   /// </summary>
        public List<ServiceRequest> InOrderTraversal()
        {
            var result = new List<ServiceRequest>();
            InOrderTraversalRec(Root, result);
            return result;
        }

        private void InOrderTraversalRec(AVLNode? node, List<ServiceRequest> result)
    {
      if (node != null)
            {
             InOrderTraversalRec(node.Left, result);
      result.Add(node.Data);
                InOrderTraversalRec(node.Right, result);
            }
        }

        /// <summary>
        /// Get top N highest priority requests
        /// </summary>
        public List<ServiceRequest> GetTopPriorityRequests(int n)
        {
     var result = new List<ServiceRequest>();
 GetTopPriorityRequestsRec(Root, n, result);
         return result;
   }

        private void GetTopPriorityRequestsRec(AVLNode? node, int n, List<ServiceRequest> result)
        {
            if (node == null || result.Count >= n)
          return;

         // Traverse in order (highest priority first due to our comparison)
    GetTopPriorityRequestsRec(node.Left, n, result);
   
            if (result.Count < n)
        result.Add(node.Data);

      GetTopPriorityRequestsRec(node.Right, n, result);
   }
    }
}
