using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Min Heap implementation for managing service requests by days open
    /// Allows quick access to the oldest unresolved requests
    /// </summary>
 public class ServiceRequestMinHeap
    {
        private List<ServiceRequest> _heap;

        public int Count => _heap.Count;

 public ServiceRequestMinHeap()
 {
     _heap = new List<ServiceRequest>();
  }

/// <summary>
        /// Insert a service request into the heap
        /// </summary>
        public void Insert(ServiceRequest request)
        {
    _heap.Add(request);
       HeapifyUp(_heap.Count - 1);
      }

   /// <summary>
   /// Extract the request with minimum days open (most recent)
        /// </summary>
        public ServiceRequest? ExtractMin()
    {
     if (_heap.Count == 0)
  return null;

            ServiceRequest min = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

      if (_heap.Count > 0)
                HeapifyDown(0);

   return min;
        }

     /// <summary>
        /// Peek at the minimum element without removing it
        /// </summary>
 public ServiceRequest? Peek()
 {
            return _heap.Count > 0 ? _heap[0] : null;
        }

        /// <summary>
        /// Get all elements sorted by days open
        /// </summary>
        public List<ServiceRequest> GetAllSorted()
        {
         var sorted = new List<ServiceRequest>(_heap);
   sorted.Sort((a, b) => a.DaysOpen.CompareTo(b.DaysOpen));
            return sorted;
        }

        private void HeapifyUp(int index)
{
      while (index > 0)
            {
       int parentIndex = (index - 1) / 2;

      if (_heap[index].DaysOpen >= _heap[parentIndex].DaysOpen)
      break;

          Swap(index, parentIndex);
       index = parentIndex;
    }
        }

        private void HeapifyDown(int index)
 {
 while (true)
            {
     int leftChild = 2 * index + 1;
         int rightChild = 2 * index + 2;
          int smallest = index;

if (leftChild < _heap.Count && _heap[leftChild].DaysOpen < _heap[smallest].DaysOpen)
    smallest = leftChild;

            if (rightChild < _heap.Count && _heap[rightChild].DaysOpen < _heap[smallest].DaysOpen)
        smallest = rightChild;

        if (smallest == index)
         break;

      Swap(index, smallest);
            index = smallest;
         }
   }

        private void Swap(int i, int j)
        {
    var temp = _heap[i];
            _heap[i] = _heap[j];
      _heap[j] = temp;
   }

        /// <summary>
        /// Clear all elements from the heap
        /// </summary>
      public void Clear()
        {
     _heap.Clear();
        }
    }

/// <summary>
    /// Max Heap implementation for managing service requests by priority
    /// Allows quick access to the highest priority requests
    /// </summary>
    public class ServiceRequestMaxHeap
    {
      private List<ServiceRequest> _heap;

 public int Count => _heap.Count;

        public ServiceRequestMaxHeap()
        {
  _heap = new List<ServiceRequest>();
        }

        /// <summary>
        /// Insert a service request into the heap
   /// </summary>
        public void Insert(ServiceRequest request)
        {
            _heap.Add(request);
HeapifyUp(_heap.Count - 1);
        }

        /// <summary>
        /// Extract the request with maximum priority
        /// </summary>
        public ServiceRequest? ExtractMax()
     {
            if (_heap.Count == 0)
        return null;

  ServiceRequest max = _heap[0];
  _heap[0] = _heap[_heap.Count - 1];
      _heap.RemoveAt(_heap.Count - 1);

    if (_heap.Count > 0)
       HeapifyDown(0);

   return max;
        }

    /// <summary>
        /// Peek at the maximum element without removing it
  /// </summary>
   public ServiceRequest? Peek()
        {
  return _heap.Count > 0 ? _heap[0] : null;
        }

        /// <summary>
        /// Get all elements sorted by priority (highest first)
      /// </summary>
 public List<ServiceRequest> GetAllSorted()
        {
   var sorted = new List<ServiceRequest>(_heap);
    sorted.Sort((a, b) => b.Priority.CompareTo(a.Priority));
          return sorted;
 }

        private void HeapifyUp(int index)
        {
            while (index > 0)
        {
   int parentIndex = (index - 1) / 2;

                if (_heap[index].Priority <= _heap[parentIndex].Priority)
        break;

                Swap(index, parentIndex);
    index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
      while (true)
            {
     int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
   int largest = index;

              if (leftChild < _heap.Count && _heap[leftChild].Priority > _heap[largest].Priority)
           largest = leftChild;

      if (rightChild < _heap.Count && _heap[rightChild].Priority > _heap[largest].Priority)
              largest = rightChild;

          if (largest == index)
 break;

       Swap(index, largest);
    index = largest;
     }
        }

        private void Swap(int i, int j)
{
            var temp = _heap[i];
            _heap[i] = _heap[j];
       _heap[j] = temp;
    }

     /// <summary>
        /// Clear all elements from the heap
    /// </summary>
  public void Clear()
        {
     _heap.Clear();
        }
    }
}
