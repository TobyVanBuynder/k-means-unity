using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;


public static class KMeans
{
    public enum Dimensions{ TWO, THREE }

    // TODO: move these to parameters
    private static int MAX_ITERATIONS{ get{ return 20; }}
    private static float MAX_ERROR_RATE{ get{ return 0.3f; }}

    // TODO: update Naive with PlusPlus improvements
    public static (float[] distributions, float errorRate, int iterations) Naive(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters, Dimensions dimensions = Dimensions.THREE)
    {
        // Pre-create needed variables
        List<Vector3> centroids = new List<Vector3>(numClusters);
        float[] distributions = new float[numClusters];
        float errorRate = 0f;

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

        int it = 0;
        do
        {
            it++;
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
                float minDistance = Mathf.Infinity;

                // Loop over all initial cluster points
                for (int c = 0; c < numClusters; c++)
                {
                    // Calculate the distance between the curently selected point and tbe currently selected centroid
                    Vector3 distanceVector = centroids[c] - currentDataPoint.position;

                    // If there's two dimensions, ignore Y-value
                    if (dimensions == Dimensions.TWO)
                        distanceVector.y = 0;

                    // If the distance is shorter than the previous minimum distance
                    if (distanceVector.magnitude < minDistance)
                    {
                        // If there was already an assigned cluster, remove it from that one
                        if (assignedCluster > -1)
                            finalClusters[assignedCluster].Remove(currentDataPoint);

                        // Assign it to the new cluster
                        finalClusters[c].Add(currentDataPoint);

                        // Set new tracking variables
                        assignedCluster = c;
                        minDistance = distanceVector.magnitude;
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
                {
                    distributions[c] += Vector3.Distance(tf.position, currentCentroid);
                }
                distributions[c] /= finalClusters[c].Count;
            }

            // Calculate average error rate of all clusters combined
            for (int d = 0; d < numClusters - 1; d++)
            {
                errorRate += Mathf.Abs(distributions[d] - distributions[d+1]);
            }
            errorRate += Mathf.Abs(distributions[0] - distributions[numClusters-1]);
            errorRate = Mathf.Sqrt(errorRate / numClusters);
        } while ((errorRate > 1.7f) && (it < 5));

        return (distributions, errorRate, it);
    }

    // Optimized initialization method 
    // https://www.geeksforgeeks.org/ml-k-means-algorithm/
    // http://ilpubs.stanford.edu:8090/778/1/2006-13.pdf
    public static (float[] distributions, float errorRate, int iterations) PlusPlus(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters, Dimensions dimensions = Dimensions.THREE, int initialRandomIndex = -1)
    {
        // Pre-create needed variables
        List<Vector3> centroids = new List<Vector3>(numClusters);
        int[] randomIndices = new int[numClusters];
        bool isChosenAlready = false;
        bool reiterate = false;
        float[] distributions = new float[numClusters];
        float avgExpectedDistribution = 1 / numClusters;
        float avgExpectedDistributionCount = dataSet.Count / numClusters;
        float errorRate = 0f;
        int iterations = 0;

        do
        {
            // If we reiterate, clear initial setup and go again
            if (reiterate)
            {
                initialRandomIndex = -1;
                centroids.Clear();
            }

            reiterate = false;
            iterations++;
            errorRate = 0f;

            // ----- Start K-Means++ -----
            randomIndices[0] = initialRandomIndex;

            // Randomly select the first cluster
            // If it isn't pregenerated yet, generate one
            if(randomIndices[0] < 0) randomIndices[0] = Random.Range(0, dataSet.Count);
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
                    isChosenAlready = false;
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
                        float dist = Vector3.Distance(dataSet[prevChosenIndex].position, dataSet[d].position);
                        if (dist > furthestDistance)
                        {
                            randomIndices[c] = d;
                            furthestDistance = dist;
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
                if(finalClusters[l].Count > 0) finalClusters[l].RemoveAll((tf) => true);
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

                    // If the distance is shorter than the previous minimum distance
                    if (distanceVector.magnitude < minDistance)
                    {
                        // Set new assigned cluster index and minimum distance
                        assignedCluster = c;
                        minDistance = distanceVector.magnitude;
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
                {
                    currentCentroid += currentCluster[t].position;
                }
                centroids[c] = currentCentroid / currentCluster.Count;

                // Calculate error of distribution  per cluster
                distributions[c] = (avgExpectedDistributionCount - currentCluster.Count) / currentCluster.Count;
            }

            // Calculate average error rate of all clusters combined
            for (int d = 0; d < distributions.Length - 1; d++)
            {
                errorRate += Mathf.Abs(distributions[d] - avgExpectedDistribution);
            }
            errorRate += Mathf.Abs(distributions[0] - avgExpectedDistribution);
            
            // Make sure the error is low enough
            reiterate = errorRate > MAX_ERROR_RATE && iterations < MAX_ITERATIONS;

        } while (reiterate);

        return (distributions, errorRate, iterations);
    }
}