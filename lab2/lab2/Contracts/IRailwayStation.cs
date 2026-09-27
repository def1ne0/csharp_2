namespace lab2.Contracts;
using Entities;
using Collections;
using System.Numerics;

public interface IRailwayStation
{
    void AddTariff(Tariff tariff);
    void RegisterPassenger(Passenger passenger);
    static abstract void BuyTicket(Passenger passenger, Ticket ticket);
    static abstract TAmount CalculateTotalCost<TAmount>(Passenger passenger)
        where TAmount : INumber<TAmount>;
    MyCustomCollection<Passenger> GetPassengersByDestination(string destination);
}