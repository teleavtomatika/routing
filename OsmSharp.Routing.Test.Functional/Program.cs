// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2016 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using OsmSharp.Math.Geo;
using OsmSharp.Osm.Streams;
using OsmSharp.Routing.Algorithms.Search;
using OsmSharp.Routing.Osm;
using System.IO;

namespace OsmSharp.Routing.Test.Functional
{
    class Program
    {
        static void Main(string[] args)
        {
            // enable logging.
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new ConsoleTraceListener());

            // create and register railway vehicle.
            var railwayVehicle = new RailwayVehicle();
            railwayVehicle.Register();

            // create router db.
            var routerDb = new RouterDb();

            // load the data.
            var target = new OsmSharp.Routing.Osm.Streams.RouterDbStreamTarget(routerDb, new Osm.Vehicles.Vehicle[] { railwayVehicle },
                normalizeTags: false); // IMPORTANT: disable tags normalization.
            target.RegisterSource(new OsmSharp.Osm.Xml.Streams.XmlOsmStreamSource(File.OpenRead("onetrack.osm")), false); // IMPORTANT: disable non-routing tags filter.
            target.Pull();

            // sort the network.
            routerDb.Network.Sort();

            // calculate route.
            var router = new Router(routerDb);
            var route = router.Calculate(railwayVehicle.Fastest(),
                new GeoCoordinate(51.04914774002, 3.64630507464),
                new GeoCoordinate(51.04798323263, 3.65157616572));
        }
    }
}
