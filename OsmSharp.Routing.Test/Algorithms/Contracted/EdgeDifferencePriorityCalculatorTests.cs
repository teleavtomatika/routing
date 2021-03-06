﻿// OsmSharp - OpenStreetMap (OSM) SDK
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
using OsmSharp.Collections.LongIndex.LongIndex;
using OsmSharp.Routing.Algorithms.Contracted;
using OsmSharp.Routing.Data.Contracted;
using OsmSharp.Routing.Graphs.Directed;
using System;
using System.Linq;

namespace OsmSharp.Routing.Test.Algorithms.Contracted
{
    /// <summary>
    /// Containts tests for the edge difference priority calculator.
    /// </summary>
    [TestFixture]
    public class EdgeDifferencePriorityCalculatorTests
    {
        /// <summary>
        /// Tests calculator with no neighbours.
        /// </summary>
        [Test]
        public void TestNoNeighbours()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph, 
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with one neighbour and no witnesses.
        /// </summary>
        [Test]
        public void TestOneNeighbour()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(-1, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighbours()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with three neighbours and no witnesses.
        /// </summary>
        [Test]
        public void TestThreeNeighbours()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(0, 3, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(3, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(3, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours, oneway edges, and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighboursOneWay()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, false, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, false, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, true, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(0, priority);

            // build another graph.
            graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, false, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, false, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours, oneway opposite edges, and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighboursOneWayOpposite()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, false, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, false, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new LongIndex(), 0);

            Assert.AreEqual(-2, priority);
        }

        /// <summary>
        /// Tests calculator with one neighbour but with contracted neighbour.
        /// </summary>
        [Test]
        public void TestOneNeighboursContracted()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, false, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new LongIndex();
            contractedFlags.Add(1);
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(contractedFlags, 0);

            Assert.AreEqual(-2, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with one contracted neighbour.
        /// </summary>
        [Test]
        public void TestTwoNeighboursContracted()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new LongIndex();
            contractedFlags.Add(1);
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(contractedFlags, 0);

            Assert.AreEqual(-3, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with contracted neighbour.
        /// </summary>
        [Test]
        public void TestOneNeighboursNotifyContracted()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, false, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new LongIndex();
            contractedFlags.Add(1);
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            priorityCalculator.NotifyContracted(1);
            var priority = priorityCalculator.Calculate(contractedFlags, 0);

            Assert.AreEqual(1, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with one contracted neighbour.
        /// </summary>
        [Test]
        public void TestTwoNeighboursNotifyContracted()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 1, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(1, 0, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(0, 2, 100, null, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, null, Constants.NO_VERTEX);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new LongIndex();
            contractedFlags.Add(1);
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            priorityCalculator.NotifyContracted(1);
            var priority = priorityCalculator.Calculate(contractedFlags, 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator on a small network with 4 vertices in a quadrilateral.
        /// </summary>
        [Test]
        public void TestQuadrilateralOneWay()
        {
            // build graph.
            var graph = new DirectedMetaGraph(ContractedEdgeDataSerializer.Size,
                ContractedEdgeDataSerializer.MetaSize);
            graph.AddEdge(0, 2, 100, true, Constants.NO_VERTEX);
            graph.AddEdge(2, 0, 100, false, Constants.NO_VERTEX);
            graph.AddEdge(0, 3, 10, false, Constants.NO_VERTEX);
            graph.AddEdge(3, 0, 10, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 2, 1000, false, Constants.NO_VERTEX);
            graph.AddEdge(2, 1, 1000, true, Constants.NO_VERTEX);
            graph.AddEdge(1, 3, 10000, true, Constants.NO_VERTEX);
            graph.AddEdge(3, 1, 10000, false, Constants.NO_VERTEX);
            graph.Compress();

            // create a witness calculator and the priority calculator.
            var contractedFlags = new LongIndex();
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock(new uint[][]
                    {
                        new uint[] { 1, 3, 2, 1 },
                        new uint[] { 3, 0, 1, 1 }
                    }));

            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, 0));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, 1));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, 2));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, 3));
        }
    }
}