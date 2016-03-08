


using System;
using OsmSharp.Collections.Tags;
using OsmSharp.Routing.Profiles;
using OsmSharp.Units.Speed;

namespace OsmSharp.Routing.Test.Functional
{
    public class RailwayVehicle : Osm.Vehicles.Vehicle
    {
        public override string UniqueName
        {
            get
            {
                return "RailwayVehicle";
            }
        }

        public override KilometerPerHour MaxSpeed()
        {
            return 500;
        }

        public override KilometerPerHour MaxSpeedAllowed(string highwayType)
        {
            return 500;
        }

        public override KilometerPerHour MinSpeed()
        {
            return 5;
        }

        public override bool CanStopOn(TagsCollectionBase tags)
        {
            return tags.ContainsKey("railway");
        }

        public override bool? IsOneWay(TagsCollectionBase tags)
        {
            return null;
        }

        public override bool CanTraverse(TagsCollectionBase tags)
        {
            return tags.ContainsKey("railway");
        }

        public override bool IsRelevant(string key)
        {
            return key == "railway";
        }

        public override bool IsRelevantForProfile(string key)
        {
            return key == "railway";
        }

        public override bool IsRelevantForMeta(string key)
        {
            return false;
        }

        public override bool IsEqualFor(TagsCollectionBase tags1, TagsCollectionBase tags2)
        {
            var railway =  string.Empty;
            if (tags1.TryGetValue("railway", out railway))
            {
                return tags2.ContainsKeyValue("railway", railway);
            }
            return false;
        }

        public override KilometerPerHour ProbableSpeed(TagsCollectionBase tags)
        {
            return 100;
        }

        public override KilometerPerHour MaxSpeedAllowed(TagsCollectionBase tags)
        {
            return 500;
        }

        protected override bool IsVehicleAllowed(TagsCollectionBase tags, string highwayType)
        {
            return true;
        }
    }
}
