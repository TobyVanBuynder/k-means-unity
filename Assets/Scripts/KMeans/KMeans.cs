using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Runtime.CompilerServices;


public static class KMeans
{
    public readonly struct Stats
    {
        public readonly float[] Distributions{ get; }
        public readonly float ErrorRate{ get; }
        public readonly int NumIterations{ get; }

        public Stats(float[] distributions, float errorRate, int numIterations)
        {
            Distributions = distributions;
            ErrorRate = errorRate;
            NumIterations = numIterations;
        }

        public override string ToString()
        {
            return $"Distributions: {Distributions},\nError Rate: {ErrorRate},\nNumIterations: {NumIterations}";
        }
    }

    public enum Dimensions{ TWO = 0, THREE = 1 }

    // TODO: update Naive with PlusPlus improvements
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stats Naive(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters, Dimensions dimensions = Dimensions.THREE, int maxIterations = 20, float maxErrorRate = 1.5f)
    {
        // Pre-create needed variables
        List<Vector3> centroids = new List<Vector3>(numClusters);
        float[] distributions = new float[numClusters];
        float errorRate;

        // Randomly select numClusters amount of data points from the data set
        using (RNGUnique uniqueRandom = new RNGUnique(numClusters))
        {
            HashSet<int> randomIndices = uniqueRandom.Generate(numClusters, 0, dataSet.Count);

            IEnumerator<int> enumerator = randomIndices.GetEnumerator();

            // Assign them as the starting centroids
            while (enumerator.MoveNext())
            {
                int randIndex = enumerator.Current;
                centroids.Add(dataSet[randIndex].position);
            }

            enumerator.Dispose();
        }

        int numIterations = 0;
        do
        {
            numIterations++;
            errorRate = 0f;

            // Make sure the clusters are empty
            foreach(List<Transform> tfList in finalClusters)
            {
                if(tfList.Count > 0) tfList.Clear();
            }

            // Start comparing distances per other data point in the set, excluding the preselected data points
            for (int d = 0; d < dataSet.Count; d++)
            {
                // Tracking variables
                Transform currentDataPoint = dataSet[d];
                int assignedCluster = -1;
                float leastDistance = Mathf.Infinity;

                // Loop over all initial cluster points
                for (int c = 0; c < numClusters; c++)
                {
                    // Calculate the distance between the curently selected point and tbe currently selected centroid
                    Vector3 distanceVector = centroids[c] - currentDataPoint.position;

                    // If there's two dimensions, ignore Y-value
                    distanceVector.Set(distanceVector.x, (int)dimensions * distanceVector.y, distanceVector.z);

                    float distance = distanceVector.sqrMagnitude;

                    // If the distance is shorter than the previous minimum distance
                    if (distance < leastDistance)
                    {
                        // If there was already an assigned cluster, remove it from that one
                        if (assignedCluster > -1)
                            finalClusters[assignedCluster].Remove(currentDataPoint);

                        // Assign it to the new cluster
                        finalClusters[c].Add(currentDataPoint);

                        // Set new tracking variables
                        assignedCluster = c;
                        leastDistance = distance;
                    }
                }
            }

            Vector3 currentCentroid;
            // Recalculate centroid per cluster
            for (int c = 0; c < centroids.Count; c++)
            {
                currentCentroid = Vector3.zero;

                // Calculate average position of cluster members
                foreach (Transform tf in finalClusters[c])
                    currentCentroid += tf.position;
                currentCentroid /= finalClusters[c].Count;

                centroids[c] = currentCentroid;

                // Calculate dot squared distance distribution average per cluster
                distributions[c] = 0;
                foreach (Transform tf in finalClusters[c])
                    distributions[c] += (tf.position - currentCentroid).sqrMagnitude;
                distributions[c] /= finalClusters[c].Count;
            }

            // Calculate average error rate of all clusters combined
            for (int d = 0; d < numClusters - 1; d++)
                errorRate += Mathf.Abs(distributions[d] - distributions[d+1]);
            errorRate += Mathf.Abs(distributions[0] - distributions[numClusters-1]);
            errorRate = Mathf.Sqrt(errorRate / numClusters);
        } while ((errorRate > maxErrorRate) && (numIterations < maxIterations));

        return new Stats(distributions, errorRate, numIterations);
    }

    // Optimized initialization method 
    // https://www.geeksforgeeks.org/ml-k-means-algorithm/
    // http://ilpubs.stanford.edu:8090/778/1/2006-13.pdf
    // Much more accurate at the cost of extra power
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stats PlusPlus(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters, Dimensions dimensions = Dimensions.THREE, int maxIterations = 20, float maxErrorRate = 0.3f, int initialRandomIndex = -1)
    {
        // Pre-create needed variables
        List<Vector3> centroids = new List<Vector3>(numClusters);
        int[] randomIndices = new int[numClusters];
        bool reiterate = false;
        float[] distributions = new float[numClusters];
        float avgExpectedDistribution = 1 / numClusters;
        float avgExpectedDistributionCount = dataSet.Count / numClusters;
        int numIterations = 0;
        float errorRate;

        do
        {
            // If we reiterate, clear initial setup and go again
            if (reiterate)
            {
                initialRandomIndex = -1;
                centroids.Clear();
            }

            reiterate = false;
            numIterations++;
            errorRate = 0f;

            // ----- Start K-Means++ -----
            randomIndices[0] = initialRandomIndex;

            // Randomly select the first cluster
            // If it isn't pregenerated yet, generate one
            if(randomIndices[0] < 0)
                randomIndices[0] = Random.Range(0, dataSet.Count);
            centroids.Add(dataSet[randomIndices[0]].position);

            // For each remaining cluster to be chosen,
            // calculate the furthest distance for the
            // remaining data points, to be chosen as
            // the next cluster.
            // 
            // Check the previous clusters for the distance
            // away from every remaining data point until
            // there are no more clusters left to be chosen

            // For each remaining cluster
            float furthestDistance;
            for (int c = 1; c < numClusters; c++)
            {
                furthestDistance = 0f;

                // Check each data point
                for(int d = 0; d < dataSet.Count; d++)
                {
                    // Check if this data point has already been chosen
                    bool isChosenAlready = false;
                    for(int p = 0; p < c; p++)
                    {
                        if (d == randomIndices[p] && !isChosenAlready)
                            isChosenAlready = true;
                    }
                    if (isChosenAlready) continue;

                    // If not, calculate the furthest squared distance from the remaining clusters
                    // for this data point
                    for(int p = 0; p < c; p++)
                    {
                        int prevChosenIndex = randomIndices[p];

                        Vector3 distanceVector = dataSet[prevChosenIndex].position - dataSet[d].position;

                        // If there's two dimensions, ignore Y-value
                        distanceVector.Set(distanceVector.x, (int)dimensions * distanceVector.y, distanceVector.z);

                        float distance = distanceVector.sqrMagnitude;

                        if (distance > furthestDistance)
                        {
                            randomIndices[c] = d;
                            furthestDistance = distance;
                        }
                    }
                }

                centroids.Add(dataSet[randomIndices[c]].position);
            }
            // ------ End K-means++ ------

            // Do the rest of the K-means algorithm

            // Make sure the clusters are empty
            for(int l = 0; l < numClusters; l++)
            {
                if(finalClusters[l].Count > 0) finalClusters[l].Clear();
            }

            // Start comparing distances per other data point in the set, excluding the preselected data points
            for (int d = 0; d < dataSet.Count; d++)
            {
                // Tracking variables
                Transform currentDataPoint = dataSet[d];
                int assignedCluster = -1;
                float minDistance = Mathf.Infinity;

                // Loop over all initial cluster points
                for (int c = 0; c < centroids.Count; c++)
                {
                    // Calculate the distance between the curently selected point and tbe currently selected centroid
                    Vector3 distanceVector = centroids[c] - currentDataPoint.position;

                    // If there's two dimensions, ignore Y-value
                    distanceVector.Set(distanceVector.x, (int)dimensions * distanceVector.y, distanceVector.z);

                    float distance = distanceVector.sqrMagnitude;

                    // If the distance is shorter than the previous minimum distance
                    if (distance < minDistance)
                    {
                        // Set new assigned cluster index and minimum distance
                        assignedCluster = c;
                        minDistance = distance;
                    }
                }
                
                // Assign it to the new cluster
                finalClusters[assignedCluster].Add(currentDataPoint);
            }

            Vector3 currentCentroid;
            // Recalculate centroid per cluster
            for (int c = 0; c < centroids.Count; c++)
            {
                List<Transform> currentCluster = finalClusters[c];

                // Calculate average position of cluster members
                currentCentroid = Vector3.zero;
                for(int t = 0; t < currentCluster.Count; t++)
                    currentCentroid += currentCluster[t].position;
                centroids[c] = currentCentroid / currentCluster.Count;

                // Calculate error of distribution  per cluster
                distributions[c] = (avgExpectedDistributionCount - currentCluster.Count) / currentCluster.Count;
            }

            // Calculate average error rate of all clusters combined
            for (int d = 0; d < distributions.Length - 1; d++)
                errorRate += Mathf.Abs(distributions[d] - avgExpectedDistribution);
            errorRate += Mathf.Abs(distributions[0] - avgExpectedDistribution);
            
            // Make sure the error is low enough
            reiterate = errorRate > maxErrorRate && numIterations < maxIterations;

        } while (reiterate);

        return new Stats(distributions, errorRate, numIterations);
    }
}