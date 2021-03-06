using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static Tests.DummyFactory;

namespace Tests {
	public class ConnectorTester {
		
		[SetUp]
		public void SetUp() {
			SetUpDataHolder();
		}
		
		
		[TearDown]
		public void TearDown() {
			TearDownDataHolder();
		}

		[Test]
		public void TestConnectorOneInOneOutBeltToBelt() {
			// Arrange
			int numberOfCases = 4;
			var connectors = new Connector[numberOfCases];
			connectors[0] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(1, 0), 1)
				},
			};
			connectors[1] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(1, 0), 1)
				},
			};
			connectors[2] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(1, 0), 1)
				},
			};
			connectors[3] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(1, 0), 1)
				},
			};

			// Act
			for (int i = 0; i < numberOfCases; i++) {
				FactorySimulator.UpdateConnector(connectors[i]);
			}


			// Assert
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].outputs[0]));

			Assert.IsFalse(CheckBeltEmptyness(connectors[1].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[1].outputs[0]));

			Assert.IsTrue(CheckBeltEmptyness(connectors[3].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[3].outputs[0]));


			// only case where we see transportation
			Assert.IsTrue(CheckBeltEmptyness(connectors[2].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[2].outputs[0]));


			// Connectors take a few updates to transport items
			FactorySimulator.UpdateConnector(connectors[2]);

			Assert.IsTrue(CheckBeltEmptyness(connectors[2].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[2].outputs[0]));


			FactorySimulator.UpdateConnector(connectors[2]);

			Assert.IsTrue(CheckBeltEmptyness(connectors[2].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[2].outputs[0]));


			FactorySimulator.UpdateConnector(connectors[2]);

			Assert.IsTrue(CheckBeltEmptyness(connectors[2].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[2].outputs[0]));
		}




		[Test]
		public void TestConnectorOneInOneOutBuildingToBuilding() {
			// Arrange
			int numberOfCases = 4;
			var connectors = new Connector[numberOfCases];
			connectors[0] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBuilding(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBuilding(), new Position(1, 0), 1)
				},
			};
			connectors[1] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBuilding(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBuilding(), new Position(1, 0), 1)
				},
			};
			connectors[2] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBuilding(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBuilding(), new Position(1, 0), 1)
				},
			};
			connectors[3] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBuilding(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBuilding(), new Position(1, 0), 1)
				},
			};

			// Act
			for (int i = 0; i < numberOfCases; i++) {
				FactorySimulator.UpdateConnector(connectors[i]);
			}


			// Assert
			Assert.AreEqual(GetBuildingInputCount(connectors[0].inputs[0]), 0);
			Assert.AreEqual(GetBuildingOutputCount(connectors[0].inputs[0]), 0);
			Assert.AreEqual(GetBuildingInputCount(connectors[0].outputs[0]), 0);
			Assert.AreEqual(GetBuildingOutputCount(connectors[0].outputs[0]), 0);

			Assert.AreEqual(GetBuildingInputCount(connectors[1].inputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[1].inputs[0]), 1);
			Assert.AreEqual(GetBuildingInputCount(connectors[1].outputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[1].outputs[0]), 1);

			Assert.AreEqual(GetBuildingInputCount(connectors[3].inputs[0]), 0);
			Assert.AreEqual(GetBuildingOutputCount(connectors[3].inputs[0]), 0);
			Assert.AreEqual(GetBuildingInputCount(connectors[3].outputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[3].outputs[0]), 1);



			// Only case where we expect some movement
			Assert.AreEqual(GetBuildingInputCount(connectors[2].inputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].inputs[0]), 0);
			Assert.AreEqual(GetBuildingInputCount(connectors[2].outputs[0]), 0);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].outputs[0]), 0);



			// Movement takes 3 more ticks
			FactorySimulator.UpdateConnector(connectors[2]);


			Assert.AreEqual(GetBuildingInputCount(connectors[2].inputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].inputs[0]), 0);
			Assert.AreEqual(GetBuildingInputCount(connectors[2].outputs[0]), 0);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].outputs[0]), 0);


			FactorySimulator.UpdateConnector(connectors[2]);
			FactorySimulator.UpdateConnector(connectors[2]);

			Assert.AreEqual(GetBuildingInputCount(connectors[2].inputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].inputs[0]), 0);
			Assert.AreEqual(GetBuildingInputCount(connectors[2].outputs[0]), 1);
			Assert.AreEqual(GetBuildingOutputCount(connectors[2].outputs[0]), 0);

		}

		[Test]
		public void TestConnectorOneInOneOutBelttoBuilding() {
			// Arrange
			int numberOfCases = 2;
			var connectors = new Connector[numberOfCases];
			connectors[0] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBuilding(), new Position(1, 0), 1)
				},
			};
			connectors[1] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBuilding(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(1, 0), 1)
				},
			};

			// Act, do 4 ticks to complete movement
			for (int ticks = 0; ticks < 4; ticks++) {
				for (int i = 0; i < numberOfCases; i++) {
					FactorySimulator.UpdateConnector(connectors[i]);
				}
			}


			// Assert
			Assert.AreEqual(true, CheckBeltEmptyness(connectors[0].inputs[0]));
			Assert.AreEqual(1, GetBuildingInputCount(connectors[0].outputs[0]));
			Assert.AreEqual(0, GetBuildingOutputCount(connectors[0].outputs[0]));


			Assert.AreEqual(1, GetBuildingInputCount(connectors[1].inputs[0]));
			Assert.AreEqual(0, GetBuildingOutputCount(connectors[1].inputs[0]));
			Assert.AreEqual(false, CheckBeltEmptyness(connectors[1].outputs[0]));
		}


		[Test]
		// long connectors should take length + 2 ticks to transfer
		public void TestLongConnectors() {
			// Arrange
			int numberOfCases = 2;
			var connectors = new Connector[numberOfCases];
			// Should take 1 + 2 ticks
			connectors[0] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(4, 0), 1)
				},
			};

			// Should take 10 + 2 ticks
			connectors[1] = new Connector(new Position(), new Position()) {
				inputs = new List<Connector.Connection>() {
					new Connector.Connection(GetFullBelt(), new Position(0, 0), 1)
				},
				outputs = new List<Connector.Connection>() {
					new Connector.Connection(GetEmptyBelt(), new Position(9, 0), 1)
				},
			};

			// Act, do 5 tick to take item
			for (int i = 0; i < numberOfCases; i++) {
				FactorySimulator.UpdateConnector(connectors[i]);
			}



			// Assert
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].outputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[1].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[1].outputs[0]));

			// Act, do 6 ticks to complete movement for first one
			for (int ticks = 0; ticks < 6; ticks++) {
				for (int i = 0; i < numberOfCases; i++) {
					FactorySimulator.UpdateConnector(connectors[i]);
				}
			}


			// Assert
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[0].outputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[1].inputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[1].outputs[0]));


			// Act, do 5 more ticks to complete movement for the second one as well
			for (int ticks = 0; ticks < 6; ticks++) {
				for (int i = 0; i < numberOfCases; i++) {
					FactorySimulator.UpdateConnector(connectors[i]);
				}
			}


			// Assert
			Assert.IsTrue(CheckBeltEmptyness(connectors[0].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[0].outputs[0]));
			Assert.IsTrue(CheckBeltEmptyness(connectors[1].inputs[0]));
			Assert.IsFalse(CheckBeltEmptyness(connectors[1].outputs[0]));

		}

		//Ideally there will be other tests for cases like
		// two in one out
		// one in two out
		// cases when item types dont match
		// etc.

	}
}
