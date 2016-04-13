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

using NUnit.Framework;
using OsmSharp.Geo.Streams.GeoJson;
using OsmSharp.Math.Geo;
using OsmSharp.Routing.Osm;
using OsmSharp.Routing.Osm.Vehicles;
using System.Reflection;

namespace OsmSharp.Routing.Test.Functional.Regression
{
    /// <summary>
    /// Contains tests to prevent regression on the following issue:
    /// 
    /// - A route was found that has an incorrect duplicated segment in the OSM-based network in Hoeselt-Industrielaan.
    /// - Profile used was bicycle.
    /// - Non-contracted bidirectional dykstra.
    /// </summary>
    [TestFixture]
    class Case1RouteIndustrielaanHoeselt
    {
        /// <summary>
        /// Tests the default case of a route gone bad.
        /// </summary>
        [Test]
        public void Case1Test1()
        {
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "OsmSharp.Routing.Test.Functional.Regression.data.industrielaan-hoeselt.osm.pbf"), Vehicle.Bicycle);

            var router = new Router(routerDb);
            var route = router.Calculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(50.86143493652344, 5.49010705947876),
                new GeoCoordinate(50.85956954956055, 5.494085788726807));
            
            Assert.IsTrue(route.TotalDistance < 450);
        }
    }
}
