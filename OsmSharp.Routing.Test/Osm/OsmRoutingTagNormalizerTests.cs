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
using OsmSharp.Collections.Tags;
using OsmSharp.Routing.Osm;

namespace OsmSharp.Routing.Test.Osm
{
    /// <summary>
    /// Contains tests for the routing tag normalizer.
    /// </summary>
    [TestFixture]
    public class OsmRoutingTagNormalizerTests
    {
        /// <summary>
        /// Test some default highway definitions.
        /// </summary>
        [Test]
        public void TestHighway()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            Assert.IsFalse(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));

            tags.Add(Tag.Create("highway", "residential"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "footway"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "footway"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "motorway"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "motorway"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests bicycle restriction normalization.
        /// </summary>
        [Test]
        public void TestBicycleRestrictions()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();
            var vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Bicycle };

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("bicycle", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("bicycle", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("bicycle", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("bicycle", "no"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("bicycle", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("bicycle", "mistake"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "footway"));
            tags.Add(Tag.Create("bicycle", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "footway"));
            Assert.IsFalse(profileTags.ContainsKeyValue("bicycle", "no"));
            profileTags.Clear();
            tags.Clear();

            vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Car };
            
            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("bicycle", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("bicycle", "no"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests foot restriction normalization.
        /// </summary>
        [Test]
        public void TestFootRestrictions()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();
            var vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Pedestrian };

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("foot", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("foot", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("foot", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("foot", "no"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("foot", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("foot", "mistake"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "cycleway"));
            tags.Add(Tag.Create("foot", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "cycleway"));
            Assert.IsTrue(profileTags.ContainsKeyValue("foot", "no"));
            profileTags.Clear();
            tags.Clear();

            vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Car };
            
            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("foot", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("foot", "no"));
            profileTags.Clear();
            tags.Clear();

            vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Bicycle };

            tags.Add(Tag.Create("highway", "cycleway"));
            tags.Add(Tag.Create("foot", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "cycleway"));
            Assert.IsFalse(profileTags.ContainsKeyValue("foot", "no"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests motorvehicle restriction normalization.
        /// </summary>
        [Test]
        public void TestMotorvehicleRestrictions()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();
            var vehicles = new Routing.Osm.Vehicles.Vehicle[] { Routing.Osm.Vehicles.Vehicle.Car };

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("motor_vehicle", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("motor_vehicle", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("motor_vehicle", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("motorcar", "no"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("motor_vehicle", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("motor_vehicle", "mistake"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "cycleway"));
            tags.Add(Tag.Create("motor_vehicle", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "cycleway"));
            Assert.IsFalse(profileTags.ContainsKeyValue("motorcar", "no"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests oneway restriction normalization.
        /// </summary>
        [Test]
        public void TestOnewayRestrictions()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("oneway", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("oneway", "no"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("oneway", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("oneway", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("oneway", "-1"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("oneway", "-1"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests access restriction normalization.
        /// </summary>
        [Test]
        public void TestAccessRestrictions()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("access", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("access", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("access", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("access", "mistake"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests junction normalization.
        /// </summary>
        [Test]
        public void TestJunction()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("junction", "roundabout"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("junction", "roundabout"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("junction", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("junction", "mistake"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests maxspeed normalization.
        /// </summary>
        [Test]
        public void TestMaxspeed()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("maxspeed", "50"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("maxspeed", "50"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("maxspeed", "mistake"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsFalse(profileTags.ContainsKeyValue("maxspeed", "mistake"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "residential"));
            tags.Add(Tag.Create("maxspeed", "50 mph"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "residential"));
            Assert.IsTrue(profileTags.ContainsKeyValue("maxspeed", "50 mph"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests normalization with steps and ramp.
        /// </summary>
        [Test]
        public void TestRamp()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "steps"));
            tags.Add(Tag.Create("ramp", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, Routing.Osm.Vehicles.Vehicle.GetAllRegistered()));
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "steps"));
            Assert.IsTrue(profileTags.ContainsKeyValue("ramp", "yes"));
            profileTags.Clear();
            tags.Clear();
        }

        /// <summary>
        /// Tests motorway access tags.
        /// </summary>
        [Test]
        public void TestMotorwayAccess()
        {
            Routing.Osm.Vehicles.Vehicle.RegisterVehicles();
            var vehicles = new Routing.Osm.Vehicles.Vehicle[] {
                Routing.Osm.Vehicles.Vehicle.Pedestrian,
                Routing.Osm.Vehicles.Vehicle.Bicycle,
                Routing.Osm.Vehicles.Vehicle.Car
            };

            var tags = new TagsCollection();
            var profileTags = new TagsCollection();
            var metaTags = new TagsCollection();

            tags.Add(Tag.Create("highway", "motorway"));
            tags.Add(Tag.Create("access", "no"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.AreEqual(2, profileTags.Count);
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "motorway"));
            Assert.IsTrue(profileTags.ContainsKeyValue("motorcar", "no"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "motorway"));
            tags.Add(Tag.Create("access", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.AreEqual(3, profileTags.Count);
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "motorway"));
            Assert.IsTrue(profileTags.ContainsKeyValue("bicycle", "yes"));
            Assert.IsTrue(profileTags.ContainsKeyValue("foot", "yes"));
            profileTags.Clear();
            tags.Clear();

            tags.Add(Tag.Create("highway", "motorway"));
            tags.Add(Tag.Create("access", "no"));
            tags.Add(Tag.Create("vehicle", "yes"));
            Assert.IsTrue(tags.Normalize(profileTags, metaTags, vehicles));
            Assert.AreEqual(2, profileTags.Count);
            Assert.IsTrue(profileTags.ContainsKeyValue("highway", "motorway"));
            Assert.IsTrue(profileTags.ContainsKeyValue("bicycle", "yes"));
            profileTags.Clear();
            tags.Clear();
        }
    }
}