using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	// In real aplication it whould be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public void FillDriverToOrder(int driverId, TaxiOrder order)
		{
			order.FillDriverToOrder(driverId);
		}
	}

	public class Car : ValueType<Car>
	{
		public Car(string color, string model, string plateNumber)
		{
			Color = color;
			Model = model;
			PlateNumber = plateNumber;
		}

		public string Color { get; set; }
		public string Model { get; set; }
		public string PlateNumber { get; set; }
	}

	public class Driver : Entity<int>
	{
		public Driver(int id, PersonName name, Car car) : base(id)
		{
			Id = id;
			Name = name;
			Car = car;
		}

		public int Id { get; }
		public PersonName Name { get; set; }

		public Car Car { get; set; }
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private DriversRepository driversRepo;
		private Func<DateTime> currentTime;
		private int idCounter;

		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime) // +
		{
			this.driversRepo = driversRepo;
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building) // +
		{
			return new TaxiOrder(0).
				CreateOrderWithoutDestination(idCounter++, firstName, lastName, street, building, currentTime);
		}

		public void UpdateDestination(TaxiOrder order, string street, string building) // +
		{
			order.UpdateDestination(new Address(street, building));
		}

		public void AssignDriver(TaxiOrder order, int driverId) // +
		{
			order.AssignDriver(driverId, driversRepo, currentTime);
		}

		public void UnassignDriver(TaxiOrder order) // +
		{
			order.UnassignDriver();
		}

		public string GetDriverFullInfo(TaxiOrder order) // +
		{
			return order.GetDriverFullInfo();
		}

		public string GetShortOrderInfo(TaxiOrder order) // +
		{
			return order.GetShortOrderInfo();
		}

		public void Cancel(TaxiOrder order) // +
		{
			order.Cancel(currentTime);
		}

		public void StartRide(TaxiOrder order) // +
		{
			order.StartRide(currentTime);
		}

		public void FinishRide(TaxiOrder order) // +
		{
			order.FinishRide(currentTime);
		}
	}

	public class TaxiOrder : Entity<int>
	{
		public TaxiOrder(int id) : base(id)
		{
			Driver = new Driver(default, null, null);
		}

		public PersonName ClientName { get; private set; }
		public Address Start { get; private set; }
		public Address Destination { get; private set; }
		public Driver Driver { get; private set; }

		public TaxiOrderStatus Status { get; private set; }

		public DateTime CreationTime { get; private set; }
		public DateTime DriverAssignmentTime { get; private set; }
		public DateTime CancelTime { get; private set; }
		public DateTime StartRideTime { get; private set; }
		public DateTime FinishRideTime { get; private set; }

		public TaxiOrder CreateOrderWithoutDestination(
			int id, string firstName, string lastName, 
			string street, string building, Func<DateTime> currentTime) // +
		{
			return
				new TaxiOrder(id)
				{
					ClientName = new PersonName(firstName, lastName),
					Start = new Address(street, building),
					CreationTime = currentTime()
				};
		}

		public void UpdateDestination(Address destination) // +
		{
			Destination = new Address(destination.Street, destination.Building);
		}

		public void AssignDriver(int driverId, DriversRepository driversRepo, Func<DateTime> currentTime) // +
		{
			if (Driver.Name != null)
				throw new InvalidOperationException();
			driversRepo.FillDriverToOrder(driverId, this);
			DriverAssignmentTime = currentTime();
			Status = TaxiOrderStatus.WaitingCarArrival;
		}

		public void UnassignDriver() // +
		{
			if (Status == TaxiOrderStatus.WaitingForDriver
				|| Status == TaxiOrderStatus.InProgress)
				throw new InvalidOperationException("WaitingForDriver");
			Driver = new Driver(0, null ,null);
			Status = TaxiOrderStatus.WaitingForDriver;
		}

		public string GetDriverFullInfo() // +
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + Driver.Id,
				"DriverName: " + FormatName(Driver.Name),
				"Color: " + Driver.Car.Color,
				"CarModel: " + Driver.Car.Model,
				"PlateNumber: " + Driver.Car.PlateNumber);
		}

		public string GetShortOrderInfo() // +
		{
			return string.Join(" ",
				"OrderId: " + Id,
				"Status: " + Status,
				"Client: " + FormatName(ClientName),
				"Driver: " + FormatName(Driver.Name),
				"From: " + FormatAddress(Start),
				"To: " + FormatAddress(Destination),
				"LastProgressTime: " + GetLastProgressTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		private DateTime GetLastProgressTime()
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return CreationTime;
			if (Status == TaxiOrderStatus.WaitingCarArrival) return DriverAssignmentTime;
			if (Status == TaxiOrderStatus.InProgress) return StartRideTime;
			if (Status == TaxiOrderStatus.Finished) return FinishRideTime;
			if (Status == TaxiOrderStatus.Canceled) return CancelTime;
			throw new NotSupportedException(Status.ToString());
		}

		private string FormatName(PersonName name)
		{
			if (name == null) return "";
			return string.Join(" ", new[] { name.FirstName, name.LastName }.Where(n => n != null));
		}

		private string FormatAddress(Address address)
		{
			if (address == null) return "";
			return string.Join(" ", new[] { address.Street, address.Building }.Where(n => n != null));
		}

		public void Cancel(Func<DateTime> currentTime)
		{	
			if (Status == TaxiOrderStatus.Canceled 
				|| Status == TaxiOrderStatus.Finished
				|| Status == TaxiOrderStatus.InProgress)
				throw new InvalidOperationException();
			Status = TaxiOrderStatus.Canceled;
			CancelTime = currentTime();
		}

		public void StartRide(Func<DateTime> currentTime)
		{
			if (Status != TaxiOrderStatus.WaitingCarArrival)
				throw new InvalidOperationException();
			Status = TaxiOrderStatus.InProgress;
			StartRideTime = currentTime();
		}

		public void FinishRide(Func<DateTime> currentTime)
		{
			if (Status != TaxiOrderStatus.InProgress)
				throw new InvalidOperationException();
			Status = TaxiOrderStatus.Finished;
			FinishRideTime = currentTime();
		}

		public void FillDriverToOrder(int driverId)
		{
			if (driverId == 15)
			{
				Driver = new Driver(driverId, new PersonName("Drive", "Driverson"), 
					new Car("Baklazhan", "Lada sedan", "A123BT 66"));
			}
			else
				throw new Exception("Unknown driver id " + driverId);
		}
	}
}