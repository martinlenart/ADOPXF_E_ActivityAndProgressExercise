using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using ActivityAndProgress.Models;

namespace ActivityAndProgress.Services
{
    public class PrimeNumberService
    {
        public Task<List<PrimeBatch>> GetPrimeBatchCountsAsync(int NrOfBatches) => GetPrimeBatchCountsAsync(NrOfBatches, null);
        public Task<List<PrimeBatch>> GetPrimeBatchCountsAsync(int NrOfBatches, IProgress<float> onProgressReporting)
        {
            return Task.Run(async () =>
            {
                var batchList = new List<PrimeBatch>();
                for (int i = 0; i < NrOfBatches; i++)
                {
                    var batch = new PrimeBatch { BatchStart = i * PrimeBatch.BatchSize + 2 };
                    batch.NrPrimes = await GetPrimesCountAsync(batch.BatchStart, PrimeBatch.BatchSize);
                    batchList.Add(batch);

                    float fProgress = ((float)i + 1) / NrOfBatches;
                    onProgressReporting?.Report(fProgress);
                }
                return batchList;
            });
        }
        public Task DisplayPrimeCountsAsync(int NrOfBatches, IProgress<(float, string)> onProgressReporting)
        {
            //Notice I can use async in Lambda Expression
            return Task.Run(async () =>
            {
                for (int i = 0; i < NrOfBatches; i++)
                {
                    float k = i+1;
                    int nrprimes = await GetPrimesCountAsync(i * 1000000 + 2, 1000000);
                    onProgressReporting.Report((k/NrOfBatches, $"{nrprimes:N0} primes between {(i * 1000000):N0} and {((i + 1) * 1000000 - 1):N0}"));
                }
            });
        }

        public Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
               ParallelEnumerable.Range(start, count).Count(n =>
                 Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        public Task<List<int>> GetPrimesAsync(int start, int count)
        {
            return Task.Run(() =>
               ParallelEnumerable.Range(start, count).Where(n =>
                 Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)).ToList<int>());
        }
    }
}
