using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.DataStructures
{
    /// <summary>
    /// Graph implementation for managing relationships between service requests
    /// Useful for showing related requests, dependencies, and patterns
    /// </summary>
    public class ServiceRequestGraph
    {
        private Dictionary<int, ServiceRequest> _requests;
        private Dictionary<int, List<int>> _adjacencyList;

        public ServiceRequestGraph()
        {
            _requests = new Dictionary<int, ServiceRequest>();
            _adjacencyList = new Dictionary<int, List<int>>();
        }

        /// <summary>
        /// Add a service request as a vertex
        /// </summary>
        public void AddRequest(ServiceRequest request)
        {
            if (!_requests.ContainsKey(request.Id))
            {
                _requests[request.Id] = request;
                _adjacencyList[request.Id] = new List<int>();
            }
        }

        /// <summary>
        /// Add a relationship/edge between two requests
        /// </summary>
        public void AddRelationship(int requestId1, int requestId2)
        {
            if (_requests.ContainsKey(requestId1) && _requests.ContainsKey(requestId2))
            {
                if (!_adjacencyList[requestId1].Contains(requestId2))
                    _adjacencyList[requestId1].Add(requestId2);

                if (!_adjacencyList[requestId2].Contains(requestId1))
                    _adjacencyList[requestId2].Add(requestId1);
            }
        }

        /// <summary>
        /// Get all related requests for a given request ID
        /// </summary>
        public List<ServiceRequest> GetRelatedRequests(int requestId)
        {
            var relatedRequests = new List<ServiceRequest>();

            if (_adjacencyList.ContainsKey(requestId))
            {
                foreach (var id in _adjacencyList[requestId])
                {
                    if (_requests.ContainsKey(id))
                        relatedRequests.Add(_requests[id]);
                }
            }

            return relatedRequests;
        }

        /// <summary>
        /// Breadth-First Search to find all connected requests
        /// </summary>
        public List<ServiceRequest> BFSTraversal(int startRequestId)
        {
            var result = new List<ServiceRequest>();
            if (!_requests.ContainsKey(startRequestId))
                return result;

            var visited = new HashSet<int>();
            var queue = new Queue<int>();

            visited.Add(startRequestId);
            queue.Enqueue(startRequestId);

            while (queue.Count > 0)
            {
                int currentId = queue.Dequeue();
                result.Add(_requests[currentId]);

                if (_adjacencyList.ContainsKey(currentId))
                {
                    foreach (var neighborId in _adjacencyList[currentId])
                    {
                        if (!visited.Contains(neighborId))
                        {
                            visited.Add(neighborId);
                            queue.Enqueue(neighborId);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Depth-First Search to find all connected requests
        /// </summary>
        public List<ServiceRequest> DFSTraversal(int startRequestId)
        {
            var result = new List<ServiceRequest>();
            if (!_requests.ContainsKey(startRequestId))
                return result;

            var visited = new HashSet<int>();
            DFSUtil(startRequestId, visited, result);
            return result;
        }

        private void DFSUtil(int requestId, HashSet<int> visited, List<ServiceRequest> result)
        {
            visited.Add(requestId);
            result.Add(_requests[requestId]);

            if (_adjacencyList.ContainsKey(requestId))
            {
                foreach (var neighborId in _adjacencyList[requestId])
                {
                    if (!visited.Contains(neighborId))
                    {
                        DFSUtil(neighborId, visited, result);
                    }
                }
            }
        }

        /// <summary>
        /// Find requests in the same location (potential pattern)
        /// </summary>
        public List<ServiceRequest> GetRequestsByLocation(string location)
        {
            return _requests.Values
                .Where(r => r.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Find requests in the same category
        /// </summary>
        public List<ServiceRequest> GetRequestsByCategory(string category)
        {
            return _requests.Values
                .Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Get clustering coefficient for pattern analysis
        /// </summary>
        public Dictionary<string, int> GetLocationClusters()
        {
            var clusters = new Dictionary<string, int>();

            foreach (var request in _requests.Values)
            {
                if (clusters.ContainsKey(request.Location))
                    clusters[request.Location]++;
                else
                    clusters[request.Location] = 1;
            }

            return clusters.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Get category distribution for analytics
        /// </summary>
        public Dictionary<string, int> GetCategoryClusters()
        {
            var clusters = new Dictionary<string, int>();

            foreach (var request in _requests.Values)
            {
                if (clusters.ContainsKey(request.Category))
                    clusters[request.Category]++;
                else
                    clusters[request.Category] = 1;
            }

            return clusters.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Minimum Spanning Tree using Prim's algorithm
        /// Useful for identifying core issues that affect multiple requests
        /// </summary>
        public List<(ServiceRequest, ServiceRequest)> GetMinimumSpanningTree()
        {
            var mst = new List<(ServiceRequest, ServiceRequest)>();
            if (_requests.Count == 0) return mst;

            var visited = new HashSet<int>();
            var priorityQueue = new PriorityQueue<(int, int, int), int>(); // (from, to, weight)

            // Start with first request
            int startId = _requests.Keys.First();
            visited.Add(startId);

            // Add all edges from start vertex
            if (_adjacencyList.ContainsKey(startId))
            {
                foreach (var neighborId in _adjacencyList[startId])
                {
                    int weight = CalculateWeight(startId, neighborId);
                    priorityQueue.Enqueue((startId, neighborId, weight), weight);
                }
            }

            // Process edges
            while (priorityQueue.Count > 0 && visited.Count < _requests.Count)
            {
                var (fromId, toId, weight) = priorityQueue.Dequeue();

                if (visited.Contains(toId))
                    continue;

                visited.Add(toId);
                mst.Add((_requests[fromId], _requests[toId]));

                // Add edges from newly added vertex
                if (_adjacencyList.ContainsKey(toId))
                {
                    foreach (var neighborId in _adjacencyList[toId])
                    {
                        if (!visited.Contains(neighborId))
                        {
                            int w = CalculateWeight(toId, neighborId);
                            priorityQueue.Enqueue((toId, neighborId, w), w);
                        }
                    }
                }
            }

            return mst;
        }

        private int CalculateWeight(int id1, int id2)
        {
            var req1 = _requests[id1];
            var req2 = _requests[id2];

            // Weight based on similarity (lower is better)
            int weight = 100;

            if (req1.Category == req2.Category) weight -= 30;
            if (req1.Location == req2.Location) weight -= 30;
            if (req1.Department == req2.Department) weight -= 20;
            if (req1.Priority == req2.Priority) weight -= 10;

            return weight;
        }

        /// <summary>
        /// Get all requests in the graph
        /// </summary>
        public List<ServiceRequest> GetAllRequests()
        {
            return _requests.Values.ToList();
        }

        /// <summary>
        /// Get connection count for a request
        /// </summary>
        public int GetConnectionCount(int requestId)
        {
            return _adjacencyList.ContainsKey(requestId) ? _adjacencyList[requestId].Count : 0;
        }
    }
}
