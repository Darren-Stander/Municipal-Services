using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Binary Search Tree node for storing service requests
    /// </summary>
  public class BSTNode
    {
        public ServiceRequest Data { get; set; }
  public BSTNode? Left { get; set; }
        public BSTNode? Right { get; set; }

     public BSTNode(ServiceRequest data)
   {
         Data = data;
     Left = null;
  Right = null;
        }
    }

    /// <summary>
    /// Binary Search Tree implementation for efficient service request searching and sorting
    /// </summary>
  public class BinarySearchTree
    {
        public BSTNode? Root { get; private set; }
        private int _count;

        public int Count => _count;

      /// <summary>
        /// Insert a service request into the BST (ordered by ID)
        /// </summary>
        public void Insert(ServiceRequest request)
      {
 Root = InsertRec(Root, request);
          _count++;
        }

        private BSTNode InsertRec(BSTNode? node, ServiceRequest request)
    {
    if (node == null)
            {
    return new BSTNode(request);
       }

 if (request.Id < node.Data.Id)
            {
      node.Left = InsertRec(node.Left, request);
            }
      else if (request.Id > node.Data.Id)
   {
       node.Right = InsertRec(node.Right, request);
}

            return node;
        }

        /// <summary>
 /// Search for a service request by ID
        /// </summary>
        public ServiceRequest? Search(int id)
        {
            return SearchRec(Root, id);
    }

      private ServiceRequest? SearchRec(BSTNode? node, int id)
     {
 if (node == null)
       return null;

            if (id == node.Data.Id)
    return node.Data;

            if (id < node.Data.Id)
                return SearchRec(node.Left, id);

            return SearchRec(node.Right, id);
        }

        /// <summary>
      /// Search for a service request by request number
        /// </summary>
     public ServiceRequest? SearchByRequestNumber(string requestNumber)
        {
            return SearchByRequestNumberRec(Root, requestNumber);
        }

        private ServiceRequest? SearchByRequestNumberRec(BSTNode? node, string requestNumber)
        {
            if (node == null)
         return null;

    if (node.Data.RequestNumber == requestNumber)
    return node.Data;

  var leftResult = SearchByRequestNumberRec(node.Left, requestNumber);
            if (leftResult != null)
   return leftResult;

          return SearchByRequestNumberRec(node.Right, requestNumber);
     }

  /// <summary>
        /// In-order traversal to get sorted list
    /// </summary>
     public List<ServiceRequest> InOrderTraversal()
{
            var result = new List<ServiceRequest>();
      InOrderTraversalRec(Root, result);
            return result;
        }

        private void InOrderTraversalRec(BSTNode? node, List<ServiceRequest> result)
  {
   if (node != null)
  {
            InOrderTraversalRec(node.Left, result);
       result.Add(node.Data);
          InOrderTraversalRec(node.Right, result);
      }
    }

 /// <summary>
        /// Get all requests in a specific status
        /// </summary>
        public List<ServiceRequest> GetRequestsByStatus(RequestStatus status)
 {
          var result = new List<ServiceRequest>();
  GetRequestsByStatusRec(Root, status, result);
   return result;
     }

        private void GetRequestsByStatusRec(BSTNode? node, RequestStatus status, List<ServiceRequest> result)
  {
       if (node != null)
          {
    GetRequestsByStatusRec(node.Left, status, result);
        if (node.Data.Status == status)
      {
        result.Add(node.Data);
    }
     GetRequestsByStatusRec(node.Right, status, result);
         }
        }

        /// <summary>
  /// Get all requests by priority
     /// </summary>
        public List<ServiceRequest> GetRequestsByPriority(RequestPriority priority)
        {
  var result = new List<ServiceRequest>();
     GetRequestsByPriorityRec(Root, priority, result);
            return result;
        }

        private void GetRequestsByPriorityRec(BSTNode? node, RequestPriority priority, List<ServiceRequest> result)
        {
            if (node != null)
      {
     GetRequestsByPriorityRec(node.Left, priority, result);
    if (node.Data.Priority == priority)
            {
       result.Add(node.Data);
    }
  GetRequestsByPriorityRec(node.Right, priority, result);
            }
        }
    }
}
