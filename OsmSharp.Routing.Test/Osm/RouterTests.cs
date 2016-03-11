// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2015 Abelshausen Ben
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
using OsmSharp.Osm.Streams;
using OsmSharp.Routing.Osm;
using OsmSharp.Collections.Tags;
using OsmSharp.Osm;
using System.Collections.Generic;
using OsmSharp.Routing.Osm.Vehicles;
using OsmSharp.Math.Geo;

namespace OsmSharp.Routing.Test.Osm
{
    /// <summary>
    /// Contains a series of regression tests.
    /// </summary>
    [TestFixture]
    public class RouterTests
    {
        /// <summary>
        /// An integration test that loads one way with the oneway tags but bicycle allowed in two directions.
        /// </summary>
        [Test]
        public void TestOnewayBicycleNo()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"),
                        Tag.Create("oneway", "yes"),
                        Tag.Create("oneway:bicycle", "no"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle);

            // test some routes.
            var router = new Router(routerDb);

            // confirm oneway is working for cars.
            var route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057), 
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsTrue(route.IsError);
            
            // confirm oneway:bicycle=no is working for bicycles.
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }

        /// <summary>
        /// An integration test that loads one way with access=no and bicycle=yes.
        /// </summary>
        [Test]
        public void TestBicycleYesAccessNo()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"),
                        Tag.Create("access", "no"),
                        Tag.Create("bicycle", "yes"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle);

            // test some routes.
            var router = new Router(routerDb);

            // confirm access=no is working for cars.
            var route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsTrue(route.IsError);
            route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsTrue(route.IsError);

            // confirm access=no combined with bicycle=yes is working for bicycles.
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }

        /// <summary>
        /// An integration test that loads one way with bicycle=no.
        /// </summary>
        [Test]
        public void TestBicycleNo()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"),
                        Tag.Create("bicycle", "no"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle, Vehicle.Pedestrian);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's not working for bicycles.
            var route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsTrue(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsTrue(route.IsError);
        }

        /// <summary>
        /// An integration test that loads one way with highway=pedestrian.
        /// </summary>
        [Test]
        public void TestBicycleHighwayPedestrian()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "pedestrian"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Bicycle);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's not working for bicycles.
            var route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsTrue(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsTrue(route.IsError);
        }

        /// <summary>
        /// An integration test that loads one way with highway=pedestrian and bicycle=yes.
        /// </summary>
        [Test]
        public void TestBicycleHighwayPedestrianBicycleYes()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "pedestrian"),
                        Tag.Create("bicycle", "yes"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Bicycle);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's working for bicycles.
            var route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }

        /// <summary>
        /// An integration test that loads two overlapping ways, highway=pedestrian, and highway=residential
        /// </summary>
        [Test]
        public void TestOverlappingWaysPedestrianResidential()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 2,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "pedestrian"))
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle, Vehicle.Pedestrian);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's working for cars.
            var route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);

            // confirm it's working for pedestrians.
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }

        /// <summary>
        /// An integration test that loads two overlapping ways, highway=pedestrian, and highway=residential
        /// </summary>
        [Test]
        public void TestOverlappingWaysResidentialPedestrian()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"))
                },
                new Way()
                {
                    Id = 2,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "pedestrian"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle, Vehicle.Pedestrian);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's working for cars.
            var route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);

            // confirm it's working for pedestrians.
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }

        /// <summary>
        /// An integration test that loads two overlapping ways, highway=cycleway, and highway=residential,bicycle=no
        /// </summary>
        [Test]
        public void TestOverlappingWaysResidentialCycleway()
        {
            // the input osm-data.
            var osmGeos = new OsmGeo[]
            {
                new Node()
                {
                    Id = 1,
                    Latitude = 51.04963322083945,
                    Longitude = 3.719692826271057
                },
                new Node()
                {
                    Id = 2,
                    Latitude = 51.05062804602733,
                    Longitude = 3.7198376655578613
                },
                new Way()
                {
                    Id = 1,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "residential"),
                        Tag.Create("bicycle", "no"))
                },
                new Way()
                {
                    Id = 2,
                    Nodes = new List<long>(new long[]
                    {
                        1, 2
                    }),
                    Tags = new TagsCollection(
                        Tag.Create("highway", "cycleway"))
                }
            }.ToOsmStreamSource();

            // build router db.
            var routerDb = new RouterDb();
            routerDb.LoadOsmData(osmGeos, Vehicle.Car, Vehicle.Bicycle, Vehicle.Pedestrian);

            // test some routes.
            var router = new Router(routerDb);

            // confirm it's working for cars.
            var route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Car.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);

            // confirm it's working for bicycle.
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.04963322083945, 3.719692826271057),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613));
            Assert.IsFalse(route.IsError);
            route = router.TryCalculate(Vehicle.Bicycle.Fastest(),
                new GeoCoordinate(51.05062804602733, 3.7198376655578613),
                new GeoCoordinate(51.04963322083945, 3.719692826271057));
            Assert.IsFalse(route.IsError);
        }
    }
}