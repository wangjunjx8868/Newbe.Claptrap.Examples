﻿using System.Linq;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.Repository
{
    public class TrainInfoRepository : ITrainInfoRepository
    {
        public Task<int[]> GetStationsAsync(int trainId)
        {
            var re = DataSource.TrainStations[trainId];
            return Task.FromResult(re);
        }

        public Task<int[]> GetTrainsAsync(int fromStationId, int toStationId)
        {
            var fromTrains = DataSource.StationTrains[fromStationId];
            var toTrains = DataSource.StationTrains[toStationId];
            var trainIds = fromTrains.Concat(toTrains).ToHashSet();
            var re = DataSource.TrainStations
                .Where(kv => trainIds.Contains(kv.Key))
                .Where(kv =>
                {
                    var stations = kv.Value;
                    var matchFrom = false;
                    var matchTo = false;
                    foreach (var station in stations)
                    {
                        if (!matchFrom && station == fromStationId)
                        {
                            matchFrom = true;
                        }

                        if (matchFrom && station == toStationId)
                        {
                            matchTo = true;
                            break;
                        }
                    }

                    return matchTo;
                })
                .Select(x => x.Key)
                .ToArray();
            return Task.FromResult(re);
        }
    }
}