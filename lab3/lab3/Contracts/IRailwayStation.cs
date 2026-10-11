namespace lab3.Contracts;
using Entities;
using System.Numerics;

public interface IRailwayStation
{
    void SetTariff(string dest, Tariff tariff);
    void RegisterPassenger(Passenger passenger);
    static abstract void BuyTicket(Passenger passenger, Ticket ticket);
    static abstract TAmount CalculateTotalCost<TAmount>(Passenger passenger)
        where TAmount : INumber<TAmount>;
    List<Passenger> GetPassengersByDestination(string destination);
}