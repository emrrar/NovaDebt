﻿using NovaDebt.Models.Enums;

namespace NovaDebt.Models.Contracts
{
    public interface ITransactor
    {
        int Id { get; }

        string Name { get; }

        string PhoneNumber { get; }

        string Email { get; }

        string Facebook { get; }

        decimal Amount { get; }

        string TransactorType { get; }
    }
}